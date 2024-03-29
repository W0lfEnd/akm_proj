﻿using Lidgren.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TMPro;
using UnityEngine;

public class Server : MonoBehaviour
{
    [SerializeField] private TMP_InputField txtIP; 
    [SerializeField] private TMP_InputField txtPort;
    [SerializeField] private TMP_InputField txtMaxPlayerCount;
    [SerializeField] private Client client;


    public NetServer _server;
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

        var host = txtIP == null || string.IsNullOrWhiteSpace(txtIP.text) ? CommonData.DefaultIPAddress : txtIP.text;
        var port = txtPort == null || string.IsNullOrWhiteSpace(txtPort.text) ? CommonData.DefaultServerPort : txtPort.text;
        _maxPlayerCount = txtMaxPlayerCount == null || string.IsNullOrWhiteSpace(txtMaxPlayerCount.text) ? (byte)1 : System.Convert.ToByte(txtMaxPlayerCount.text);

        NetPeerConfiguration config = new NetPeerConfiguration(CommonData.DefaultHostName);
        config.Port = int.Parse(port);
        config.LocalAddress = NetUtility.Resolve(host);
        config.MaximumConnections = _maxPlayerCount;
        config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
        config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
        config.EnableMessageType(NetIncomingMessageType.StatusChanged);
        config.EnableMessageType(NetIncomingMessageType.Data);
        config.EnableMessageType(NetIncomingMessageType.DebugMessage);
        config.EnableMessageType(NetIncomingMessageType.WarningMessage);
        config.EnableMessageType(NetIncomingMessageType.ErrorMessage);
        config.EnableMessageType(NetIncomingMessageType.Error);


        _server = new NetServer(config);
        _server.RegisterReceivedCallback(ReceiveData);
        _server.Start();

        CommonData.DefaultIPAddress = host;

        _gameController = new GameController();
        StartCoroutine(ClientConnect());
        StartCoroutine(IterationEachSecond());

        Debug.Log("Server starting");
    }

    IEnumerator ClientConnect()
    {
        yield return new WaitForSeconds(0.5f);

        if (_server.Status != NetPeerStatus.Running)
        {
            Debug.Log("Server NOT started");
        }
        else
        {
            Debug.Log("Server started");
            client.Connect( true );
        }
    }

    IEnumerator IterationEachSecond()
    {
        do
        {
            yield return new WaitForSeconds(1);
            if (_server.Status == NetPeerStatus.Running)
            {
                _gameController.DoIterationOnEachSeconds();
            }
        } while (true);
    }

    private void OnConnect(ClientInfo clientInfo)
    {
        _isRun = true;
        if (_clients.Count <= _maxPlayerCount)
        {
            _clients.Add(clientInfo);

            var packetMap = PacketFactory.CreatePacketByType(PacketType.S2C_Map, _gameController.GetMap());
            var messageMap = _server.CreateMessage();
            messageMap.Write(packetMap.GetData());
            _server.SendMessage(messageMap, clientInfo.NetConnection, NetDeliveryMethod.ReliableOrdered);
            packetMap.Dispose();

            var messageModel = _server.CreateMessage();
            var packetModel = PacketFactory.CreatePacketByType(PacketType.S2C_Model, _gameController.GetModel());
            messageModel.Write(packetModel.GetData());
            _server.SendMessage(messageModel, clientInfo.NetConnection, NetDeliveryMethod.ReliableOrdered);
            packetModel.Dispose();
        }
    }

    private void OnDisconnect(ClientInfo clientInfo)
    {
        _clients.Remove(clientInfo);
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
                if (secretKey == CommonData.DefaultHostName)
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
                break;
            case NetIncomingMessageType.StatusChanged:
                Debug.Log("StatusChanged");
                if (message.SenderConnection.Status == NetConnectionStatus.Connected)
                {
                    var clientInfo = new ClientInfo();
                    clientInfo.IpAddress = message.SenderConnection.RemoteEndPoint.Address;
                    clientInfo.Port = (ushort)message.SenderConnection.RemoteEndPoint.Port;
                    clientInfo.NetConnection = message.SenderConnection;
                    clientInfo.ConnectionId = message.SenderConnection.Peer.UniqueIdentifier;
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
                Debug.Log("Debug");
                break;
            case NetIncomingMessageType.WarningMessage:
                Debug.Log("WarningMessage");
                break;
            case NetIncomingMessageType.ErrorMessage:
                Debug.Log("ErrorMessage");
                break;
            default:
                break;
        }

        _server.Recycle(message);
    }


    private void Awake()
    {
        _clients = new List<ClientInfo>();

        _packetHandlerManager = new PacketHandlerManager();
        _packetHandlerManager.AddHandler(PacketType.C2S_Input, new PlayerInputPacketHandler());
        _packetHandlerManager.AddHandler(PacketType.C2S_Join, new JoinPacketHandler());

        if (txtIP != null)
        {
            foreach(IPAddress ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    txtIP.text = ip.MapToIPv4().ToString();
                    Debug.Log(ip.MapToIPv4().ToString());
                }
            }
        }
    }

    private static int iteration = 0;
    private void FixedUpdate()
    {
        if (!_isRun) return;
        iteration++;
        if ( iteration >= 10)
        {
            iteration = 0;
            _gameController.DoMeteorIteration();
            Debug.Log( "Iterate successful" );
            var messageModel = _server.CreateMessage();
            var packetModel = PacketFactory.CreatePacketByType(PacketType.S2C_Model, _gameController.GetModel());
            messageModel.Write(packetModel.GetData());
            _server.SendToAll(messageModel, NetDeliveryMethod.ReliableOrdered);
            packetModel.Dispose();
        }
    }
}
