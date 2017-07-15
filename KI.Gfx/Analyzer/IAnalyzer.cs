using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.Core;
namespace KI.Gfx.Analyzer
{
    public abstract class IAnalyzer : KIObject
    {
        public Dictionary<Enum, IParameter> Parameters = new Dictionary<Enum, IParameter>();
    }
}
