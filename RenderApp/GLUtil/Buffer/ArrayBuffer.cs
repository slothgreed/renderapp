using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderApp.Utility;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace RenderApp.GLUtil.Buffer
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
   
    class ArrayBuffer : BufferObject
    {
        public BufferUsageHint UsageHint
        {
            get;
            set;
        }
        public BufferTarget Target
        {
            get;
            set;
        }
        public object Data
        {
            get;
            set;
        }
        public EArrayType ArrayType
        {
            get;
            set;
        }

        public ArrayBuffer()
        {
            Target = BufferTarget.ArrayBuffer;
            UsageHint = BufferUsageHint.StaticDraw;
        }
        public ArrayBuffer(BufferTarget target)
        {
            Target = target;
        }
        public override void PreGenBuffer()
        {
           this.ID = GL.GenBuffer();
        }
        public void SetData(object data,EArrayType arrayType)
        {
            Data = data;
            ArrayType = arrayType;

            switch (ArrayType)
            {
                case EArrayType.None:
                    break;
                case EArrayType.IntArray:
                    List<int> intTmp = (List<int>)data;
                    GL.BufferData(Target, (IntPtr)(intTmp.Count * sizeof(int)), intTmp.ToArray(), UsageHint);
                    break;
                case EArrayType.FloatArray:
                    List<int> fArray = (List<int>)data;
                    GL.BufferData(Target, (IntPtr)(fArray.Count * sizeof(float)), fArray.ToArray(), UsageHint); 
                    break;
                case EArrayType.DoubleArra:
                    List<int> dArray = (List<int>)data;
                    GL.BufferData(Target, (IntPtr)(dArray.Count * sizeof(double)), dArray.ToArray(), UsageHint);
                    break;
                case EArrayType.Vec2Array:
                    List<int> v2Array = (List<int>)data;
                    GL.BufferData(Target, (IntPtr)(v2Array.Count * Vector2.SizeInBytes), v2Array.ToArray(), UsageHint);
                    break;
                case EArrayType.Vec3Array:
                    List<int> v3Array = (List<int>)data;
                    GL.BufferData(Target, (IntPtr)(v3Array.Count * Vector3.SizeInBytes), v3Array.ToArray(), UsageHint);
                    break;
                case EArrayType.Vec4Array:
                    List<int> v4Array = (List<int>)data;
                    GL.BufferData(Target, (IntPtr)(v4Array.Count * Vector4.SizeInBytes), v4Array.ToArray(), UsageHint);
                    break;
                default:
                    break;
            }
        }
        
        public override void PreBindBuffer()
        {
            GL.BindBuffer(Target, ID);
        }
        public override void PreUnBindBuffer()
        {
            GL.BindBuffer(Target, 0);
        }

        public override void PreDispose()
        {
            GL.DeleteBuffer(ID);
        }
    }
}
