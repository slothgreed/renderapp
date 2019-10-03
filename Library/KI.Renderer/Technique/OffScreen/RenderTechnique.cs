using System;
using System.Runtime.CompilerServices;
using KI.Asset;
using KI.Foundation.Core;
using KI.Gfx.KIShader;
using KI.Gfx.Render;
using KI.Asset.Primitive;
using OpenTK.Graphics.OpenGL;

namespace KI.Renderer.Technique
{
    /// <summary>
    /// レンダーテクニック
    /// </summary>
    public abstract class RenderTechnique : KIObject
    {
        /// <summary>
        /// 描画タイプ
        /// </summary>
        private RenderType renderType;

        /// <summary>
        /// レンダラ
        /// </summary>
        public RenderSystem RenderSystem
        {
            get;
            private set;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="renderer">レンダラ</param>
        /// <param name="tech">レンダーテクニックの種類</param>
        /// <param name="type">レンダリングタイプ</param>
        public RenderTechnique(string name, RenderSystem renderer, RenderType type)
            : base(name)
        {
            renderType = type;
            RenderSystem = renderer;
            Init();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="renderer">レンダラ</param>
        /// <param name="vertexShader">頂点シェーダ</param>
        /// <param name="fragShader">フラグシェーダ</param>
        /// <param name="type">レンダリングタイプ</param>
        public RenderTechnique(string name, RenderSystem renderer, string vertexShader, string fragShader, RenderType type)
            : base(name)
        {
            renderType = type;
            RenderSystem = renderer;
            Init(vertexShader, fragShader);
        }

        /// <summary>
        /// レンダリングタイプ
        /// </summary>
        public enum RenderType
        {
            Forward,
            OffScreen
        }

        /// <summary>
        /// レンダリングターゲット
        /// </summary>
        public RenderTarget RenderTarget { get; set; }

        /// <summary>
        /// オフスクリーン用平面
        /// </summary>
        protected PolygonNode Rectangle { get; set; }

        /// <summary>
        /// バッファのクリア
        /// </summary>
        public void ClearBuffer()
        {
            RenderTarget.ClearBuffer();
        }

        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public virtual void SizeChanged(int width, int height)
        {
            RenderTarget.SizeChanged(width, height);
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <param name="renderInfo">レンダリング情報</param>
        public virtual void Render(Scene scene, RenderInfo renderInfo)
        {
            if (renderType == RenderType.Forward)
            {
                Logger.Log(Logger.LogLevel.Error, "RenderTechnique : Not Defined Forward Render");
                throw new NotImplementedException();
            }

            if (renderType == RenderType.OffScreen)
            {
                RenderTarget.ClearBuffer();
                RenderTarget.BindRenderTarget();
                Rectangle.Render(scene, renderInfo);
                RenderTarget.UnBindRenderTarget();
            }
        }

        #region [initalize event]

        /// <summary>
        /// 初期化
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// レンダーターゲットの作成
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        protected virtual void CreateRenderTarget(int width, int height)
        {
            var texture = new RenderTexture[] { TextureFactory.Instance.CreateRenderTexture("Texture:" + Name, width, height, PixelFormat.Rgba) };
            RenderTarget = RenderTargetFactory.Instance.CreateRenderTarget("RenderTarget:" + Name, width, height, 1);
            RenderTarget.SetRenderTexture(texture);
        }

        /// <summary>
        /// シェーダへ値のセット
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="member">変数</param>
        /// <param name="value">値</param>
        /// <param name="memberName">シェーダ変数名</param>
        protected void SetValue<T>(ref T member, T value, [CallerMemberName]string memberName = "")
        {
            if (Rectangle.Polygon.Material.Shader.SetValue(memberName, value))
            {
                member = value;
            }
            else
            {
                Logger.Log(Logger.LogLevel.Error, "Set Shader Error " + memberName);
            }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="vertexShader">頂点シェーダ</param>
        /// <param name="fragShader">フラグシェーダ</param>
        private void Init(string vertexShader = null, string fragShader = null)
        {
            var polygon = PolygonUtility.CreatePolygon(Name, new Rectangle());
            Rectangle = new PolygonNode(polygon);
            // gbuffer用 以外はシェーダ作成
            if (vertexShader != null && fragShader != null)
            {
                Rectangle.Polygon.Material.Shader = ShaderFactory.Instance.CreateShaderVF(vertexShader, fragShader);
            }

            CreateRenderTarget(KI.Gfx.GLUtil.DeviceContext.Instance.Width, KI.Gfx.GLUtil.DeviceContext.Instance.Height);
            Initialize();
        }
        #endregion
    }
}
