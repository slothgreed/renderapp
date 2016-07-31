using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace RenderApp.AssetModel.LightModel
{
    class SpotLight : Light
    {
        public float SpotRange { get; set; }
        public float Fallof { get; set; }

        /// <summary>
        /// スポットライト
        /// </summary>
        public SpotLight(Vector3 lightPos, Vector3 lightDir, float SpotRangeDegree = 90, float fallof = 0) : base(lightPos,lightDir)
        {
            SpotRange = SpotRangeDegree;
            Fallof = fallof;
        }
    }
}
