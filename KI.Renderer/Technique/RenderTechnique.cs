using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KI.Asset;
using KI.Foundation.Core;
using KI.Foundation.Utility;
using KI.Gfx.KIShader;
using KI.Gfx.KITexture;
using KI.Gfx.Render;

namespace KI.Renderer
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
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="tech">レンダーテクニックの種類</param>
        /// <param name="type">レンダリングタイプ</param>
        public RenderTechnique(string name, RenderTechniqueType tech, RenderType type)
            : base(name)
        {
            renderType = type;
            Technique = tech;
            Init();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="vertexShader">頂点シェーダ</param>
        /// <param name="fragShader">フラグシェーダ</param>
        /// <param name="tech">レンダーテクニックの種類</param>
        /// <param name="type">レンダリングタイプ</param>
        public RenderTechnique(string name, string vertexShader, string fragShader, RenderTechniqueType tech, RenderType type)
            : base(name)
        {
            renderType = type;
            Technique = tech;
            Init(vertexShader, fragShader);
        }

        /// <summary>
        /// レンダリングタイプ
        /// </summary>
        public enum RenderType
        {
            Original,
            OffScreen
        }

        /// <summary>
        /// レンダーテクニックの種類
        /// </summary>
        public RenderTechniqueType Technique { get; private set; }

        /// <summary>
        /// シェーダ
        /// </summary>
        public Shader ShaderItem { get; set; }

        /// <summary>
        /// レンダリングターゲット
        /// </summary>
        public RenderTarget RenderTarget { get; set; }

        /// <summary>
        /// 出力テクスチャ
        /// </summary>
        public Texture[] OutputTexture { get; protected set; }

        /// <summary>
        /// ポストプロセス用平面
        /// </summary>
        protected RenderObject Plane { get; set; }

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
        public virtual void Render(Scene scene)
        {
            if (renderType == RenderType.Original)
            {
                Logger.Log(Logger.LogLevel.Error, "RenderTechnique : Not Defined Original Render");
                throw new NotImplementedException();
            }

            if (renderType == RenderType.OffScreen)
            {
                RenderTarget.ClearBuffer();
                RenderTarget.BindRenderTarget(OutputTexture);
                Plane.Shader = ShaderItem;
                Plane.Render(scene);
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
        public virtual void CreateRenderTarget(int width, int height)
        {
            OutputTexture = new Texture[] { TextureFactory.Instance.CreateTexture("Texture:" + Name, width, height) };
            RenderTarget = RenderTargetFactory.Instance.CreateRenderTarget("RenderTarget:" + Name, width, height, OutputTexture.Length);
            //RenderTarget = RenderTargetFactory.Instance.Default;
            RenderTarget.SizeChanged(width, height);
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
            if (ShaderItem.SetValue(memberName, value))
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
            // gbuffer用 以外はシェーダ作成
            if (vertexShader != null && fragShader != null)
            {
                ShaderItem = ShaderFactory.Instance.CreateShaderVF(vertexShader, fragShader);
            }

            Plane = RenderObjectFactory.Instance.CreateRenderObject(Name, AssetFactory.Instance.CreatePlane(Name));
            CreateRenderTarget(KI.Gfx.GLUtil.DeviceContext.Instance.Width, KI.Gfx.GLUtil.DeviceContext.Instance.Height);
            Initialize();
        }
        #endregion
    }
}
