using System;
using System.Collections.Generic;

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
    }
}