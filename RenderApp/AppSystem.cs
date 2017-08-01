using System;

namespace RenderApp
{
    /// <summary>
    /// アプリ
    /// </summary>
    public class AppSystem
    {
        /// <summary>
        /// KIProjectDirectory
        /// </summary>
        private static string kiDirectory = null;

        /// <summary>
        /// KIProjectDirectory
        /// </summary>
        public static string KIDirectory
        {
            get
            {
                if (kiDirectory == null)
                {
                    kiDirectory = Environment.GetEnvironmentVariable("KIProject");
                }

                return kiDirectory;
            }
        }
    }
}
