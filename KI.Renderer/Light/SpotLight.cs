using OpenTK;

namespace KI.Renderer
{
    /// <summary>
    /// スポットライト
    /// </summary>
    public class SpotLight : Light
    {
        /// <summary>
        /// スポットライト
        /// </summary>
        public SpotLight(string name, Vector3 lightPos, Vector3 lightDir, float spotRangeDegree = 90, float fallof = 0)
            : base(name, lightPos, lightDir)
        {
            SpotRange = spotRangeDegree;
            Fallof = fallof;
        }

        public float SpotRange { get; private set; }

        public float Fallof { get; private set; }

        public override void RenderCore(Scene scene)
        {
            // TODO:
        }
    }
}
