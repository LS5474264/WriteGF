using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public static partial class ReferencePool
    {
        private sealed class ReferenceCollection
        {
            public Type ReferenceType => m_ReferenceType;

            public int UsingReferenceCount => m_UsingReferenceCount;

            public int AcquireReferenceCount => m_AcquireReferenceCount;

            public int ReleaseReferenceCount => m_ReleaseReferenceCount;

            public int AddReferenceCount => m_AddReferenceCount;

            public int RemoveReferenceCount => m_RemoveReferenceCount;

            private readonly Queue<IReference> m_References;
            private readonly Type m_ReferenceType;
            private int m_UsingReferenceCount;
            private int m_AcquireReferenceCount;
            private int m_ReleaseReferenceCount;
            private int m_AddReferenceCount;
            private int m_RemoveReferenceCount;


            public ReferenceCollection(Type referenceType)
            {
                m_References = new Queue<IReference>();
                m_ReferenceType = referenceType;
                m_UsingReferenceCount = 0;
                m_AcquireReferenceCount = 0;
                m_ReleaseReferenceCount = 0;
                m_AddReferenceCount = 0;
                m_RemoveReferenceCount = 0;
            }


            //T必须是类不能是结构体，必须继承IReference接口，必须要有无参构造类型
            public T Acquire<T>() where T : class, IReference, new()
            {
                if (typeof(T) != m_ReferenceType)
                {
                    throw new GameFrameworkException("Type is invalid");
                }

                m_UsingReferenceCount++;
                m_AcquireReferenceCount++;
                lock (m_References)
                {
                    if (m_References.Count > 0)
                    {
                        return (T) m_References.Dequeue();
                    }
                }

                //新添加了一个引用
                m_AddReferenceCount++;
                return new T();
            }
        }
    }
}