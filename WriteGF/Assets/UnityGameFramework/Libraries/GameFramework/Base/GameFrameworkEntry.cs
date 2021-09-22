using System.Collections.Generic;

namespace GameFramework
{
    /// <summary>
    /// 游戏框架入口（静态类，入口的唯一性，和便于直接用类名进行访问）
    /// </summary>
    public static class GameFrameworkEntry
    {
        private static readonly GameFrameworkLinkedList<GameFrameworkModule> s_GameFrameworkModules =
            new GameFrameworkLinkedList<GameFrameworkModule>();

        public static void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var module in s_GameFrameworkModules)
            {
                module.Update(elapseSeconds, realElapseSeconds);
            }
        }

        public static void Shutdown()
        {
            for (LinkedListNode<GameFrameworkModule> current = s_GameFrameworkModules.Last;
                current != null;
                current = current.Previous)
            {
                current.Value.Shutdown();
            }
            
            s_GameFrameworkModules.Clear();
            
        }
    }
}