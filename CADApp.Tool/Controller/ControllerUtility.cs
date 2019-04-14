using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CADApp.Model;
using KI.Asset;
using KI.Gfx.GLUtil;
using KI.Mathmatics;
using OpenTK;

namespace CADApp.Tool.Controller
{
    public static class ControllerUtility
    {
        /// <summary>
        /// クリックした位置と平面の公式からWorld座標の位置を抽出する
        /// </summary>
        /// <param name="camera">カメラ</param>
        /// <param name="planeFormula">平面の公式</param>
        /// <param name="mouse">マウス情報</param>
        /// <param name="worldPosition">ワールド座標の位置</param>
        /// <returns>成功</returns>
        public static bool GetClickWorldPosition(Camera camera, Vector4 planeFormula, KIMouseEventArgs mouse, out Vector3 worldPosition)
        {
            Vector3 near;
            Vector3 far;
            GLUtility.GetClickPos(camera.Matrix, camera.ProjMatrix, Viewport.Instance.ViewportRect, mouse.Current, out near, out far);
            var result = Interaction.PlaneToLine(camera.Position, far, planeFormula, out worldPosition);

            return result;
        }
    }
}
