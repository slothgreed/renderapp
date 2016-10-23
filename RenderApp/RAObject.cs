using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp
{
    public abstract class RAObject
    {
        private static int objectCounter = 0;
        public RAObject()
        {
        }
        private string _key = null;
        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value + (objectCounter++).ToString();
            }
        }

        internal virtual void Dispose()
        {
            
        }
    }
}
