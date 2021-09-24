namespace GameFramework
{
    public static partial class GameFrameworkLog
    {
        public interface ILogHelper
        {
            void Log(GameFrameworkLogLevel level, object message);
        }
    }
}