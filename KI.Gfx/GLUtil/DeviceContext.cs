using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using KI.Gfx.KIAsset;
namespace KI.Gfx.GLUtil
{
    public class DeviceContext
    {
        /// <summary>
        /// Multi化はしない
        /// </summary>
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
            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Enable(EnableCap.DepthTest);
            Enable(EnableCap.CullFace);
            Enable(EnableCap.AlphaTest);
            Enable(EnableCap.PolygonOffsetFill);
            Enable(EnableCap.Texture2D);
            Enable(EnableCap.TextureCubeMap);
            GL.PointSize(2.0f);
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
        private PrimitiveType ConvertToPrimitiveType(GeometryType type)
        {
            switch (type)
            {
                case GeometryType.None:
                case GeometryType.Point:
                    return PrimitiveType.Points;
                case GeometryType.Line:
                    return PrimitiveType.Lines;
                case GeometryType.Triangle:
                    return PrimitiveType.Triangles;
                case GeometryType.Quad:
                    return PrimitiveType.Quads;
                case GeometryType.Mix:
                    break;
                case GeometryType.Patch:
                    return PrimitiveType.Patches;
                default:
                    break;
            }
            return PrimitiveType.Points;
        }
        public void DrawArrays(GeometryType type, int first, int count)
        {
            GL.DrawArrays(ConvertToPrimitiveType(type), first, count);
        }
        public void DrawElements(GeometryType type,int count,DrawElementsType elementType,int indices)
        {
            GL.DrawElements(ConvertToPrimitiveType(type), count, elementType, indices);
        }

        public void SizeChanged(int width, int height)
        {
            Width = width;
            Height = height;
            GL.Viewport(0, 0, Width, Height);
        }

        public void ReadPixel(System.Drawing.Imaging.BitmapData data)
        {
            GL.ReadPixels(0, 0, Width, Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
        }

        public void Clear()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
    }
}
