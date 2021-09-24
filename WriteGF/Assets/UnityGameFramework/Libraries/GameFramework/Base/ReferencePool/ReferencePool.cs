using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;

namespace GameFramework
{
    public static partial class ReferencePool
    {
        private static readonly Dictionary<Type, ReferenceCollection> s_RefereneceCollections =
            new Dictionary<Type, ReferenceCollection>();

        private static bool m_EnableStrictCheck = false;


        /// <summary>
        /// 获取或设置是否开启强制检查(不允许一个引用池中，存在两个相同的引用)。
        /// </summary>
        public static bool EnableStrictCheck
        {
            get { return m_EnableStrictCheck; }
            set { m_EnableStrictCheck = value; }
        }

        public static int Count => s_RefereneceCollections.Count;

        public static ReferencePoolInfo[] GetAllReferencePoolInfos()
        {
            int index = 0;
            ReferencePoolInfo[] results = null;
            lock (s_RefereneceCollections)
            {
                results = new ReferencePoolInfo[s_RefereneceCollections.Count];
                foreach (var referenceCollection in s_RefereneceCollections)
                {
                    results[index++] = new ReferencePoolInfo(referenceCollection.Key,
                        referenceCollection.Value.UnusedReferenceCount, referenceCollection.Value.UsingReferenceCount,
                        referenceCollection.Value.AcquireReferenceCount,
                        referenceCollection.Value.ReleaseReferenceCount, referenceCollection.Value.AddReferenceCount,
                        referenceCollection.Value.RemoveReferenceCount);
                }
            }

            return results;
        }

        public static void ClearAll()
        {
            lock (s_RefereneceCollections)
            {
                foreach (var referenceCollection in s_RefereneceCollections)
                {
                    referenceCollection.Value.RemoveAll();
                }

                s_RefereneceCollections.Clear();
            }
        }

        public static T Acquire<T>() where T : class, IReference, new()
        {
            return GetReferenceCollection(typeof(T)).Acquire<T>();
        }

        public static IReference Acquire(Type referenceType)
        {
            InternalCheckReferenceType(referenceType);
            return GetReferenceCollection(referenceType).Acquire();
        }

        public static void Release(IReference reference)
        {
            if (reference == null)
            {
                throw new GameFrameworkException("Reference is invalid.");
            }

            var referenceType = reference.GetType();
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).Release(reference);
        }

        public static void Add<T>(int count) where T : class, IReference, new()
        {
            GetReferenceCollection(typeof(T)).Add<T>(count);
        }

        public static void Add(Type referenceType, int count)
        {
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).Add(count);
        }

        public static void Remove<T>(int count) where T : class, IReference
        {
            GetReferenceCollection(typeof(T)).Remove(count);
        }

        public static void Remove(Type referenceType, int count)
        {
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).Remove(count);
        }

        /// <summary>
        /// 从引用池中移除所有的引用。
        /// </summary>
        /// <typeparam name="T">引用类型。</typeparam>
        public static void RemoveAll<T>() where T : class, IReference
        {
            GetReferenceCollection(typeof(T)).RemoveAll();
        }

        /// <summary>
        /// 从引用池中移除所有的引用。
        /// </summary>
        /// <param name="referenceType">引用类型。</param>
        public static void RemoveAll(Type referenceType)
        {
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).RemoveAll();
        }


        //强制检查
        private static void InternalCheckReferenceType(Type referenceType)
        {
            if (!m_EnableStrictCheck)
            {
                return;
            }

            if (referenceType == null)
            {
                throw new GameFrameworkException("Reference type is invalid.");
            }

            if (!referenceType.IsClass || referenceType.IsAbstract)
            {
                throw new GameFrameworkException("Reference type is not a non-abstract class type.");
            }

            if (!typeof(IReference).IsAssignableFrom(referenceType))
            {
                throw new GameFrameworkException(Utility.Text.Format("Reference type '{0}' is invalid.)",
                    referenceType.FullName));
            }
        }

        private static ReferenceCollection GetReferenceCollection(Type referenceType)
        {
            if (referenceType == null)
            {
                throw new GameFrameworkException("ReferenceType is invalid.");
            }

            ReferenceCollection referenceCollection = null;

            lock (s_RefereneceCollections)
            {
                if (!s_RefereneceCollections.TryGetValue(referenceType, out referenceCollection))
                {
                    referenceCollection = new ReferenceCollection(referenceType);
                    s_RefereneceCollections.Add(referenceType, referenceCollection);
                }
            }

            return referenceCollection;
        }
    }
}