using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UIElements;

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
            ReferencePool.ClearAll();
            Utility.Marshal.FreeCachedHGlobal();
            GameFrameworkLog.SetLogHelper(null);
        }

        public static T GetModule<T>() where T : class
        {
            Type interfaceType = typeof(T);
            if (!interfaceType.IsInterface)
            {
                throw new GameFrameworkException(
                    Utility.Text.Format("You must get module by interface,but '{0}' is not", interfaceType.FullName));
            }

            if (!interfaceType.FullName.StartsWith("GameFramework.", StringComparison.Ordinal))
            {
                throw new GameFrameworkException(
                    Utility.Text.Format(
                        "You must get a Game Framework module,but '{0} is not.", interfaceType.FullName));
            }

            string moduleName =
                Utility.Text.Format("{0}.{1}", interfaceType.Namespace, interfaceType.Name.Substring(1));
            Type moduleType = Type.GetType(moduleName);
            if (moduleType == null)
            {
                throw new GameFrameworkException(Utility.Text.Format("Can not find Game Framework module type '{0}'",
                    moduleName));
            }

            return GetModule(moduleType) as T;
        }

        private static GameFrameworkModule GetModule(Type moduleType)
        {
            foreach (var module in s_GameFrameworkModules)
            {
                if (module.GetType() == moduleType)
                {
                    return module;
                }
            }

            return CreateModule(moduleType);
        }

        private static GameFrameworkModule CreateModule(Type moduleType)
        {
            GameFrameworkModule module = (GameFrameworkModule) Activator.CreateInstance(moduleType);

            if (module == null)
            {
                throw new GameFrameworkException(
                    Utility.Text.Format("Can not create module '{0}'", moduleType.FullName));
            }

            LinkedListNode<GameFrameworkModule> current = s_GameFrameworkModules.First;

            while (current != null)
            {
                if (module.Priority > current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            if (current != null)
            {
                s_GameFrameworkModules.AddBefore(current, module);
            }
            else
            {
                s_GameFrameworkModules.AddLast(module);
            }

            return module;
        }
    }
}