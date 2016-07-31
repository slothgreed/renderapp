using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace RenderApp.AssetModel.LightModel
{
    class PointLight : Light
    {
        public PointLight(Vector3 lightPos, Vector3 lightDir) : base(lightPos,lightDir)
        {
        }

    }
}
