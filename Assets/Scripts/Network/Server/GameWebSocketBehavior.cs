using System.Linq;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class GameWebSocketBehavior : WebSocketBehavior
{
    protected override void OnClose(CloseEventArgs e)
    {
        Debug.Log("Close");
        Debug.Log(e.Reason);
        Debug.Log(e.Code);
        Debug.Log(e.WasClean);
        
        var clientInfo = new ClientInfo();

        clientInfo.IpAddress = Context.UserEndPoint.Address;
        clientInfo.Port = (ushort)Context.UserEndPoint.Port;
        clientInfo.Socket = Context.WebSocket;
        // verify session.id for new client
        clientInfo.ConnectionId = string.Empty;
        SimpleEventBus.SendEvent(TopicType.ServiceToServer, EventType.WebSocketDataDisconnceted, clientInfo);
    }

    protected override void OnError(ErrorEventArgs e)
    {
        Debug.Log("Error");
        Debug.Log(e.Message);
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        Debug.Log("Message");
        if (e.IsBinary)
        {
            SimpleEventBus.SendEvent(TopicType.ServiceToServer, EventType.WebSocketDataReceived, e.RawData);
        }
        else if (e.IsText)
        {
            Debug.Log(e.Data);
        }
    }

    protected override void OnOpen()
    {
        Debug.Log("OPENING");
        var clientInfo = new ClientInfo();

        clientInfo.IpAddress = Context.UserEndPoint.Address;
        clientInfo.Port = (ushort)Context.UserEndPoint.Port;
        clientInfo.Socket = Context.WebSocket;
        // verify session.id for new client
        clientInfo.ConnectionId = Sessions.IDs.Last();
        SimpleEventBus.SendEvent(TopicType.ServiceToServer, EventType.WebSocketDataConnceted, clientInfo);
        
        Debug.Log("OPENED");
    }
}

