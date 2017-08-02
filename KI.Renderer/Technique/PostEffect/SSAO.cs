﻿using System;

namespace KI.Renderer
{
    /// <summary>
    /// SSAO
    /// </summary>
    public partial class SSAO : RenderTechnique
    {
        /// <summary>
        /// 頂点シェーダ
        /// </summary>
        private static string vertexShader = Global.ShaderDirectory + @"\ssao.vert";

        /// <summary>
        /// フラグシェーダ
        /// </summary>
        private static string fragShader = Global.ShaderDirectory + @"\ssao.frag";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SSAO()
            : base("SSAO", vertexShader, fragShader, RenderTechniqueType.SSAO, RenderType.OffScreen)
        {
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            uSample = new float[1200];
            float radius = 0.02f;//10.0f / m_ViewPort.w;
            Random rand = new Random();
            float[] val = new float[uSample.Length];

            for (int i = 0; i < val.Length; i += 3)
            {
                float r = (float)(radius * rand.NextDouble());
                float t = (float)(Math.PI * 2 * rand.NextDouble());
                float cp = (float)(2 * rand.NextDouble()) - 1.0f;
                float sp = (float)Math.Sqrt(1.0f - cp * cp);
                float ct = (float)Math.Cos(t);
                float st = (float)Math.Sin(t);
                val[i] = r * sp * ct;
                val[i + 1] = r * sp * st;
                val[i + 2] = r * cp;
            }

            uSample = val;
        }
    }
}
