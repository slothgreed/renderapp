using KI.Foundation.Core;
using KI.Gfx.Buffer;
using KI.Gfx.KITexture;
using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// 光源
    /// </summary>
    public abstract class Light : KIObject
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="lightPos">位置</param>
        /// <param name="lightDir">方向</param>
        protected Light(string name, Vector3 lightPos, Vector3 lightDir)
            : base(name)
        {
            Position = lightPos;
            Matrix = Matrix4.LookAt(Position, lightDir, Vector3.UnitY);
            Shiness = 1;
            if (name != null)
            {
                Name = name;
            }
            else
            {
                Name = "Light";
            }
        }

        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// 方向
        /// </summary>
        public Vector3 Direction { get; set; }

        /// <summary>
        /// 強度
        /// </summary>
        public float Shiness { get; set; }

        /// <summary>
        /// 環境光
        /// </summary>
        public Vector3 AmbientColor { get; set; }

        /// <summary>
        /// 拡散光
        /// </summary>
        public Vector3 DiffuseColor { get; set; }

        /// <summary>
        /// 反射光
        /// </summary>
        public Vector3 SpecularColor { get; set; }

        /// <summary>
        /// マトリクス
        /// </summary>
        public Matrix4 Matrix { get; private set; }

        /// <summary>
        /// シャドウマップ
        /// </summary>
        public TextureBuffer ShadowMap { get; private set; }
    }
}
