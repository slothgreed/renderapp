using OpenTK;

namespace KI.Asset
{
    /// <summary>
    /// スポットライト
    /// </summary>
    class SpotLight : Light
    {
        /// <summary>
        /// スポットライト
        /// </summary>
        public SpotLight(string name, Vector3 lightPos, Vector3 lightDir, float SpotRangeDegree = 90, float fallof = 0)
            : base(name, lightPos, lightDir)
        {
            SpotRange = SpotRangeDegree;
            Fallof = fallof;
        }

        public float SpotRange { get; private set; }

        public float Fallof { get; private set; }
    }
}
