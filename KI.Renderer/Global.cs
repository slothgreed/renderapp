using System;
using KI.Asset;

namespace KI.Renderer
{
    public static class Global
    {
        public static RenderSystem RenderSystem;
        public static Scene Scene;
        private static string kiDirectory = null;

        /// <summary>
        /// KIProjectDirecoty
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

        public static string ShaderDirectory
        {
            get
            {
                return KIDirectory + @"\renderapp\Resource\Shader";
            }
        }
    }
}
