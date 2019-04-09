using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// 平行光源
    /// </summary>
    public class DirectionLight : Light
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="lightPos">位置</param>
        /// <param name="lightDir">方向</param>
        public DirectionLight(string name, Vector3 lightPos, Vector3 lightDir)
            : base(name, lightPos, lightDir)
        {
        }
    }
}
