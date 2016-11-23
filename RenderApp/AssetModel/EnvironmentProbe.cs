using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.AssetModel;

namespace RenderApp.AssetModel
{
    public class EnvironmentProbe : RAFile
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
