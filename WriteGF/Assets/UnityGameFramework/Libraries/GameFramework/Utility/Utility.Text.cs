using System;
using System.Text;
using UnityEditor;

namespace GameFramework
{
    public static partial class Utility
    {
        public static class Text
        {
            private const int StringBuilderCapacity = 1024;

            [ThreadStatic] private static StringBuilder s_CachedStringBuilder = null;

            public static string Format(string format, object arg0)
            {
                if (format == null)
                {
                    throw new GameFrameworkException("Format is invalid.");
                }

                CheckCachedStringBuilder();
                s_CachedStringBuilder.Length = 0;
                s_CachedStringBuilder.AppendFormat(format, arg0);
                return s_CachedStringBuilder.ToString();
            }


            public static string Format(string format, object arg0, object arg1)
            {
                if (format == null)
                {
                    throw new GameFrameworkException("Format is invalid.");
                }

                CheckCachedStringBuilder();
                s_CachedStringBuilder.Length = 0;
                s_CachedStringBuilder.AppendFormat(format, arg0, arg1);
                return s_CachedStringBuilder.ToString();
            }

            public static string Format(string format, object arg0, object arg1, object arg2)
            {
                if (format == null)
                {
                    throw new GameFrameworkException("Format is invalid.");
                }

                CheckCachedStringBuilder();
                s_CachedStringBuilder.Length = 0;
                s_CachedStringBuilder.AppendFormat(format, arg0, arg1, arg2);
                return s_CachedStringBuilder.ToString();
            }

            public static string Format(string format, params object[] args)
            {
                if (format == null)
                {
                    throw new GameFrameworkException("Format is invalid.");
                }

                if (args == null)
                {
                    throw new GameFrameworkException("Args is invalid.");
                }

                CheckCachedStringBuilder();
                s_CachedStringBuilder.Length = 0;
                s_CachedStringBuilder.AppendFormat(format, args);
                return s_CachedStringBuilder.ToString();
            }

            private static void CheckCachedStringBuilder()
            {
                if (s_CachedStringBuilder == null)
                {
                    s_CachedStringBuilder = new StringBuilder(StringBuilderCapacity);
                }
            }
        }
    }
}