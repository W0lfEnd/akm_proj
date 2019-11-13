using System;
using System.Collections.Generic;
using System.Text;

public class WebSocketPlugin
{
    public enum SocketActionState
    {
        NONE,

        OPEN,
        MESSAGE,
        CLOSE,
        ERROR
    }

	private Uri _mUrl;

    public delegate void ChangeState(SocketActionState state);
    public event ChangeState ChangeStateEvent;

    public WebSocketPlugin(Uri url)
	{
		_mUrl = url;

		string protocol = _mUrl.Scheme;
        if (!protocol.Equals("ws") && !protocol.Equals("wss"))
        {
            throw new ArgumentException("Unsupported protocol: " + protocol);
        }
	}	

	public void SendString(string str)
	{
        var result = Encoding.UTF8.GetBytes(str);

        Send(result);
	}

	public string RecvString()
	{
		byte[] retval = Recv();
        if (retval == null)
        {
            return null;
        }

        var result = Encoding.UTF8.GetString(retval);

        return result;
	}
    
	WebSocketSharp.WebSocket m_Socket;
	Queue<byte[]> m_Messages = new Queue<byte[]>();
	bool m_IsConnected = false;
	string m_Error = null;

    public void Connect(string nickname = null)
	{
		var builder = new UriBuilder(_mUrl);
		_mUrl = builder.Uri;
		m_Socket = new WebSocketSharp.WebSocket(_mUrl.ToString());
		m_Socket.OnMessage += OnMessage;
		m_Socket.OnOpen += OnOpen;
		m_Socket.OnError += OnError;
		m_Socket.OnClose += OnClose;
		m_Socket.Connect();
		
		while (m_IsConnected && m_Error == null)
		{
			return;
		}
	}

    public void Send(byte[] buffer)
	{
		if (m_Socket != null && m_Socket.ReadyState == WebSocketSharp.WebSocketState.Open)
		{
			m_Socket.Send(buffer);
		}
	}

	public byte[] Recv()
	{
		if (m_Socket == null || m_Messages.Count == 0)
		{
			return null;
		}

		return m_Messages.Dequeue();
	}

	public void Close()
	{
		if (m_Socket != null)
		{
			m_Socket.Close();
		}
	}

	public string error
	{
		get { return m_Error; }
	}

	private void OnClose(object sender, WebSocketSharp.CloseEventArgs e)
	{
		if (ChangeStateEvent != null)
		{
			ChangeStateEvent(SocketActionState.CLOSE);
		}
	}

	private void OnError(object sender, WebSocketSharp.ErrorEventArgs e)
	{
		m_Error = e.Message;
		if (ChangeStateEvent != null)
		{
			ChangeStateEvent(SocketActionState.ERROR);
		}
	}

	private void OnOpen(object sender, EventArgs e)
	{
		m_IsConnected = true;
		if (ChangeStateEvent != null)
		{
			ChangeStateEvent(SocketActionState.OPEN);
		}
	}

	private void OnMessage(object sender, WebSocketSharp.MessageEventArgs e)
	{
		m_Messages.Enqueue(e.RawData);
		if (ChangeStateEvent != null)
		{
			ChangeStateEvent(SocketActionState.MESSAGE);
		}
	}
}