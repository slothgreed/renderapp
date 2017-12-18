using System.Collections.Generic;
using System.Linq;
using KI.Asset;
using KI.Foundation.Utility;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Gfx.KIShader;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KI.Renderer
{
    /// <summary>
    /// レンダリング種類
    /// </summary>
    public enum RenderMode
    {
        None,
        Point,
        Line,
        Polygon,
        PolygonLine,
    }

    /// <summary>
    /// 任意形状(triangle,quad,line,patchのみ対応)
    /// </summary>
    public class RenderObject : SceneNode
    {
        /// <summary>
        /// 形状データ
        /// </summary>
        private Polygon polygon;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        public RenderObject(string name)
            : base(name)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="polygon">形状</param>
        public RenderObject(string name, Polygon polygon)
            : base(name)
        {
            SetPolygon(polygon);
        }

        /// <summary>
        /// レンダリングパッケージ
        /// </summary>
        public Dictionary<PrimitiveType, RenderPackage> Packages { get; private set; } = new Dictionary<PrimitiveType, RenderPackage>();

        /// <summary>
        /// メインのシェーダ
        /// </summary>
        public Shader Shader
        {
            get
            {
                return Packages[Polygon.Type].Shader;
            }

            set
            {
                Packages[Polygon.Type].Shader = value;
            }
        }

        /// <summary>
        /// 形状
        /// </summary>
        public Polygon Polygon
        {
            get
            {
                return polygon;
            }

            private set
            {
                if (polygon != null)
                {
                    polygon.PolygonUpdated -= OnPolygonUpdated;
                }

                polygon = value;
                polygon.PolygonUpdated += OnPolygonUpdated;
            }
        }

        /// <summary>
        /// レンダリング種類
        /// </summary>
        public RenderMode RenderMode { get; set; }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void RenderCore(IScene scene)
        {
            if (RenderMode == RenderMode.Point)
            {
                RenderPackage(scene, PrimitiveType.Points);
            }

            if (RenderMode == RenderMode.Line)
            {
                RenderPackage(scene, PrimitiveType.Lines);
            }

            if (RenderMode == RenderMode.Polygon)
            {
                RenderPackage(scene, Polygon.Type);
            }

            if (RenderMode == RenderMode.PolygonLine)
            {
                RenderPackage(scene, PrimitiveType.Lines);
                RenderPackage(scene, Polygon.Type);
            }
        }

        /// <summary>
        /// 形状をセット
        /// </summary>
        /// <param name="polygon">形状情報</param>
        public void SetPolygon(Polygon polygon)
        {
            Polygon = polygon;
            SetupRenderPackage(polygon.Type, null);

            RenderMode = PrimitiveTypeToRenderMode(Polygon.Type);
        }

        /// <summary>
        /// PrimitiveTypeからRenderModeを取得。
        /// </summary>
        /// <param name="primitiveType">PrimitiveType</param>
        /// <returns>RenderMode</returns>
        public RenderMode PrimitiveTypeToRenderMode(PrimitiveType primitiveType)
        {
            if (primitiveType == PrimitiveType.Points)
            {
                return RenderMode.Point;
            }
            else
            if (primitiveType == PrimitiveType.Lines)
            {
                return RenderMode.Line;
            }
            else
            if (primitiveType == PrimitiveType.Triangles ||
                primitiveType == PrimitiveType.Quads)
            {
                return RenderMode.Polygon;
            }

            return RenderMode.None;
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public override void Dispose()
        {
            foreach (var pack in Packages.Values)
            {
                pack.VertexBuffer.Dispose();
            }
        }

        /// <summary>
        /// レンダリング
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <param name="type">形状種類</param>
        private void RenderPackage(IScene scene, PrimitiveType type)
        {
            if (Packages.ContainsKey(type))
            {
                Packages[type].RenderPackageCore(scene, this);
            }
            else
            {
                Logger.Log(Logger.LogLevel.Warning, "can't Render" + type.ToString());
            }
        }

        /// <summary>
        /// レンダーパッケージの設定
        /// </summary>
        /// <param name="type">形状種類</param>
        /// <param name="color">色</param>
        public void SetupRenderPackage(PrimitiveType type, List<Vector3> color)
        {
            if (!Packages.ContainsKey(type))
            {
                Packages[type] = new RenderPackage(Polygon, type);
                string vert = ShaderCreater.Instance.GetVertexShader(this);
                string frag = ShaderCreater.Instance.GetFragShader(this);
                Packages[type].Shader = ShaderFactory.Instance.CreateShaderVF(vert, frag);
            }

            if(color != null)
            {
                Packages[type].Color = color;
            }

            SetupBuffer(type);
        }

        /// <summary>
        /// バッファにデータの設定
        /// </summary>
        /// <param name="polygon">ポリゴン</param>
        /// <param name="type">形状種類</param>
        /// <param name="color">頂点カラーの設定</param>
        public void SetupBuffer(PrimitiveType type)
        {
            int[] indexBuffer = null;
            Vector3[] position = null;
            Vector3[] normal = null;
            Vector3[] color = null;
            Vector2[] texCoord = null;
            if (polygon.Index.ContainsKey(type))
            {
                indexBuffer = polygon.Index[type].ToArray();
                position = polygon.Vertexs.Select(p => p.Position).ToArray();
                normal = polygon.Vertexs.Select(p => p.Normal).ToArray();
                color = polygon.Vertexs.Select(p => p.Color).ToArray();
                texCoord = polygon.Vertexs.Select(p => p.TexCoord).ToArray();
            }
            else if (type == PrimitiveType.Points)
            {
                position = polygon.Vertexs.Select(p => p.Position).ToArray();
                normal = polygon.Vertexs.Select(p => p.Normal).ToArray();
                color = polygon.Vertexs.Select(p => p.Color).ToArray();
                texCoord = polygon.Vertexs.Select(p => p.TexCoord).ToArray();
                Packages[type].VertexBuffer.SetBuffer(position, normal, color, texCoord);
            }
            else if (type == PrimitiveType.Lines)
            {
                var vertexs = new List<Vertex>();
                foreach (var line in polygon.Lines)
                {
                    vertexs.Add(line.Start);
                    vertexs.Add(line.End);
                }

                position = vertexs.Select(p => p.Position).ToArray();
                normal = vertexs.Select(p => p.Normal).ToArray();
                color = vertexs.Select(p => p.Color).ToArray();
                texCoord = vertexs.Select(p => p.TexCoord).ToArray();
            }
            else
            {
                var vertexs = new List<Vertex>();
                var normals = new List<Vector3>();

                if (type == PrimitiveType.Triangles)
                {
                    foreach (var mesh in polygon.Meshs)
                    {
                        vertexs.AddRange(mesh.Vertexs);
                        normals.Add(mesh.Normal);
                        normals.Add(mesh.Normal);
                        normals.Add(mesh.Normal);
                    }
                }
                else
                {
                    foreach (var mesh in polygon.Meshs)
                    {
                        vertexs.AddRange(mesh.Vertexs);
                        normals.Add(mesh.Normal);
                        normals.Add(mesh.Normal);
                        normals.Add(mesh.Normal);
                        normals.Add(mesh.Normal);
                    }
                }

                position = vertexs.Select(p => p.Position).ToArray();
                normal = normals.ToArray();
                color = vertexs.Select(p => p.Color).ToArray();
                texCoord = vertexs.Select(p => p.TexCoord).ToArray();
            }

            // デフォルトカラーよりすでに設定されているカラーを優先
            if(Packages[type].Color != null)
            {
                color = Packages[type].Color.ToArray();
            }
            Packages[type].VertexBuffer.SetBuffer(position, normal, color, texCoord);

            if(indexBuffer != null)
            {
                Packages[type].VertexBuffer.SetIndexBuffer(indexBuffer);
            }
        }

        /// <summary>
        /// ジオメトリ更新処理
        /// </summary>
        /// <param name="sender">ジオメトリ</param>
        /// <param name="e">イベント</param>
        private void OnPolygonUpdated(object sender, UpdatePolygonEventArgs e)
        {
            SetupRenderPackage(e.Type,e.Color);
        }
    }
}
