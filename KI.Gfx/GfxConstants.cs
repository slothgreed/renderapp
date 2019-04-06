using System;

namespace KI.Gfx
{
    /// <summary>
    /// AssetDirectoryクラス
    /// </summary>
    public static class GfxConstants
    {
        /// <summary>
        /// KIProjectDirecoty
        /// </summary>
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
