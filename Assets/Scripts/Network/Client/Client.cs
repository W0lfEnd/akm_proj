using Lidgren.Network;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Client : MonoBehaviour, IClient
{
    [SerializeField] private TMP_InputField txtIP;
    [SerializeField] private TMP_InputField txtPort;
    [SerializeField] private TMP_InputField txtName;

    public static IClient client;

    public GameModel Model { get; set; }
    public Map Map { get; set; }
    public long Id { get; set; }
    public string Nickname => txtName.text;


    private NetClient _client;
    private NetConnection _connection;
    private PacketHandlerManager _packetHandlerManager;
    private bool _isConnected;

    public Dictionary<long, string> otherClients = new Dictionary<long, string>();
    public List<ServerInfo> availableServers = new List<ServerInfo>();

    public void Send(PlayerInput playerInput)
    {
        var message = _client.CreateMessage();
        message.Write(PacketFactory.CreatePacketByType( PacketType.C2S_Input, playerInput).GetData());
        _client.SendMessage(message, _connection, NetDeliveryMethod.ReliableOrdered);
    }

    public void StartFoundServer()
    {
        if(_client == null)
        {
            return;
        }
        _client.DiscoverLocalPeers(Convert.ToInt32(CommonData.DefaultServerPort));
    }

    public void Connect( bool isLocal )
    {
        var host = txtIP == null || string.IsNullOrWhiteSpace(txtIP.text) || isLocal ? CommonData.DefaultIPAddress : txtIP.text;
        var port = txtPort == null || string.IsNullOrWhiteSpace(txtPort.text) ? CommonData.DefaultServerPort : txtPort.text;

        var config = new NetPeerConfiguration(CommonData.DefaultHostName);
        config.Port = Convert.ToUInt16(CommonData.DefaultClientPort);
        config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

        _client = new NetClient(config);
        _client.Start();
        
        NetOutgoingMessage message = _client.CreateMessage(CommonData.DefaultHostName);
        _connection = _client.Connect(host, Convert.ToUInt16(port), message);
        print("Clinet connecting ...");

        StartCoroutine(ClientConnected());
    }

    IEnumerator ClientConnected()
    {
        while (_connection != null && _connection.Status != NetConnectionStatus.Connected)
        {
            yield return null;
        }
        _isConnected = true;
        Id = _client.UniqueIdentifier;
        var name = txtName == null || string.IsNullOrWhiteSpace( txtName.text) ? CommonData.NickName : txtName.text;

        var message = _client.CreateMessage();
        message.Write(PacketFactory.CreatePacketByType(PacketType.C2S_Join, new Tuple<long, string>(Id, name)).GetData());
        _client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
    }

    private void Awake()
    {
        if (client == null)
        {
            client = this;
        }

        _isConnected = false;
        _packetHandlerManager = new PacketHandlerManager();
        _packetHandlerManager.AddHandler(PacketType.S2C_Joined, new JoinedPacketHandler());
        _packetHandlerManager.AddHandler(PacketType.S2C_Map, new MapPacketHandler());
        _packetHandlerManager.AddHandler(PacketType.S2C_Model, new GameModelPacketHandler());
        _packetHandlerManager.AddHandler(PacketType.S2C_ServerInfo, new ServerInfoPacketHandler());
    }

    private void FixedUpdate()
    {
        if (_client != null)
        {
            NetIncomingMessage message;
            while ((message = _client.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryResponse:
                        _packetHandlerManager.RunPacketHandler(message.Data, this);
                        break;
                    case NetIncomingMessageType.Data:
                        if (_connection != null)
                        {
                            _packetHandlerManager.RunPacketHandler(message.Data, this);
                        }
                        break;
                    default: print($"Client {nameof(message.MessageType)} -> {message.MessageType}"); break;
                }
            }
        }
    }
}
