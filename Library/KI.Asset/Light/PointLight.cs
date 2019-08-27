using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// 点光源
    /// </summary>
    public class PointLight : Light
    {
        /// <summary>
        /// 点光源
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="lightPos">位置</param>
        /// <param name="lightDir">方向</param>
        public PointLight(string name, Vector3 lightPos)
            : base(name, lightPos, Vector3.Zero)
        {
        }
    }
}
