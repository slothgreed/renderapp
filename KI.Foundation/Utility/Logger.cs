using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Runtime.CompilerServices;
using System.Diagnostics;

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
        public static LogLevel outputLogLevel = LogLevel.Warning;
        public static LogLevel glLogLevel = LogLevel.Error;
        public static void Log(LogLevel level, string error, [CallerMemberName]string methodName = "")
        {
            if(level >= outputLogLevel)
            {
                Console.WriteLine(level +" : "+ error);
            }
        }

        public static void GLLog(LogLevel level, [CallerMemberName]string methodName = "")
        {
            if (level >= glLogLevel)
            {
                ErrorCode error = GL.GetError();
                if (error != ErrorCode.NoError)
                {
                    Console.WriteLine(level + " : " +  methodName + ":" + error);
                }
            }
        }
    }
}
