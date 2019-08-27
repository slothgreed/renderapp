using OpenTK;

namespace KI.Mathmatics
{
    /// <summary>
    /// GL用Utility
    /// </summary>
    public static class GLUtility
    {
        /// <summary>
        /// GLのUnproject
        /// </summary>
        /// <param name="mosue">マウス位置、z=0or1,w=1</param>
        public static Vector3 UnProject(Vector3 mouse, Matrix4 cameraMatrix, Matrix4 projMatrix, int[] viewPort)
        {
            Vector4 projPos = new Vector4(mouse, 1.0f);
            Matrix4 modelproj = cameraMatrix * projMatrix;
            modelproj.Invert();
            Vector4 ret = new Vector4();

            projPos.X = (mouse.X - viewPort[0]) / (float)viewPort[2];
            projPos.Y = (mouse.Y - viewPort[1]) / (float)viewPort[3];

            projPos.X = (projPos.X * 2) - 1.0f;
            projPos.Y = (projPos.Y * 2) - 1.0f;
            projPos.Z = (projPos.Z * 2) - 1.0f;
            projPos.Y *= -1;
            projPos.W = 1.0f;

            ret = Vector4.Transform(projPos, modelproj);
            if (ret.W == 0.0f)
            {
                return Vector3.Zero;
            }

            ret.X /= ret.W;
            ret.Y /= ret.W;
            ret.Z /= ret.W;
            return new Vector3(ret.X, ret.Y, ret.Z);
        }

        /// <summary>
        /// Near面Far面からクリックした線分の取得
        /// </summary>
        public static void GetClickPos(Matrix4 cameraMatrix, Matrix4 projMatrix, int[] viewport, Vector2 mouse, out Vector3 near, out Vector3 far)
        {
            near = UnProject(new Vector3(mouse.X, mouse.Y, 0.0f), cameraMatrix, projMatrix, viewport);
            far = UnProject(new Vector3(mouse.X, mouse.Y, 1.0f), cameraMatrix, projMatrix, viewport);
        }
    }
}
