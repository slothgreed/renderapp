using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Asset
{
    /// <summary>
    /// AssetDirectoryクラス
    /// </summary>
    public static class AssetDirectory
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
