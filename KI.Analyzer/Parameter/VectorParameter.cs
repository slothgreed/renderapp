using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace KI.Analyzer
{
    public class VectorParameter<T>
    {
        private T[] values;

        public VectorParameter(T[] val)
        {
            values = val;
        }

        public T GetValue(int index)
        {
            return values[index];
        }
    }
}
