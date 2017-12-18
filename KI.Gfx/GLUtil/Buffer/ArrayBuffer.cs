using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// コンストラクタ
        /// </summary>
        /// <param name="target">バッファターゲット</param>
        internal ArrayBuffer(BufferTarget target)
        {
            Target = target;
            UsageHint = BufferUsageHint.StaticDraw;
            Enable = true;
        }

        public BufferUsageHint UsageHint { get; set; }

        public BufferTarget Target { get; set; }

        /// <summary>
        /// データ
        /// </summary>
        public object Data { get; private set; }

        /// <summary>
        /// 配列の型
        /// </summary>
        public EArrayType ArrayType { get; private set; }

        /// <summary>
        /// バッファにデータの入力
        /// </summary>
        /// <param name="data">データ</param>
        /// <param name="arrayType">配列の型</param>
        public void SetData(object data, EArrayType arrayType)
        {
            Data = data;
            ArrayType = arrayType;
            BindBuffer();
            switch (ArrayType)
            {
                case EArrayType.None:
                    break;
                case EArrayType.IntArray:
                    var intTmp = (int[])data;
                    GL.BufferData(Target, (IntPtr)(intTmp.Length * sizeof(int)), intTmp, UsageHint);
                    break;
                case EArrayType.FloatArray:
                    var fArray = (float[])data;
                    GL.BufferData(Target, (IntPtr)(fArray.Length * sizeof(float)), fArray, UsageHint);
                    break;
                case EArrayType.DoubleArra:
                    var dArray = (double[])data;
                    GL.BufferData(Target, (IntPtr)(dArray.Length * sizeof(double)), dArray, UsageHint);
                    break;
                case EArrayType.Vec2Array:
                    var v2Array = (Vector2[])data;
                    GL.BufferData(Target, (IntPtr)(v2Array.Length * Vector2.SizeInBytes), v2Array, UsageHint);
                    break;
                case EArrayType.Vec3Array:
                    var v3Array = (Vector3[])data;
                    GL.BufferData(Target, (IntPtr)(v3Array.Length * Vector3.SizeInBytes), v3Array, UsageHint);
                    break;
                case EArrayType.Vec4Array:
                    var v4Array = (Vector4[])data;
                    GL.BufferData(Target, (IntPtr)(v4Array.Length * Vector4.SizeInBytes), v4Array, UsageHint);
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
            GL.BindBuffer(Target, DeviceID);
        }

        /// <summary>
        /// バッファのバインド解除
        /// </summary>
        protected override void UnBindBufferCore()
        {
            GL.BindBuffer(Target, 0);
        }

        /// <summary>
        /// バッファの解放
        /// </summary>
        protected override void DisposeCore()
        {
            GL.DeleteBuffer(DeviceID);
        }
    }
}
