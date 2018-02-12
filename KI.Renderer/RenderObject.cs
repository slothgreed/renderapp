using System.Collections.Generic;
using System.Linq;
using KI.Foundation.Utility;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using KI.Asset.Attribute;
using OpenTK.Graphics.OpenGL;

namespace KI.Asset
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
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="polygon">形状</param>
        public RenderObject(string name, Polygon polygon, Shader shader)
            : base(name)
        {
            SetPolygon(polygon, shader);
        }

        /// <summary>
        /// ジオメトリステージのアトリビュート
        /// </summary>
        public AttributeBase PolygonAttribute { get; set; }

        /// <summary>
        /// 形状ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// アトリビュート
        /// </summary>
        public List<AttributeBase> Attributes { get; private set; } = new List<AttributeBase>();

        /// <summary>
        /// 頂点バッファ
        /// </summary>
        public VertexBuffer VertexBuffer { get; set; }

        /// <summary>
        /// 可視不可視
        /// </summary>
        public new bool Visible
        {
            get
            {
                return PolygonAttribute.Visible;
            }
            set
            {
                PolygonAttribute.Visible = value;
            }
        }

        /// <summary>
        /// シェーダ
        /// </summary>
        public Shader Shader
        {
            get
            {
                return PolygonAttribute.Shader;
            }

            set
            {
                PolygonAttribute.Shader = value;
            }
        }

        /// <summary>
        /// 形状データ
        /// </summary>
        private Polygon polygon;

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
            foreach (var attribute in Attributes.Where(p => p.Visible))
            {
                if (attribute.Shader == null)
                {
                    Logger.Log(Logger.LogLevel.Error, "not set shader");
                    return;
                }

                if (attribute.VertexBuffer.Num == 0)
                {
                    Logger.Log(Logger.LogLevel.Error, "vertexs list is 0");
                    return;
                }

                attribute.Binding();
                ShaderHelper.InitializeState(attribute.Shader, scene, this, attribute.VertexBuffer, Polygon.Textures);
                attribute.Shader.BindBuffer();
                if (attribute.VertexBuffer.EnableIndexBuffer)
                {
                    DeviceContext.Instance.DrawElements(attribute.Type, attribute.VertexBuffer.Num, DrawElementsType.UnsignedInt, 0);
                }
                else
                {
                    DeviceContext.Instance.DrawArrays(attribute.Type, 0, attribute.VertexBuffer.Num);
                }

                attribute.Shader.UnBindBuffer();
                attribute.UnBinding();

                Logger.GLLog(Logger.LogLevel.Error);
            }
        }

        /// <summary>
        /// 形状をセット
        /// </summary>
        /// <param name="polygon">形状情報</param>
        private void SetPolygon(Polygon polygon, Shader shader)
        {
            Polygon = polygon;
            VertexBuffer = new VertexBuffer();
            VertexBuffer.SetupBuffer(Polygon);

            PolygonAttribute = new PolygonAttribute("Attribute:" + Name, VertexBuffer.ShallowCopy(), Polygon.Type, shader);
            Attributes.Add(PolygonAttribute);
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
            VertexBuffer.SetupBuffer(Polygon);
        }
    }
}
