using System.Text;

namespace GameFramework
{
    internal sealed partial class EventPool<T> where T:BaseEventArgs
    {
        private sealed class Event:IReference
        {
            private object m_Sender;
            private T m_EventArgs;

            public Event()
            {
                m_Sender = null;
                m_EventArgs = null;
            }

            public object Sender => m_Sender;

            public T EventArgs => m_EventArgs;

            public static Event Create(object sender, T e)
            {
                Event eventNode = ReferencePool.Acquire<Event>();
                eventNode.m_Sender = sender;
                eventNode.m_EventArgs = e;
                return eventNode;
            }

            public void Clear()
            {
                m_Sender = null;
                m_EventArgs = null;
            }
        }
    }
}