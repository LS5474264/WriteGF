using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;

namespace GameFramework
{
    public struct GameFrameworkLinkedListRange<T> : IEnumerable<T>
    {
        private readonly LinkedListNode<T> m_First;
        private readonly LinkedListNode<T> m_Terminal;

        public GameFrameworkLinkedListRange(LinkedListNode<T> first, LinkedListNode<T> termianl)
        {
            if (first == null || termianl == null || first == termianl)
            {
                throw new GameFrameworkException("Range is invalid.");
            }

            m_First = first;
            m_Terminal = termianl;
        }

        public bool IsValid
        {
            get { return m_First != null && m_Terminal != null && m_First != m_Terminal; }
        }

        public LinkedListNode<T> First
        {
            get { return m_First; }
        }

        public LinkedListNode<T> Terminal
        {
            get { return m_Terminal; }
        }

        public int Count
        {
            get
            {
                if (!IsValid)
                {
                    return 0;
                }

                int count = 0;

                for (LinkedListNode<T> current = m_First;
                    current != null && current != m_Terminal;
                    current = current.Next)
                {
                    count++;
                }

                return count;
            }
        }

        public bool Contains(T value)
        {
            for (LinkedListNode<T> current = m_First; current != null && current != m_Terminal; current = current.Next)
            {
                if (current.Value.Equals(value))
                {
                    return true;
                }
            }

            return false;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }


        public struct Enumerator : IEnumerator<T>
        {
            private readonly GameFrameworkLinkedListRange<T> m_GameFrameworkLinkedListRange;
            private LinkedListNode<T> m_Current;

            private T m_CurrentValue;

            internal Enumerator(GameFrameworkLinkedListRange<T> range)
            {
                if (!range.IsValid)
                {
                    throw new GameFrameworkException("Range is invalid.");
                }

                m_GameFrameworkLinkedListRange = range;
                m_Current = m_GameFrameworkLinkedListRange.m_First;
                m_CurrentValue = default(T);
            }

            public T Current => m_CurrentValue;

            object IEnumerator.Current => m_CurrentValue;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (m_Current == null || m_Current == m_GameFrameworkLinkedListRange.m_Terminal)
                {
                    return false;
                }

                m_CurrentValue = m_Current.Value;
                m_Current = m_Current.Next;
                return true;
            }

            void IEnumerator.Reset()
            {
                m_Current = m_GameFrameworkLinkedListRange.m_First;
                m_CurrentValue = default(T);
            }
        }
    }
}