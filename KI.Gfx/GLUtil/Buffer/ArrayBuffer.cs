using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Gfx.GLUtil
{
    public class ArrayBuffer : BufferObject
    {
        internal ArrayBuffer()
        {
            Target = BufferTarget.ArrayBuffer;
            UsageHint = BufferUsageHint.StaticDraw;
            Enable = true;
        }

        internal ArrayBuffer(BufferTarget target)
        {
            Target = target;
            UsageHint = BufferUsageHint.StaticDraw;
            Enable = true;
        }

        public BufferUsageHint UsageHint { get; set; }

        public BufferTarget Target { get; set; }

        public object Data { get; set; }

        public EArrayType ArrayType { get; set; }

        public override void PreGenBuffer()
        {
            this.DeviceID = GL.GenBuffer();
        }

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

        public override void PreBindBuffer()
        {
            GL.BindBuffer(Target, DeviceID);
        }

        public override void PreUnBindBuffer()
        {
            GL.BindBuffer(Target, 0);
        }

        public override void PreDispose()
        {
            GL.DeleteBuffer(DeviceID);
        }
    }
}
