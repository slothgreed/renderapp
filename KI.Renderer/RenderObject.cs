using System.Collections.Generic;
using System.Linq;
using KI.Asset;
using KI.Foundation.Utility;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using KI.Renderer.Material;
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
            SetPolygon(polygon, null);
        }

        /// <summary>
        /// PolygonMaterial
        /// </summary>
        public GeometryMaterial PolygonMaterial { get; private set; }

        /// <summary>
        /// 形状ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// マテリアル
        /// </summary>
        public List<MaterialBase> Materials { get; private set; } = new List<MaterialBase>();

        /// <summary>
        /// メインのシェーダ
        /// </summary>
        public Shader Shader
        {
            get
            {
                return PolygonMaterial.Shader;
            }

            set
            {
                PolygonMaterial.Shader = value;
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
        /// 描画
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void RenderCore(IScene scene)
        {
            foreach (var material in Materials.Where(p => p.Visible))
            {
                if (material.Shader == null)
                {
                    Logger.Log(Logger.LogLevel.Error, "not set shader");
                    return;
                }

                if (material.VertexBuffer.Num == 0)
                {
                    Logger.Log(Logger.LogLevel.Error, "vertexs list is 0");
                    return;
                }

                material.Binding();

                ShaderHelper.InitializeState(scene, this, material.VertexBuffer, material.Shader, Polygon.Textures);
                material.Shader.BindBuffer();
                if (material.VertexBuffer.EnableIndexBuffer)
                {
                    DeviceContext.Instance.DrawElements(material.Type, material.VertexBuffer.Num, DrawElementsType.UnsignedInt, 0);
                }
                else
                {
                    DeviceContext.Instance.DrawArrays(material.Type, 0, material.VertexBuffer.Num);
                }

                material.Shader.UnBindBuffer();

                material.UnBinding();
                Logger.GLLog(Logger.LogLevel.Error);
            }
        }

        /// <summary>
        /// 形状をセット
        /// </summary>
        /// <param name="polygon">形状情報</param>
        public void SetPolygon(Polygon polygon, MaterialBase material = null)
        {
            Polygon = polygon;

            if (material == null)
            {
                string vert = ShaderCreater.Instance.GetVertexShader(this);
                string frag = ShaderCreater.Instance.GetFragShader(this);
                var shader = ShaderFactory.Instance.CreateShaderVF(vert, frag);
                material = new GeometryMaterial("Material:" + Name, polygon, shader);
            }

            if (PolygonMaterial == null)
            {
                PolygonMaterial = material as GeometryMaterial;
            }

            if (!Materials.Contains(material))
            {
                Materials.Add(material);
            }

            UpdateMaterial(material, null);
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public override void Dispose()
        {
            foreach (var material in Materials)
            {
                PolygonMaterial.VertexBuffer.Dispose();
            }
        }

        /// <summary>
        /// マテリアルの更新
        /// </summary>
        /// <param name="type">形状種類</param>
        /// <param name="color">色</param>
        public void UpdateMaterial(MaterialBase material, List<Vector3> color)
        {
            //if (color != null)
            //{
            //    material.Color = color;
            //}

            SetupBuffer(material);
        }

        /// <summary>
        /// バッファにデータの設定
        /// </summary>
        /// <param name="polygon">ポリゴン</param>
        /// <param name="type">形状種類</param>
        /// <param name="color">頂点カラーの設定</param>
        public void SetupBuffer(MaterialBase material)
        {
            int[] indexBuffer = null;
            Vector3[] position = null;
            Vector3[] normal = null;
            Vector3[] color = null;
            Vector2[] texCoord = null;
            if (polygon.Index.Count != 0)
            {
                indexBuffer = polygon.Index.ToArray();
                position = polygon.Vertexs.Select(p => p.Position).ToArray();
                normal = polygon.Vertexs.Select(p => p.Normal).ToArray();
                color = polygon.Vertexs.Select(p => p.Color).ToArray();
                texCoord = polygon.Vertexs.Select(p => p.TexCoord).ToArray();
            }
            else if (material.Type == PrimitiveType.Points)
            {
                position = polygon.Vertexs.Select(p => p.Position).ToArray();
                normal = polygon.Vertexs.Select(p => p.Normal).ToArray();
                color = polygon.Vertexs.Select(p => p.Color).ToArray();
                texCoord = polygon.Vertexs.Select(p => p.TexCoord).ToArray();
            }
            else if (material.Type == PrimitiveType.Lines)
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

                if (material.Type == PrimitiveType.Triangles)
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
            //if(material.Color != null)
            //{
            //    color = material.Color.ToArray();
            //}

            material.VertexBuffer.SetBuffer(position, normal, color, texCoord);

            if(indexBuffer != null)
            {
                material.VertexBuffer.SetIndexBuffer(indexBuffer);
            }
        }

        /// <summary>
        /// ジオメトリ更新処理
        /// </summary>
        /// <param name="sender">ジオメトリ</param>
        /// <param name="e">イベント</param>
        private void OnPolygonUpdated(object sender, UpdatePolygonEventArgs e)
        {
            foreach (var material in Materials.Where(p => p.Type == e.Type))
            {
                UpdateMaterial(material, e.Color);
            }
        }
    }
}
