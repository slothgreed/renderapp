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
                    List<int> intTmp = (List<int>)data;
                    GL.BufferData(Target, (IntPtr)(intTmp.Count * sizeof(int)), intTmp.ToArray(), UsageHint);
                    break;
                case EArrayType.FloatArray:
                    List<float> fArray = (List<float>)data;
                    GL.BufferData(Target, (IntPtr)(fArray.Count * sizeof(float)), fArray.ToArray(), UsageHint);
                    break;
                case EArrayType.DoubleArra:
                    List<double> dArray = (List<double>)data;
                    GL.BufferData(Target, (IntPtr)(dArray.Count * sizeof(double)), dArray.ToArray(), UsageHint);
                    break;
                case EArrayType.Vec2Array:
                    List<Vector2> v2Array = (List<Vector2>)data;
                    GL.BufferData(Target, (IntPtr)(v2Array.Count * Vector2.SizeInBytes), v2Array.ToArray(), UsageHint);
                    break;
                case EArrayType.Vec3Array:
                    List<Vector3> v3Array = (List<Vector3>)data;
                    GL.BufferData(Target, (IntPtr)(v3Array.Count * Vector3.SizeInBytes), v3Array.ToArray(), UsageHint);
                    break;
                case EArrayType.Vec4Array:
                    List<Vector4> v4Array = (List<Vector4>)data;
                    GL.BufferData(Target, (IntPtr)(v4Array.Count * Vector4.SizeInBytes), v4Array.ToArray(), UsageHint);
                    break;
                default:
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
