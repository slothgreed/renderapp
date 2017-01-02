using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.Utility;
namespace RenderApp.GLUtil
{
    public abstract class BufferObject
    {
        public bool NowBind;
        public int DeviceID { get; set; }
        public abstract void PreGenBuffer();
        public abstract void PreDispose();
        public abstract void PreBindBuffer();
        public abstract void PreUnBindBuffer();

        public virtual void GenBuffer()
        {
            if (DeviceID != -1)
            {
                Dispose();
            }
            PreGenBuffer();
            Output.GLLog(Output.LogLevel.Error);
        }
        public virtual void BindBuffer()
        {
            PreBindBuffer();
            if(NowBind)
            {
                Output.Log(Output.LogLevel.Warning,"Duplicate Bind Error");
            }
            NowBind = true;
            Output.GLLog(Output.LogLevel.Error);
        }
        public virtual void UnBindBuffer()
        {
            PreUnBindBuffer();
            NowBind = false;
            Output.GLLog(Output.LogLevel.Error);
        }
        public virtual void Dispose()
        {
            PreDispose();
            DeviceID = -1;
        }
        public BufferObject()
        {
            DeviceID = -1;
        }
    }
}
