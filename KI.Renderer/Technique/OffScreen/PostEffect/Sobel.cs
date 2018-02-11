namespace KI.Renderer.Technique
{
    /// <summary>
    /// Sobel
    /// </summary>
    public partial class Sobel : RenderTechnique
    {
        /// <summary>
        /// 閾値の最大値
        /// </summary>
        public float Max { get; set; } = 1;

        /// <summary>
        /// 閾値の最小値
        /// </summary>
        public float Min { get; set; } = 0;

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
    }
}
