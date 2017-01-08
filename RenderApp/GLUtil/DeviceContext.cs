using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
namespace RenderApp.GLUtil
{
    /// <summary>
    /// enableCapから使いそうなもの抽出
    /// </summary>
    public enum EEnableCap
    {
        
    }
    public class DeviceContext
    {
        private static DeviceContext _Instance;
        public static DeviceContext Instance
        {
            get
            {
                if(_Instance == null)
                {
                    _Instance = new DeviceContext();
                }
                return _Instance;
            }
        }
        public int Width
        {
            get;
            set;
        }
        public int Height
        {
            get;
            set;
        }
        private DeviceContext()
        {

        }
        public void Initialize(int width,int height)
        {
            GL.ClearColor(1, 1, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Enable(EnableCap.DepthTest);
            Enable(EnableCap.CullFace);
            Enable(EnableCap.AlphaTest);
            Enable(EnableCap.PolygonOffsetFill);
            Enable(EnableCap.Texture2D);
            PolygonOffset(1.0f, 1.0f);
            FrontFace(FrontFaceDirection.Ccw);

            SizeChanged(width, height);
        }

        public void CullFace(CullFaceMode value)
        {
            GL.CullFace(value);
        }
        public void PolygonOffset(float factor, float value)
        {
            GL.PolygonOffset(factor, value);
        }
        public void FrontFace(FrontFaceDirection value)
        {
            GL.FrontFace(value);
        }
        public void Enable(EnableCap enable)
        {
            GL.Enable(enable);
        }
        public void DrawArrays(PrimitiveType type,int first,int count)
        {
            GL.DrawArrays(type,first,count);
        }
        public void DrawElements(PrimitiveType type,int count,DrawElementsType elementType,int indices)
        {
            GL.DrawElements(type, count, elementType, indices);
        }

        internal void SizeChanged(int width, int height)
        {
            Width = width;
            Height = height;
            GL.Viewport(0, 0, Width, Height);
        }

        internal void ReadPixel(System.Drawing.Imaging.BitmapData data)
        {
            GL.ReadPixels(0, 0, Width, Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
        }

        internal void Clear()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
    }
}
