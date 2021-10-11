namespace GameFramework.UI
{
    internal sealed partial class UIManager:GameFrameworkModule,IUIManager
    {
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            throw new System.NotImplementedException();
        }

        internal override void Shutdown()
        {
            throw new System.NotImplementedException();
        }

        public int UIGroupCount { get; }
        public float InstanceAutoReleaseInterval { get; set; }
        public int InstanceCapacity { get; set; }
        public float InstanceExpireTime { get; set; }
        public int InstancePriority { get; set; }
    }
}