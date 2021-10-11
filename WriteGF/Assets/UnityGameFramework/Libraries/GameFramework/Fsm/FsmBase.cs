namespace GameFramework.Fsm
{
    public abstract class FsmBase
    {
        private string m_Name;

        public FsmBase()
        {
            m_Name = string.Empty;
        }

        public string Name
        {
            get { return m_Name; }
            protected set { m_Name = value ?? string.Empty; }
        }

        // public string FullName
        // {
        //     get
        //     {
        //         return new TYpe
        //     }
        // }
    }
}