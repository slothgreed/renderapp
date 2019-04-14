using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Gfx.GLUtil
{
    /// <summary>
    /// 配列バッファ
    /// </summary>
    public class ArrayBuffer : BufferObject
    {
        /// <summary>
        /// UsageHint
        /// </summary>
        public BufferUsageHint usageHint;

        /// <summary>
        /// バッファターゲット
        /// </summary>
        private BufferTarget target;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">バッファターゲット</param>
        internal ArrayBuffer(BufferTarget target)
        {
            this.target = target;
            usageHint = BufferUsageHint.StaticDraw;
            Enable = true;
            GenBuffer();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="target">バッファターゲット</param>
        internal ArrayBuffer(BufferTarget target, EArrayType arrayType)
        {
            this.target = target;
            usageHint = BufferUsageHint.StaticDraw;
            ArrayType = arrayType;
            Enable = true;
            GenBuffer();
        }

        public EArrayType ArrayType { get; private set; }

        /// <summary>
        /// バッファにデータの入力
        /// </summary>
        /// <param name="data">データ</param>
        /// <param name="arrayType">配列の型</param>
        public void SetData(object data, EArrayType type)
        {
            ArrayType = type;
            BindBuffer();
            switch (ArrayType)
            {
                case EArrayType.None:
                    break;
                case EArrayType.IntArray:
                    var intTmp = (int[])data;
                    GL.BufferData(target, (IntPtr)(intTmp.Length * sizeof(int)), intTmp, usageHint);
                    break;
                case EArrayType.FloatArray:
                    var fArray = (float[])data;
                    GL.BufferData(target, (IntPtr)(fArray.Length * sizeof(float)), fArray, usageHint);
                    break;
                case EArrayType.DoubleArra:
                    var dArray = (double[])data;
                    GL.BufferData(target, (IntPtr)(dArray.Length * sizeof(double)), dArray, usageHint);
                    break;
                case EArrayType.Vec2Array:
                    var v2Array = (Vector2[])data;
                    GL.BufferData(target, (IntPtr)(v2Array.Length * Vector2.SizeInBytes), v2Array, usageHint);
                    break;
                case EArrayType.Vec3Array:
                    var v3Array = (Vector3[])data;
                    GL.BufferData(target, (IntPtr)(v3Array.Length * Vector3.SizeInBytes), v3Array, usageHint);
                    break;
                case EArrayType.Vec4Array:
                    var v4Array = (Vector4[])data;
                    GL.BufferData(target, (IntPtr)(v4Array.Length * Vector4.SizeInBytes), v4Array, usageHint);
                    break;
                default:
                    new Exception();
                    break;
            }

            UnBindBuffer();
        }

        /// <summary>
        /// バッファの生成
        /// </summary>
        protected override void GenBufferCore()
        {
            this.DeviceID = GL.GenBuffer();
        }

        /// <summary>
        /// バッファのバインド
        /// </summary>
        protected override void BindBufferCore()
        {
            GL.BindBuffer(target, DeviceID);
        }

        /// <summary>
        /// バッファのバインド解除
        /// </summary>
        protected override void UnBindBufferCore()
        {
            GL.BindBuffer(target, 0);
        }

        /// <summary>
        /// バッファの解放
        /// </summary>
        protected override void DisposeCore()
        {
            GL.DeleteBuffer(DeviceID);
        }

        /// <summary>
        /// 解放不要な情報のみのコピーを返す。
        /// </summary>
        public ArrayBuffer ShallowCopy()
        {
            var buffer = new ArrayBuffer(target);
            // setdataを行っていない状態orコンストラクタで定義していないとArrayTypeはNoneになる。
            buffer.ArrayType = ArrayType;
            buffer.DeviceID = DeviceID;
            return buffer;
        }
    }
}
