using System;
using OpenTK.Graphics.OpenGL;
using System.Runtime.CompilerServices;

namespace KI.Foundation.Utility
{
    public class Logger
    {
        public enum LogLevel
        {
            None,
            Debug,
            Warning,
            Error,
            Descript
        }

        public static LogLevel outputLogLevel = LogLevel.Error;
        public static LogLevel glLogLevel = LogLevel.Error;
        public static void Log(LogLevel level, string message, [CallerMemberName]string methodName = "")
        {
            if (level == LogLevel.Descript)
            {
                Console.WriteLine(level + ": " + message);
                return;
            }
            if (outputLogLevel == LogLevel.None)
            {
                return;
            }
            if (level >= outputLogLevel)
            {
                Console.WriteLine(level + " : " + message);
            }
        }

        public static void GLLog(LogLevel level, [CallerMemberName]string methodName = "")
        {
            if (glLogLevel == LogLevel.None)
            {
                return;
            }

            if (level >= glLogLevel)
            {
                ErrorCode error = GL.GetError();
                if (error != ErrorCode.NoError)
                {
                    Console.WriteLine(level + "GL: " + methodName + ":" + error);
                }
            }
        }
    }
}
