using System;
using TMPro;
using UnityEngine;
using static WebSocketPlugin;

public class Client : MonoBehaviour
{
    [SerializeField] private TMP_InputField txtIP;
    [SerializeField] private TMP_InputField txtPort;

    private WebSocketPlugin _client;
    private PacketHandlerManager _packetHandlerManager;
    private bool _isConnected;

    public void Connect()
    {
        var host = txtIP == null || string.IsNullOrWhiteSpace(txtIP.text) ? CommonConstants.DefaultIPAddress : txtIP.text;
        var port = txtPort == null || string.IsNullOrWhiteSpace(txtPort.text) ? CommonConstants.DefaultPort : txtPort.text;

        var uri = new Uri($"ws://{host}:{port}/{CommonConstants.DefaultHostName}");
        _client = new WebSocketPlugin(uri);
        _client.ChangeStateEvent += OnChangeState;
        _client.Connect();
    }

    private void OnChangeState(SocketActionState actionState)
    {
        if (actionState == SocketActionState.MESSAGE)
        {
            var message = _client.Recv();
            if (message == null)
            {
                return;
            }
            _packetHandlerManager.RunPacketHandler(message, this);
        }
        else if (actionState ==  SocketActionState.OPEN)
        {
            _isConnected = true;
        }
        else if (actionState == SocketActionState.CLOSE)
        {
            print(actionState.ToString());
        }
        else if (actionState == SocketActionState.ERROR)
        {
            print(_client.error);
            print(actionState.ToString());
        }
    }

    private void Awake()
    {
        _isConnected = false;
        _packetHandlerManager = new PacketHandlerManager();
    }
}
