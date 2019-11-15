using Lidgren.Network;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class Server : MonoBehaviour//, ISubscriber
{
    [SerializeField] private TMP_InputField txtIP; 
    [SerializeField] private TMP_InputField txtPort;
    [SerializeField] private TMP_InputField txtMaxPlayerCount;
    [SerializeField] private Client client;

    //private WebSocketServer _server;
    private static NetServer _server;
    private static List<ClientInfo> _clients;
    private PacketHandlerManager _packetHandlerManager;
    private bool _isRun;
    private byte _maxPlayerCount;
    private GameController _gameController;

    public GameController GameController => _gameController;

    public void Run()
    {
        if (_server != null && (_server.Status == NetPeerStatus.Running || _server.Status == NetPeerStatus.Starting))
        {
            return;
        }

        var host = txtIP == null || string.IsNullOrWhiteSpace(txtIP.text) ? CommonConstants.DefaultIPAddress : txtIP.text;
        var port = txtPort == null || string.IsNullOrWhiteSpace(txtPort.text) ? CommonConstants.DefaultPort : txtPort.text;
        _maxPlayerCount = txtMaxPlayerCount == null || string.IsNullOrWhiteSpace(txtMaxPlayerCount.text) ? (byte)1 : System.Convert.ToByte(txtMaxPlayerCount);

        var uri = $"ws://{host}:{port}";

        //_server = new WebSocketServer(uri);
        //_server.Log.Level = LogLevel.Trace;

        //_server.AddWebSocketService<GameWebSocketBehavior>($"/{CommonConstants.DefaultHostName}");
        //_server.Start();
        NetPeerConfiguration config = new NetPeerConfiguration(CommonConstants.DefaultHostName);
        config.Port = int.Parse(port);
        config.LocalAddress = NetUtility.Resolve(host);
        config.MaximumConnections = _maxPlayerCount;
        config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
        config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

        _server = new NetServer(config);
        _server.RegisterReceivedCallback(ReceiveData);
        _server.Start();

        _gameController = new GameController();

        Debug.Log("Server started");
    }

    /*void ISubscriber.ReceiveEvent(EventType name, object payload)
    {
        switch (name)
        {
            case EventType.WebSocketDataConnceted:
                var connClient = payload as ClientInfo;
                if (connClient == null) return;
                OnConnect(connClient);
                break;
            case EventType.WebSocketDataDisconnceted:
                var discClient = payload as ClientInfo;
                if (discClient == null) return;
                OnDisconnect(discClient);
                break;
            case EventType.WebSocketDataReceived:
                var receivData = payload as byte[];
                if (receivData == null) return;
                OnReceive(receivData);
                break;
        }
    }*/

    private void OnConnect(ClientInfo clientInfo)
    {
        _isRun = true;
        if (_clients.Count < _maxPlayerCount)
        {
            _clients.Add(clientInfo);

            var messageId = _server.CreateMessage();
            messageId.Write(PacketFactory.CreatePacketByType(PacketType.S2C_SendId, 1000 + _clients.Count).GetData());
            _server.SendMessage(messageId, clientInfo.NetConnection, NetDeliveryMethod.ReliableOrdered);

            var messageMap = _server.CreateMessage();
            messageId.Write(PacketFactory.CreatePacketByType(PacketType.S2C_Map, _gameController.GetMap()).GetData());
            _server.SendMessage(messageId, clientInfo.NetConnection, NetDeliveryMethod.ReliableOrdered);

            var messageModel = _server.CreateMessage();
            messageId.Write(PacketFactory.CreatePacketByType(PacketType.S2C_Model, _gameController.GetModel()).GetData());
            _server.SendMessage(messageId, clientInfo.NetConnection, NetDeliveryMethod.ReliableOrdered);
        }
        clientInfo.Socket.Close(CloseStatusCode.TooBig);
    }

    private void OnDisconnect(ClientInfo clientInfo)
    {
        _clients.Remove(clientInfo);
    }

    private void OnReceive(byte[] message)
    {
        _packetHandlerManager.RunPacketHandler(message, this);
    }

    private void ReceiveData(object peer)
    {
        Debug.Log("ReceiveData");
        NetIncomingMessage message = _server.ReadMessage();
        if (message == null)
        {
            return;
        }

        switch (message.MessageType)
        {
            case NetIncomingMessageType.ConnectionApproval:
                Debug.Log("ConnectionApproval");
                string secretKey = message.ReadString();
                if (secretKey == "here have to be the secret key")
                {
                    message.SenderConnection.Approve();
                }
                else
                {
                    message.SenderConnection.Deny();
                }

                break;
            case NetIncomingMessageType.Data:
                Debug.Log("Data");
                _packetHandlerManager.RunPacketHandler(message.Data, this);
                //Receive(message.Data);
                break;
            case NetIncomingMessageType.StatusChanged:
                Debug.Log("StatusChanged");
                if (message.SenderConnection.Status == NetConnectionStatus.Connected)
                {
                    var clientInfo = new ClientInfo();
                    clientInfo.IpAddress = message.SenderConnection.RemoteEndPoint.Address;
                    clientInfo.Port = (ushort)message.SenderConnection.RemoteEndPoint.Port;
                    clientInfo.NetConnection = message.SenderConnection;
                    clientInfo.ConnectionId = message.SenderConnection.Peer.UniqueIdentifier.ToString();
                    _clients.Add(clientInfo);
                    OnConnect(clientInfo);
                }
                else if (message.SenderConnection.Status == NetConnectionStatus.Disconnected)
                {
                    var client = _clients.FirstOrDefault(cl =>
                        cl.IpAddress == message.SenderConnection.RemoteEndPoint.Address
                        && cl.Port == message.SenderConnection.RemoteEndPoint.Port);
                    OnDisconnect(client);
                }

                break;
            case NetIncomingMessageType.DebugMessage:
            case NetIncomingMessageType.WarningMessage:
            case NetIncomingMessageType.ErrorMessage:
                Debug.Log("Debug");
                break;
            default:
                break;
        }

        _server.Recycle(message);
    }


    private void Awake()
    {
        //SimpleEventBus.SubscribeOnEvent(TopicType.ServiceToServer, EventType.WebSocketDataConnceted, this);
        //SimpleEventBus.SubscribeOnEvent(TopicType.ServiceToServer, EventType.WebSocketDataDisconnceted, this);
        //SimpleEventBus.SubscribeOnEvent(TopicType.ServiceToServer, EventType.WebSocketDataReceived, this);

        _clients = new List<ClientInfo>();

        _packetHandlerManager = new PacketHandlerManager();
    }

    private static int iteration = 0;
    private void FixedUpdate()
    {
        if (!_isRun) return;
        iteration++;
        if ( iteration == 10)
        {
            _gameController.DoIteration(Time.realtimeSinceStartup);
            iteration = 0;
        }
    }
}
