namespace KI.Renderer
{
    partial class Sobel : RenderTechnique
    {
        private static string vertexShader = Global.ShaderDirectory + @"\PostEffect\sobel.vert";
        private static string fragShader = Global.ShaderDirectory + @"\PostEffect\sobel.frag";

        #region [Shaderの初期化関数]
        public Sobel(RenderTechniqueType tech)
            : base("Sobel", vertexShader, fragShader, tech, RenderType.OffScreen)
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
