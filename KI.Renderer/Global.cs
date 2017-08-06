using System;

namespace KI.Renderer
{
    /// <summary>
    /// グローバルクラス
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// KIProjectDirecoty
        /// </summary>
        private static string kiDirectory = null;

        /// <summary>
        /// レンダリングシステム
        /// </summary>
        public static RenderSystem RenderSystem { get; set; }

        /// <summary>
        /// シーン
        /// </summary>
        public static Scene Scene { get; set; }

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

        /// <summary>
        /// シェーダディレクトリ
        /// </summary>
        public static string ShaderDirectory
        {
            get
            {
                return KIDirectory + @"\renderapp\Resource\Shader";
            }
        }
    }
}
