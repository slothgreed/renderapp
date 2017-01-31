using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.Foundation.Core
{
    public abstract class KIObject
    {
        public KIObject()
        {
        }
        public KIObject(string name)
        {
            Name = name;
        }
        private string _name = null;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public virtual void Dispose()
        {
            
        }
        public override string ToString()
        {
            return Name;
        } 
    }
}
