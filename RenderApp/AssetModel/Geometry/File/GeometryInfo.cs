using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace RenderApp.AssetModel
{
    public enum GeometryType
    {
        None,
        Triangle,
        Quad,
        Mix
    }
    /// <summary>
    /// 頂点情報
    /// </summary>
    public class VertexInfo
    {
        public List<Vector3> Position = new List<Vector3>();
        public List<Vector3> Normal = new List<Vector3>();
        public List<Vector3> Color = new List<Vector3>();
        public List<Vector2> TexCoord = new List<Vector2>();
    }
    public class VertexArrayInfo
    {
        /// <summary>
        /// 頂点配列用
        /// </summary>
        public List<int> posIndex = new List<int>();
        public List<int> texIndex = new List<int>();
        public List<Vector3> colIndex = new List<Vector3>();
        public List<int> norIndex = new List<int>();
    }
    /// <summary>
    /// 形状情報
    /// </summary>
    public class GeometryInfo
    {
        public GeometryInfo()
        {
        }
        public string FilePath { get; private set; }
        public string group;
        public List<Vector3> Position = new List<Vector3>();
        public List<Vector3> Normal = new List<Vector3>();
        public List<Vector3> Color = new List<Vector3>();
        public List<Vector2> TexCoord = new List<Vector2>();
        /// <summary>
        /// ファイルの読み込んだ順番に格納される
        /// </summary>
        public List<Vector3> posStream = new List<Vector3>();
        public List<Vector3> norStream = new List<Vector3>();
        public List<Vector3> colStream = new List<Vector3>();
        public List<Vector2> texStream = new List<Vector2>();
        /// <summary>
        /// 頂点配列用
        /// </summary>
        public List<int> posIndex = new List<int>();
        public List<int> texIndex = new List<int>();
        public List<Vector3> colIndex = new List<Vector3>();
        public List<int> norIndex = new List<int>();
        public MaterialFileInfo Material;
        public GeometryType geometryType = GeometryType.None;
    }
}
