using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.Utility;
using System.Diagnostics;

namespace KI.Gfx.GLUtil
{
    public enum EArrayType
    {
        None,
        IntArray,
        FloatArray,
        DoubleArra,
        Vec2Array,
        Vec3Array,
        Vec4Array,
    }
   
    public abstract class BufferObject
    {
        public bool NowBind;
        public int DeviceID { get; set; }
        public abstract void PreGenBuffer();
        public abstract void PreDispose();
        public abstract void PreBindBuffer();
        public abstract void PreUnBindBuffer();

        private static int CreateNum = 0;
        public bool Enable
        {
            get;
            set;
        }
        public virtual void GenBuffer()
        {
            CreateNum++;
            if (DeviceID != -1)
            {
                Dispose();
            }
            PreGenBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }
        public virtual void BindBuffer()
        {
            PreBindBuffer();
            if(NowBind)
            {
                Logger.Log(Logger.LogLevel.Warning,"Duplicate Bind Error");
            }
            NowBind = true;
            Logger.GLLog(Logger.LogLevel.Error);
        }
        public virtual void UnBindBuffer()
        {
            PreUnBindBuffer();
            NowBind = false;
            Logger.GLLog(Logger.LogLevel.Error);
        }
        public virtual void Dispose()
        {
            CreateNum--;
            PreDispose();
            DeviceID = -1;
        }
        public BufferObject()
        {
            DeviceID = -1;
        }
    }
}
