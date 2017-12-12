namespace KI.Renderer
{
    /// <summary>
    /// Sobel
    /// </summary>
    public partial class Sobel : RenderTechnique
    {
        #region [Shaderの初期化関数]

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Sobel(string vertexShader, string fragShader)
            : base("Sobel", vertexShader, fragShader, RenderTechniqueType.Sobel, RenderType.OffScreen)
        {
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            uThreshold = 0.001f;
            uWidth = 1;
            uHeight = 1;
            uTarget = null;
        }

        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="width">横</param>
        /// <param name="height">縦</param>
        public override void SizeChanged(int width, int height)
        {
            base.SizeChanged(width, height);
            uWidth = width;
            uHeight = height;
        }
        #endregion
    }
}
