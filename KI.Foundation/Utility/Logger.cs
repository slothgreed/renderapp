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
        }
        public static LogLevel outputLogLevel = LogLevel.Error;
        public static LogLevel glLogLevel = LogLevel.Error;
        public static void Log(LogLevel level, string error, [CallerMemberName]string methodName = "")
        {
            if(outputLogLevel == LogLevel.None)
            {
                return;
            }
            if(level >= outputLogLevel)
            {
                Console.WriteLine(level +" : "+ error);
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
                    Console.WriteLine(level + "GL: " +  methodName + ":" + error);
                }
            }
        }
    }
}
