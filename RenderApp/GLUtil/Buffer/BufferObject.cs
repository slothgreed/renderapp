using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderApp.GLUtil
{
    public abstract class BufferObject
    {
        public int ID { get; set; }
        public abstract void GenBuffer();
        public abstract void BindBuffer();
        public abstract void UnBindBuffer();
        public abstract void Dispose();
        public BufferObject()
        {
            ID = -1;
        }
    }
}
