using KI.Foundation.Core;
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
        public Vector3 Position { get; protected set; }

        /// <summary>
        /// 方向
        /// </summary>
        public Vector3 Direction { get; protected set; }

        /// <summary>
        /// 強度
        /// </summary>
        public float Shiness { get; protected set; }

        /// <summary>
        /// 色
        /// </summary>
        public Vector3 Color { get; protected set; }

        /// <summary>
        /// マトリクス
        /// </summary>
        public Matrix4 Matrix { get; private set; }

        /// <summary>
        /// シャドウマップ
        /// </summary>
        public Texture ShadowMap { get; private set; }

        /// <summary>
        /// 解放処理
        /// </summary>
        public override void Dispose()
        {
        }
    }
}
