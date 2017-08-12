using KI.Foundation.Core;
using KI.Foundation.Utility;

namespace KI.Gfx.GLUtil
{
    /// <summary>
    /// 配列の種類
    /// </summary>
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

    /// <summary>
    /// バッファの種類
    /// </summary>
    public enum BufferType
    {
        Array,
        Frame,
        Render,
        Sampler,
        Texture
    }

    /// <summary>
    /// バッファ
    /// </summary>
    public abstract class BufferObject : KIObject
    {
        /// <summary>
        /// 生成した数
        /// </summary>
        private static int createNum = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected BufferObject()
            : base("BufferObject")
        {
            DeviceID = -1;
        }

        /// <summary>
        /// 有効
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// デバイスID
        /// </summary>
        public int DeviceID { get; protected set; }

        /// <summary>
        /// バインド中か
        /// </summary>
        protected bool NowBind { get; set; }

        /// <summary>
        /// バッファの生成
        /// </summary>
        protected abstract void GenBufferCore();

        /// <summary>
        /// バッファの解放
        /// </summary>
        protected abstract void DisposeCore();

        /// <summary>
        /// バッファのバインド
        /// </summary>
        protected abstract void BindBufferCore();

        /// <summary>
        /// バッファのバインド解除
        /// </summary>
        protected abstract void UnBindBufferCore();

        /// <summary>
        /// バッファの生成
        /// </summary>
        public virtual void GenBuffer()
        {
            createNum++;
            if (DeviceID != -1)
            {
                Dispose();
            }

            GenBufferCore();
            Logger.GLLog(Logger.LogLevel.Error);
        }

        /// <summary>
        /// バッファのバインド
        /// </summary>
        public virtual void BindBuffer()
        {
            BindBufferCore();
            if (NowBind)
            {
                Logger.Log(Logger.LogLevel.Warning, "Duplicate Bind Error");
            }

            NowBind = true;
            Logger.GLLog(Logger.LogLevel.Error);
        }

        /// <summary>
        /// バッファのバインド解除
        /// </summary>
        public virtual void UnBindBuffer()
        {
            UnBindBufferCore();
            NowBind = false;
            Logger.GLLog(Logger.LogLevel.Error);
        }

        /// <summary>
        /// バッファの解放
        /// </summary>
        public override void Dispose()
        {
            createNum--;
            DisposeCore();
            DeviceID = -1;
            if(createNum < 0)
            {
                Logger.Log(Logger.LogLevel.Error, "Dispose Error");
            }
        }
    }
}
