using KI.Analyzer;
using KI.Analyzer.Algorithm;
using KI.Foundation.Controller;
using KI.Renderer;
using RenderApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RenderApp.Tool.Controller
{
    /// <summary>
    /// シェイプマッチング用のコントローラ
    /// </summary>
    public class ShapeMatchingController : ControllerBase
    {
        /// <summary>
        /// シェイプマッチングアルゴリズム
        /// </summary>
        private ShapeMatchingAlgorithm shapeMatching;

        /// <summary>
        /// 形状の選択
        /// </summary>
        private PolygonNode selectObject;

        /// <summary>
        /// キー押下イベント
        /// </summary>
        /// <param name="e">キー</param>
        /// <returns>成功</returns>
        public override bool KeyDown(KeyEventArgs e)
        {
            if(e.KeyCode == Keys.S)
            {
                selectObject = Workspace.Instance.RenderSystem.ActiveScene.SelectNode as PolygonNode;
                if(selectObject == null)
                {
                    return false;
                }

                if (shapeMatching == null)
                {
                    if (selectObject.Polygon is HalfEdgeDS)
                    {
                        shapeMatching = new ShapeMatchingAlgorithm(selectObject.Polygon as HalfEdgeDS);
                    }
                }

                shapeMatching.Update(1);
                selectObject.UpdateVertexBufferObject();
            }

            if (e.KeyCode == Keys.E)
            {
                if (shapeMatching != null)
                {
                    shapeMatching.Dispose();
                    shapeMatching = null;
                }

                if (selectObject != null)
                {
                    selectObject.UpdateVertexBufferObject();
                    selectObject = null;
                }
            }

            return true;
        }
    }
}
