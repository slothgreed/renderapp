﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Foundation.Utility;
using System.Diagnostics;
using KI.Foundation.Core;

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

    public enum BufferType
    {
        Array,
        Frame,
        Render,
        Sampler,
        Texture
    }

    public abstract class BufferObject : KIObject
    {
        private static int CreateNum = 0;

        protected bool NowBind;
        public abstract void PreGenBuffer();
        public abstract void PreDispose();
        public abstract void PreBindBuffer();
        public abstract void PreUnBindBuffer();

        public BufferObject()
            : base("BufferObject")
        {
            DeviceID = -1;
        }

        public bool Enable { get; set; }
        public int DeviceID { get; protected set; }

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
            if (NowBind)
            {
                Logger.Log(Logger.LogLevel.Warning, "Duplicate Bind Error");
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

        /// <summary>
        /// 解放処理
        /// </summary>
        public override void Dispose()
        {
            CreateNum--;
            PreDispose();
            DeviceID = -1;
        }
    }
}
