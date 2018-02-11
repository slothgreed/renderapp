using System.Collections.Generic;
using System.Linq;
using KI.Asset;
using KI.Foundation.Utility;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using KI.Renderer.Attribute;
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
        /// ジオメトリステージのアトリビュート
        /// </summary>
        public GeometryAttribute GeometryAttribute { get; private set; }

        /// <summary>
        /// 形状ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// アトリビュート
        /// </summary>
        public List<AttributeBase> Attributes { get; private set; } = new List<AttributeBase>();


        /// <summary>
        /// 可視不可視
        /// </summary>
        public new bool Visible
        {
            get
            {
                return GeometryAttribute.Visible;
            }
            set
            {
                GeometryAttribute.Visible = value;
            }
        }

        /// <summary>
        /// シェーダ
        /// </summary>
        public Shader Shader
        {
            get
            {
                return GeometryAttribute.Shader;
            }

            set
            {
                GeometryAttribute.Shader = value;
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
        public override void RenderCore(Scene scene)
        {
            foreach (var material in Attributes.Where(p => p.Visible))
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
        public void SetPolygon(Polygon polygon, AttributeBase material = null)
        {
            Polygon = polygon;

            if (material == null)
            {
                string vert = ShaderCreater.Instance.GetVertexShader(this);
                string frag = ShaderCreater.Instance.GetFragShader(this);
                var shader = ShaderFactory.Instance.CreateShaderVF(vert, frag, ShaderStage.Geometry);
                material = new GeometryAttribute("Attribute:" + Name, polygon, shader);
            }

            if (GeometryAttribute == null)
            {
                GeometryAttribute = material as GeometryAttribute;
            }

            if (!Attributes.Contains(material))
            {
                Attributes.Add(material);
            }

        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public override void Dispose()
        {
            foreach (var material in Attributes)
            {
                material.Dispose();
            }
        }

        /// <summary>
        /// ジオメトリ更新処理
        /// </summary>
        /// <param name="sender">ジオメトリ</param>
        /// <param name="e">イベント</param>
        private void OnPolygonUpdated(object sender, UpdatePolygonEventArgs e)
        {
            GeometryAttribute.SetupBuffer();
        }
    }
}
