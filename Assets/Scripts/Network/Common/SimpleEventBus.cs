using System.Collections.Generic;

public interface ISubscriber
{
    void ReceiveEvent(EventType name, object payload);
}


public enum EventType : byte
{
    WebSocketDataConnceted,
    WebSocketDataDisconnceted,
    WebSocketDataReceived
}


public enum TopicType
{
    ServiceToServer
}


public static class SimpleEventBus
{
    private static readonly Dictionary<TopicType, Dictionary<EventType, List<ISubscriber>>> _eventChannels =
        new Dictionary<TopicType, Dictionary<EventType, List<ISubscriber>>>();
       
    public static void SendEvent(TopicType topic, EventType eventType, object payload)
    {
        if(_eventChannels.ContainsKey(topic))
        {
            if (_eventChannels[topic].ContainsKey(eventType))
            {
                for (int j = 0; j < _eventChannels[topic][eventType].Count; j++)
                {
                    _eventChannels[topic][eventType][j].ReceiveEvent(eventType, payload);
                }
            }
        }
    }    

    public static void SubscribeOnEvent(TopicType topic, EventType eventType, ISubscriber subscriber)
    {
        if(_eventChannels.ContainsKey(topic))
        {
            if (!_eventChannels[topic].ContainsKey(eventType))
            {
                _eventChannels[topic].Add(eventType, new List<ISubscriber>{subscriber});
            }
            else
            {
                _eventChannels[topic][eventType].Add(subscriber);
            }
        }
        else
        {
            _eventChannels.Add(topic, new Dictionary<EventType, List<ISubscriber>> { { eventType, new List<ISubscriber>{subscriber} } } );
        }
    }

    public static void UnsubscribeOnEvent(TopicType topic, EventType eventType, ISubscriber subscriber)
    {
        if (_eventChannels.ContainsKey(topic))
        {
            for (int i = 0; i < _eventChannels[topic][eventType].Count; i++)
            {
                if (_eventChannels[topic].ContainsKey(eventType))
                {
                    _eventChannels[topic][eventType].Remove(subscriber);
                }
            }
            
            if(_eventChannels[topic].Count == 0)
            {
                _eventChannels.Remove(topic);
            }
        }        
    }    
}
