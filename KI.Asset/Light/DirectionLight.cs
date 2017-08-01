using OpenTK;

namespace KI.Asset
{
    class DirectionLight : Light
    {
        public DirectionLight(string name, Vector3 lightPos, Vector3 lightDir)
            : base(name, lightPos, lightDir)
        {
        }
    }
}
