using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagingSystem : SingletonComponent<MessagingSystem>
{
	public static MessagingSystem Instance {
		get { return (MessagingSystem)_Instance; }
		set { _Instance = value; }
	}

	private Dictionary<string, List<MessageHandlerDelegate>> _listenerDic = new Dictionary<string, List<MessageHandlerDelegate>>();

	private Queue<Message> _messagesQueue = new Queue<Message>();

	private const int _maxQueueProcessingTime = 16667;
	private System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

	public bool AttachListener(System.Type type, MessageHandlerDelegate handler)
	{
		if (type == null)
		{
			return false;
		}

		string msgType = type.Name;

		if (!_listenerDic.ContainsKey(msgType))
		{
			_listenerDic.Add(msgType, new List<MessageHandlerDelegate>());
		}

		List<MessageHandlerDelegate> listenerList = _listenerDic[msgType];
		if(listenerList.Contains(handler))
		{
			return false;
		}

		listenerList.Add(handler);
		return true;
	}

	public bool DetachListener(System.Type type, MessageHandlerDelegate handler)
	{
		if (type == null)
		{
			return false;
		}

		string msgType = type.Name;

		if (!_listenerDic.ContainsKey(msgType))
		{
			return false;
		}

		List<MessageHandlerDelegate> listenerList = _listenerDic[msgType];
		if (!listenerList.Contains(handler))
		{
			return false;
		}

		listenerList.Remove(handler);
		return true;
	}

	public bool QueueMessage(Message msg)
	{
		if (!_listenerDic.ContainsKey(msg.type))
		{
			return false;
		}

		_messagesQueue.Enqueue(msg);
		return true;
	}

	private void Update()
	{
		timer.Start();
		while (_messagesQueue.Count > 0)
		{
			if (_maxQueueProcessingTime > 0.0f)
			{
				if (timer.Elapsed.Milliseconds > _maxQueueProcessingTime)
				{
					timer.Stop();
					return;
				}
			}

			Message msg = _messagesQueue.Dequeue();
			if (!TriggerMessage(msg))
			{
				Debug.Log("Error");
			}
		}
	}

	private bool TriggerMessage(Message msg)
	{
		string msgType = msg.type;
		if (!_listenerDic.ContainsKey(msgType))
		{
			return false;
		}

		List<MessageHandlerDelegate> listenerList = _listenerDic[msgType];

		for (int i = 0; i < listenerList.Count; ++i)
		{
			if (listenerList[i](msg))
			{
				return true;
			}
		}
		return true;
	}
}
