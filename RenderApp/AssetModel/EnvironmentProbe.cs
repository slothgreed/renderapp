using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.Core;
namespace RenderApp.AssetModel
{
    public class EnvironmentProbe : KIObject
    {
        public EnvironmentProbe(string name)
            :base(name)
        {

        }
        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
