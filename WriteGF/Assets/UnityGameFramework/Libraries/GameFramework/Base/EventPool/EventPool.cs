using System;
using System.Collections.Generic;

namespace GameFramework
{
    internal sealed partial class EventPool<T> where T : BaseEventArgs
    {
        private readonly GameFrameworkMultiDictionary<int, EventHandler<T>> m_EventHandlers;
        private readonly Queue<Event> m_Events;
        private readonly Dictionary<object, LinkedListNode<EventHandler<T>>> m_CachedNodes;
        private readonly Dictionary<object, LinkedListNode<EventHandler<T>>> m_TempNodes;
        private readonly EventPoolMode m_EventPoolMode;
        private EventHandler<T> m_DefaultHandler;

        public EventPool(EventPoolMode mode)
        {
            m_EventHandlers = new GameFrameworkMultiDictionary<int, EventHandler<T>>();
            m_Events = new Queue<Event>();
            m_CachedNodes = new Dictionary<object, LinkedListNode<EventHandler<T>>>();
            m_TempNodes = new Dictionary<object, LinkedListNode<EventHandler<T>>>();
            m_EventPoolMode = mode;
            m_DefaultHandler = null;
        }

        public int EventHandlerCount => m_EventHandlers.Count;

        public int EventCount => m_Events.Count;

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            while (m_Events.Count > 0)
            {
                Event eventNode = null;
                lock (m_Events)
                {
                    eventNode = m_Events.Dequeue();
                    HandleEvent(eventNode.Sender,eventNode.EventArgs);
                }
            }
        }

        private void HandleEvent(object sender, T e)
        {
            bool noHandlerException = false;
            GameFrameworkLinkedListRange<EventHandler<T>>
                range = default(GameFrameworkLinkedListRange<EventHandler<T>>);
            if (m_EventHandlers.TryGetValue(e.Id, out range))
            {
                LinkedListNode<EventHandler<T>> current = range.First;
                while (current != null && current != range.Terminal)
                {
                    m_CachedNodes[e] = current.Next != range.Terminal ? current.Next : null;
                    current.Value(sender, e);
                    current = m_CachedNodes[e];
                }

                m_CachedNodes.Remove(e);
            }
            else if (m_DefaultHandler != null)
            {
                m_DefaultHandler(sender, e);
            }
            else if ((m_EventPoolMode & EventPoolMode.AllowNoHandler) == 0)
            {
                noHandlerException = true;
            }

            ReferencePool.Release(e);

            if (noHandlerException)
            {
                throw new GameFrameworkException(Utility.Text.Format("Event '{0}' not allow no handler.",
                    e.Id.ToString()));
            }
        }
    }
}