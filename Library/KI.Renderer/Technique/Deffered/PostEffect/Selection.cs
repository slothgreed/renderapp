﻿using KI.Gfx;

namespace KI.Renderer.Technique
{
    /// <summary>
    /// 形状選択ようのレンダリング
    /// </summary>
    public partial class Selection : DefferedTechnique
    {
        private int _uID;
        public int uID
        {
            get
            {
                return _uID;
            }

            set
            {
                SetValue<int>(ref _uID, value);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Selection(RenderSystem renderer, string vertexShader, string fragShader)
            : base("SelectionBuffer", renderer, vertexShader, fragShader)
        {
            var textures = RenderSystem.RenderQueue.OutputTexture<GBuffer>();
            Rectangle.Polygon.Material.AddTexture(TextureKind.Normal, textures[(int)GBuffer.OutputTextureType.Color]);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            uID = -1;
        }

        /// <summary>
        /// 形状の選択
        /// </summary>
        public void SelectObject()
        {
            if (RenderSystem.ActiveScene.SelectNode != null)
            {
                if (RenderSystem.ActiveScene.SelectNode is PolygonNode)
                {
                    var polygonNode = RenderSystem.ActiveScene.SelectNode as PolygonNode;
                    uID = polygonNode.ID;
                }
                else
                {
                    uID = 0;
                }
            }
            else
            {
                uID = 0;
            }
        }
    }
}
