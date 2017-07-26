using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace KI.Asset
{
    class DirectionLight : Light
    {
        public DirectionLight(string name,Vector3 lightPos, Vector3 lightDir)
            : base(name,lightPos, lightDir)
        {
        }
    }
}
