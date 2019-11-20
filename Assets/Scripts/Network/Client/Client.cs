using Lidgren.Network;
using Model;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
//using static WebSocketPlugin;

public class Client : MonoBehaviour, IClient
{
    [SerializeField] private TMP_InputField txtIP;
    [SerializeField] private TMP_InputField txtPort;

    public static IClient client;

    public GameModel Model { get; set; }
    public Map Map { get; set; }
    public int Id { get; set; }

    private NetClient _client;
    private NetConnection _connection;
    private PacketHandlerManager _packetHandlerManager;
    private bool _isConnected;

    public void Send(PlayerInput playerInput)
    {
        var message = _client.CreateMessage();
        message.Write(PacketFactory.CreatePacketByType( PacketType.C2S_Input, playerInput).GetData());
        _client.SendMessage(message, _connection, NetDeliveryMethod.ReliableOrdered);
    }

    public void Connect()
    {
        var host = txtIP == null || string.IsNullOrWhiteSpace(txtIP.text) ? CommonConstants.DefaultIPAddress : txtIP.text;
        var port = txtPort == null || string.IsNullOrWhiteSpace(txtPort.text) ? CommonConstants.DefaultServerPort : txtPort.text;

        var config = new NetPeerConfiguration(CommonConstants.DefaultHostName);
        config.Port = Convert.ToUInt16(CommonConstants.DefaultClientPort);
        config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

        _client = new NetClient(config);
        _client.Start();
        
        NetOutgoingMessage message = _client.CreateMessage(CommonConstants.DefaultHostName);
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
    }

    private void Awake()
    {
        if (client == null)
        {
            client = this;
        }

        _isConnected = false;
        _packetHandlerManager = new PacketHandlerManager();
        _packetHandlerManager.AddHandler(PacketType.S2C_SendId, new SendIdPacketHandler());
        _packetHandlerManager.AddHandler(PacketType.S2C_Map, new MapPacketHandler());
        _packetHandlerManager.AddHandler(PacketType.S2C_Model, new GameModelPacketHandler());
    }

    private void FixedUpdate()
    {
        if (_client != null && _connection != null)
        {
            NetIncomingMessage message;
            while ((message = _client.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        _packetHandlerManager.RunPacketHandler(message.Data, this);
                        break;
                    default: print($"Client {nameof(message.MessageType)} -> {message.MessageType}"); break;
                }
            }
        }
    }
}
