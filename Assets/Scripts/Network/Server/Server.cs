using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class Server : MonoBehaviour, ISubscriber
{
    [SerializeField] private TMP_InputField txtIP; 
    [SerializeField] private TMP_InputField txtPort;
    [SerializeField] private TMP_InputField txtMaxPlayerCount;
    [SerializeField] private Client client;

    private WebSocketServer _server;
    private static List<ClientInfo> _clients;
    private PacketHandlerManager _packetHandlerManager;
    private bool _isRun;
    private byte _maxPlayerCount;
    private GameController _gameController;

    public GameController GameController => _gameController;

    public void Run()
    {
        if (_server != null)
        {
            return;
        }

        var host = txtIP == null || string.IsNullOrWhiteSpace(txtIP.text) ? CommonConstants.DefaultIPAddress : txtIP.text;
        var port = txtPort == null || string.IsNullOrWhiteSpace(txtPort.text) ? CommonConstants.DefaultPort : txtPort.text;
        _maxPlayerCount = txtMaxPlayerCount == null || string.IsNullOrWhiteSpace(txtMaxPlayerCount.text) ? (byte)1 : System.Convert.ToByte(txtMaxPlayerCount);

        var uri = $"ws://{host}:{port}";

        _server = new WebSocketServer(uri);
        _server.Log.Level = LogLevel.Trace;

        _server.AddWebSocketService<GameWebSocketBehavior>($"/{CommonConstants.DefaultHostName}");
        _server.Start();
        
        client.Connect();

        _gameController = new GameController();

        Debug.Log("Server started");
    }

    void ISubscriber.ReceiveEvent(EventType name, object payload)
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
    }

    private void OnConnect(ClientInfo clientInfo)
    {
        _isRun = true;
        if (_clients.Count < _maxPlayerCount)
        {
            _clients.Add(clientInfo);
            clientInfo.Socket.Send(PacketFactory.CreatePacketByType(PacketType.S2C_SendId, 1000 + _clients.Count).GetData());
            clientInfo.Socket.Send(PacketFactory.CreatePacketByType(PacketType.S2C_Map, _gameController.GetMap()).GetData());
            clientInfo.Socket.Send(PacketFactory.CreatePacketByType(PacketType.S2C_Model, _gameController.GetModel()).GetData());
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

    private void Awake()
    {
        SimpleEventBus.SubscribeOnEvent(TopicType.ServiceToServer, EventType.WebSocketDataConnceted, this);
        SimpleEventBus.SubscribeOnEvent(TopicType.ServiceToServer, EventType.WebSocketDataDisconnceted, this);
        SimpleEventBus.SubscribeOnEvent(TopicType.ServiceToServer, EventType.WebSocketDataReceived, this);

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
