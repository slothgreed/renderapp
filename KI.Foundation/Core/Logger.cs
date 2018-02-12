using System;
using System.Runtime.CompilerServices;
using OpenTK.Graphics.OpenGL;

namespace KI.Foundation.Core
{
    /// <summary>
    /// ログクラス
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// 標準エラー
        /// </summary>
        private static LogLevel outputLogLevel = LogLevel.Error;

        /// <summary>
        /// GLエラー
        /// </summary>
        private static LogLevel glLogLevel = LogLevel.Error;

        /// <summary>
        /// ログレベル
        /// </summary>
        public enum LogLevel
        {
            None,
            Debug,
            Warning,
            Error,
            Allway
        }

        /// <summary>
        /// ログ
        /// </summary>
        /// <param name="level">エラーレベル</param>
        /// <param name="message">メッセージ</param>
        /// <param name="methodName">関数名</param>
        public static void Log(LogLevel level, string message, [CallerMemberName]string methodName = "")
        {
            if (level == LogLevel.Allway)
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

        /// <summary>
        /// GLログ
        /// </summary>
        /// <param name="level">エラーレベル</param>
        /// <param name="message">メッセージ</param>
        /// <param name="methodName">関数名</param>
        public static void GLLog(LogLevel level,string message = null, [CallerMemberName]string methodName = "")
        {
            if (glLogLevel == LogLevel.None)
            {
                return;
            }

            if (level >= glLogLevel)
            {
                ErrorCode error = GL.GetError();
                if (message != null)
                {
                    Console.WriteLine(level + "GL:" + methodName + ":" + message);
                }

                if (error != ErrorCode.NoError)
                {
                    Console.WriteLine(level + "GL: " + methodName + ":" + error);
                }
            }
        }
    }
}
