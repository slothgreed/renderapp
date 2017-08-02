using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Analyzer
{
    public interface IParameter
    {
        void AddValue(object value);

        object GetValue(int index);
    }
}
