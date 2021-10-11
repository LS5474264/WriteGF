using System.Collections;
using System.Collections.Generic;

namespace GameFramework
{
    public class
        GameFrameworkMultiDictionary<TKey, TValue> : IEnumerable<
            KeyValuePair<TKey, GameFrameworkLinkedListRange<TValue>>>
    {
        private readonly GameFrameworkLinkedList<TValue> m_LinkedList;
        private readonly Dictionary<TKey, GameFrameworkLinkedListRange<TValue>> m_Dictionary;

        public GameFrameworkMultiDictionary()
        {
            m_LinkedList = new GameFrameworkLinkedList<TValue>();
            m_Dictionary = new Dictionary<TKey, GameFrameworkLinkedListRange<TValue>>();
        }

        public int Count => m_Dictionary.Count;

        public GameFrameworkLinkedListRange<TValue> this[TKey key]
        {
            get
            {
                GameFrameworkLinkedListRange<TValue> range = default(GameFrameworkLinkedListRange<TValue>);
                m_Dictionary.TryGetValue(key, out range);
                return range;
            }
        }

        public void Clear()
        {
            m_Dictionary.Clear();
            m_LinkedList.Clear();
        }

        public bool Contains(TKey key)
        {
            return m_Dictionary.ContainsKey(key);
        }

        public bool Contains(TKey key, TValue value)
        {
            GameFrameworkLinkedListRange<TValue> range = default(GameFrameworkLinkedListRange<TValue>);
            if (m_Dictionary.TryGetValue(key, out range))
            {
                return range.Contains(value);
            }

            return false;
        }

        public bool TryGetValue(TKey key, out GameFrameworkLinkedListRange<TValue> range)
        {
            return m_Dictionary.TryGetValue(key, out range);
        }

        public void Add(TKey key, TValue value)
        {
            GameFrameworkLinkedListRange<TValue> range = default(GameFrameworkLinkedListRange<TValue>);
            if (m_Dictionary.TryGetValue(key, out range))
            {
                m_LinkedList.AddBefore(range.Terminal, value);
            }
            else
            {
                LinkedListNode<TValue> first = m_LinkedList.AddLast(value);
                LinkedListNode<TValue> terminal = m_LinkedList.AddLast(default(TValue));
                m_Dictionary.Add(key, new GameFrameworkLinkedListRange<TValue>(first, terminal));
            }
        }

        public bool Remove(TKey key, TValue value)
        {
            GameFrameworkLinkedListRange<TValue> range = default(GameFrameworkLinkedListRange<TValue>);
            if (m_Dictionary.TryGetValue(key, out range))
            {
                for (LinkedListNode<TValue> current = range.First;
                    current != null && current != range.Terminal;
                    current = current.Next)
                {
                    if (current.Value.Equals(value))
                    {
                        if (current == range.First)
                        {
                            LinkedListNode<TValue> next = current.Next;
                            if (next == range.Terminal)
                            {
                                m_LinkedList.Remove(next);
                                m_Dictionary.Remove(key);
                            }
                            else
                            {
                                m_Dictionary[key] = new GameFrameworkLinkedListRange<TValue>(next, range.Terminal);
                            }
                        }

                        m_LinkedList.Remove(current);
                        return true;
                    }
                }
            }

            return false;
        }

        public bool RemoveAll(TKey key)
        {
            GameFrameworkLinkedListRange<TValue> range = default(GameFrameworkLinkedListRange<TValue>);
            if (m_Dictionary.TryGetValue(key, out range))
            {
                m_Dictionary.Remove(key);

                LinkedListNode<TValue> current = range.First;
                while (current != null)
                {
                    LinkedListNode<TValue> next = current != range.Terminal ? current.Next : null;
                    m_LinkedList.Remove(current);
                    current = next;
                }

                return true;
            }

            return false;
        }


        public Enumerator GetEnumerator()
        {
            return new Enumerator(m_Dictionary);
        }

        IEnumerator<KeyValuePair<TKey, GameFrameworkLinkedListRange<TValue>>>
            IEnumerable<KeyValuePair<TKey, GameFrameworkLinkedListRange<TValue>>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        System.Collections.IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public struct Enumerator : IEnumerator<KeyValuePair<TKey, GameFrameworkLinkedListRange<TValue>>>
        {
            private Dictionary<TKey, GameFrameworkLinkedListRange<TValue>>.Enumerator m_Enumerator;

            internal Enumerator(Dictionary<TKey, GameFrameworkLinkedListRange<TValue>> dictionary)
            {
                if (dictionary == null)
                {
                    throw new GameFrameworkException("Dictionary is invalid.");
                }

                m_Enumerator = dictionary.GetEnumerator();
            }

            public KeyValuePair<TKey, GameFrameworkLinkedListRange<TValue>> Current
            {
                get { return m_Enumerator.Current; }
            }

            object System.Collections.IEnumerator.Current
            {
                get { return m_Enumerator.Current; }
            }

            public void Dispose()
            {
                m_Enumerator.Dispose();
            }

            public bool MoveNext()
            {
                return m_Enumerator.MoveNext();
            }

            void System.Collections.IEnumerator.Reset()
            {
                ((IEnumerator<KeyValuePair<TKey, GameFrameworkLinkedListRange<TValue>>>) m_Enumerator).Reset();
            }
        }
    }
}