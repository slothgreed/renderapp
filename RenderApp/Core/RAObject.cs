﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp
{
    public abstract class RAObject
    {
        public RAObject()
        {
        }
        public RAObject(string name)
        {
            Key = name;
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
                _key = value;
            }
        }

        public virtual void Dispose()
        {
            
        }
        public override string ToString()
        {
            return Key;
        } 
    }
}