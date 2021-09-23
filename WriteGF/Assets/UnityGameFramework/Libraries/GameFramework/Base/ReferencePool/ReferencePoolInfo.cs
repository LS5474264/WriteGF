using System;
using System.Runtime.InteropServices;


namespace GameFramework
{
    [StructLayout(LayoutKind.Auto)]
    public struct ReferencePoolInfo
    {
        public Type MType => m_Type;

        public int MUnusedReferenceCount => m_UnusedReferenceCount;

        public int MUsingReferenceCount => m_UsingReferenceCount;

        public int MAcquireReferenceCount => m_AcquireReferenceCount;

        public int MReleaseReferenceCount => m_ReleaseReferenceCount;

        public int MAddReferenceCount => m_AddReferenceCount;

        public int MRemoveReferenceCount => m_RemoveReferenceCount;

        public ReferencePoolInfo(Type mType, int mUnusedReferenceCount, int mUsingReferenceCount, int mAcquireReferenceCount, int mReleaseReferenceCount, int mAddReferenceCount, int mRemoveReferenceCount)
        {
            m_Type = mType;
            m_UnusedReferenceCount = mUnusedReferenceCount;
            m_UsingReferenceCount = mUsingReferenceCount;
            m_AcquireReferenceCount = mAcquireReferenceCount;
            m_ReleaseReferenceCount = mReleaseReferenceCount;
            m_AddReferenceCount = mAddReferenceCount;
            m_RemoveReferenceCount = mRemoveReferenceCount;
        }

        private readonly Type m_Type;
        private readonly int m_UnusedReferenceCount;
        private readonly int m_UsingReferenceCount;
        private readonly int m_AcquireReferenceCount;
        private readonly int m_ReleaseReferenceCount;
        private readonly int m_AddReferenceCount;
        private readonly int m_RemoveReferenceCount;
        
        
    }
}