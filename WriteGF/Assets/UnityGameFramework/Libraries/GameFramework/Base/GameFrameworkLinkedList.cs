using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace GameFramework
{
    /// <summary>
    /// 游戏框架链表类。sealed链表存储结构已经是最后的不需要进行继承。
    /// 为什么要重新写个链表，缓存了结点，降低GC
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class GameFrameworkLinkedList<T> : ICollection<T>, ICollection
    {
        private readonly LinkedList<T> m_LinkedList;
        private readonly Queue<LinkedListNode<T>> m_CachedNodes;

        public GameFrameworkLinkedList()
        {
            m_LinkedList = new LinkedList<T>();
            m_CachedNodes = new Queue<LinkedListNode<T>>();
        }


        public int Count
        {
            get { return m_LinkedList.Count; }
        }


        public int CacheNodeCount
        {
            get { return m_CachedNodes.Count; }
        }

        public LinkedListNode<T> First
        {
            get { return m_LinkedList.First; }
        }

        public LinkedListNode<T> Last
        {
            get { return m_LinkedList.Last; }
        }

        public bool IsReadOnly
        {
            get { return ((ICollection<T>) m_LinkedList).IsReadOnly; }
        }

        public object SyncRoot
        {
            get { return ((ICollection<T>) m_LinkedList).IsReadOnly; }
        }

        public bool IsSynchronized
        {
            get { return ((ICollection) m_LinkedList).IsSynchronized; }
        }


        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_LinkedList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
        {
            LinkedListNode<T> newNode = AcquireNode(value);
            m_LinkedList.AddAfter(node, newNode);
            return newNode;
        }

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            m_LinkedList.AddAfter(node, newNode);
        }

        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
        {
            LinkedListNode<T> newNode = AcquireNode(value);
            m_LinkedList.AddBefore(node, newNode);
            return newNode;
        }

        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            m_LinkedList.AddBefore(node, newNode);
        }

        public LinkedListNode<T> AddFirst(T value)
        {
            LinkedListNode<T> node = AcquireNode(value);
            m_LinkedList.AddFirst(node);
            return node;
        }

        public void AddFirst(LinkedListNode<T> node)
        {
            m_LinkedList.AddFirst(node);
        }

        public LinkedListNode<T> AddLast(T value)
        {
            LinkedListNode<T> node = AcquireNode(value);
            m_LinkedList.AddLast(node);
            return node;
        }

        public void AddLast(LinkedListNode<T> node)
        {
            m_LinkedList.AddLast(node);
        }

        public void Clear()
        {
            LinkedListNode<T> current = m_LinkedList.First;
            while (current != null)
            {
                ReleaseNode(current);
                current = current.Next;
            }

            m_LinkedList.Clear();
        }

        public void ClearCacheNodes()
        {
            m_CachedNodes.Clear();
        }

        public bool Contains(T value)
        {
            return m_LinkedList.Contains(value);
        }

        public void CopyTo(T[] array, int index)
        {
            m_LinkedList.CopyTo(array, index);
        }

        public bool Remove(T value)
        {
            LinkedListNode<T> node = m_LinkedList.Find(value);
            if (node != null)
            {
                m_LinkedList.Remove(node);
                ReleaseNode(node);
                return true;
            }

            return false;
        }

        private LinkedListNode<T> AcquireNode(T value)
        {
            LinkedListNode<T> node = null;
            if (m_CachedNodes.Count > 0)
            {
                node = m_CachedNodes.Dequeue();
                node.Value = value;
            }
            else
            {
                node = new LinkedListNode<T>(value);
            }

            return node;
        }

        private void ReleaseNode(LinkedListNode<T> node)
        {
            node.Value = default(T);
            m_CachedNodes.Enqueue(node);
        }


        public void Add(T item)
        {
            AddLast(item);
        }

        public struct Enumerator : IEnumerator<T>
        {
            private LinkedList<T>.Enumerator m_Enumerator;

            internal Enumerator(LinkedList<T> linkedList)
            {
                if (linkedList != null)
                {
                    throw new GameFrameworkException("Linked list is invalid");
                }

                m_Enumerator = linkedList.GetEnumerator();
            }

            public bool MoveNext()
            {
                return m_Enumerator.MoveNext();
            }

            void IEnumerator.Reset()
            {
                ((IEnumerator) m_Enumerator).Reset();
            }

            public T Current
            {
                get { return m_Enumerator.Current; }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                m_Enumerator.Dispose();
            }
        }
    }
}