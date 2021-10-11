using System;
using UnityEngine.EventSystems;

namespace GameFramework.UI
{
    public interface IUIManager
    {
        int UIGroupCount { get; }

        float InstanceAutoReleaseInterval
        {
            get;
            set;
        }

        int InstanceCapacity
        {
            get;
            set;
        }

        float InstanceExpireTime
        {
            get;
            set;
        }

        int InstancePriority
        {
            get;
            set;
        }
    }
}