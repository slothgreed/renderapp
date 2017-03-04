using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KI.Gfx.GLUtil;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace KI.Gfx.KIAsset
{
    public enum GeometryType
    {
        None,
        Triangle,
        Quad,
        Mix
    }
    public enum VertexStoreType
    {
        None,       //入っていない
        Normal,     //普通に順番に入っている
        VertexArray //VertexArray状態で入っている
    }
    /// <summary>
    /// 頂点情報
    /// </summary>
    public class GeometryInfo
    {
        public List<Vector3> Position = new List<Vector3>();
        public List<Vector3> Normal = new List<Vector3>();
        public List<Vector3> Color = new List<Vector3>();
        public List<Vector2> TexCoord = new List<Vector2>();
        public List<int> Index = new List<int>();

        public ArrayBuffer PositionBuffer { get; set; }
        public ArrayBuffer NormalBuffer { get; set; }
        public ArrayBuffer ColorBuffer { get; set; }
        public ArrayBuffer TexCoordBuffer { get; set; }
        public ArrayBuffer IndexBuffer { get; set; }

        public void Dispose()
        {
            Position.Clear();
            Color.Clear();
            Normal.Clear();
            TexCoord.Clear();
            Index.Clear();

            if(PositionBuffer != null)
            {
                PositionBuffer.Dispose();
            }
            if(NormalBuffer != null)
            {
                NormalBuffer.Dispose();
            }
            if (ColorBuffer != null)
            {
                ColorBuffer.Dispose();
            }
            if (TexCoordBuffer != null)
            {
                TexCoordBuffer.Dispose();
            }
            if (IndexBuffer != null)
            {
                IndexBuffer.Dispose();
            }
        }

        public int TriangleNum
        {
            get
            {
                if (Index.Count == 0)
                {
                    return Position.Count / 3;
                }
                else
                {
                    return Index.Count / 3;
                }
            }
        }

        public void GenBuffer()
        {
            if (Position.Count != 0)
            {
                PositionBuffer = new ArrayBuffer();
                PositionBuffer.GenBuffer();
                PositionBuffer.SetData(Position, EArrayType.Vec3Array);
            }
            if (Normal.Count != 0)
            {
                NormalBuffer = new ArrayBuffer();
                NormalBuffer.GenBuffer();
                NormalBuffer.SetData(Normal, EArrayType.Vec3Array);
            }
            if (Color.Count != 0)
            {
                ColorBuffer = new ArrayBuffer();
                ColorBuffer.GenBuffer();
                ColorBuffer.SetData(Color, EArrayType.Vec3Array);
            }
            if (TexCoord.Count != 0)
            {
                TexCoordBuffer = new ArrayBuffer();
                TexCoordBuffer.GenBuffer();
                TexCoordBuffer.SetData(TexCoord, EArrayType.Vec2Array);
            }
            if (Index.Count != 0)
            {
                IndexBuffer = new ArrayBuffer(BufferTarget.ElementArrayBuffer);
                IndexBuffer.GenBuffer();
                IndexBuffer.SetData(Index, EArrayType.IntArray);
            }
        }

        #region [convert mesh]
        /// <summary>
        /// Triangle毎に変換
        /// </summary>
        protected void ConvertPerTriangle()
        {
            if (Index.Count == 0)
                return;

            if (Position.Count == 0)
                return;

            bool texArray = false;
            bool colorArray = false;
            bool normalArray = false;
            if (TexCoord.Count == Position.Count)
                texArray = true;

            if (Normal.Count == Position.Count)
                normalArray = true;

            if (Color.Count == Position.Count)
                colorArray = true;

            var newPosition = new List<Vector3>();
            var newTexcoord = new List<Vector2>();
            var newColor = new List<Vector3>();
            var newNormal = new List<Vector3>();


            for (int i = 0; i < Index.Count; i += 3)
            {
                newPosition.Add(Position[Index[i]]);
                newPosition.Add(Position[Index[i + 1]]);
                newPosition.Add(Position[Index[i + 2]]);
                if (texArray)
                {
                    newTexcoord.Add(TexCoord[Index[i]]);
                    newTexcoord.Add(TexCoord[Index[i + 1]]);
                    newTexcoord.Add(TexCoord[Index[i + 2]]);
                }
                if (colorArray)
                {
                    newColor.Add(Color[Index[i]]);
                    newColor.Add(Color[Index[i + 1]]);
                    newColor.Add(Color[Index[i + 2]]);
                }

                if (normalArray)
                {
                    newNormal.Add(Normal[Index[i]]);
                    newNormal.Add(Normal[Index[i + 1]]);
                    newNormal.Add(Normal[Index[i + 2]]);
                }
            }

            Position = newPosition;
            Normal = newNormal;
            TexCoord = newTexcoord;
            Color = newColor;
            Index.Clear();
        }
        /// <summary>
        /// 頂点配列に変換
        /// </summary>
        public void ConvertVertexArray()
        {
            if (Index.Count != 0)
                return;

            if (Position.Count == 0)
                return;

            bool texArray = false;
            bool colorArray = false;
            bool normalArray = false;
            if (TexCoord.Count == Position.Count)
                texArray = true;

            if (Normal.Count == Position.Count)
                normalArray = true;

            if (Color.Count == Position.Count)
                colorArray = true;

            var newPosition = new List<Vector3>();
            var newTexcoord = new List<Vector2>();
            var newColor = new List<Vector3>();
            var newNormal = new List<Vector3>();
            bool isExist = false;
            for (int i = 0; i < Position.Count; i++)
            {
                isExist = false;
                for (int j = 0; j < newPosition.Count; j++)
                {
                    if (newPosition[j] == Position[i])
                    {
                        isExist = true;
                        Index.Add(j);
                        break;
                    }
                }
                if (!isExist)
                {
                    newPosition.Add(Position[i]);
                    Index.Add(newPosition.Count - 1);


                    if (texArray)
                    {
                        newTexcoord.Add(TexCoord[i]);
                    }
                    if (colorArray)
                    {
                        newColor.Add(Color[i]);
                    }
                    if (normalArray)
                    {
                        newNormal.Add(Normal[i]);
                    }
                }
            }
            Position = newPosition;
            TexCoord = newTexcoord;
            Color = newColor;
            Normal = newNormal;

        }
        #endregion


    }
}
