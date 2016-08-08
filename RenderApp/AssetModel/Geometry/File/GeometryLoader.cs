using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace RenderApp.AssetModel
{
    public class GeometryLoader : Geometry
    {
        public GeometryLoader(string name)
            :base(name)
        {

        }
        public struct CMaterial
        {
            public string name;
            public Vector3 Ka;
            public Vector3 Kd;
            public Vector3 Ks;
            public string imageFile;
        }

        /// <summary>
        /// ファイルの読み込んだ順番に格納される
        /// </summary>
        public List<Vector3> m_posStream = new List<Vector3>();
        public List<Vector3> m_norStream = new List<Vector3>();
        public List<Vector3> m_colStream = new List<Vector3>();
        public List<Vector2> m_texStream = new List<Vector2>();


        /// <summary>
        /// 頂点配列用
        /// </summary>
        public List<int> m_posIndex = new List<int>();
        public List<int> m_texIndex = new List<int>();
        public List<Vector3> m_colIndex = new List<Vector3>();
        public List<int> m_norIndex = new List<int>();
        public List<CMaterial> m_Materials = new List<CMaterial>();

    }
}
