using System;
using System.Collections.Generic;
using System.Linq;
using KI.Asset.Attribute;
using KI.Foundation.Core;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Gfx.GLUtil.Buffer;
using KI.Gfx.KIShader;
using OpenTK.Graphics.OpenGL;

namespace KI.Renderer
{
    /// <summary>
    /// 任意形状(triangle,quad,line,patchのみ対応)
    /// </summary>
    public class PolygonNode : SceneNode
    {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="polygon">形状</param>
        /// <param name="shader">シェーダ</param>
        public PolygonNode(string name, Polygon polygon, Shader shader)
            : base(name)
        {
            SetPolygon(polygon, shader);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        public PolygonNode(string name)
            : base(name)
        {

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
        /// アトリビュートのみSRTが違うことはないためAttributeNodeを作成して子ノードにすることはしない。
        /// </summary>
        public List<AttributeBase> Attributes { get; private set; } = new List<AttributeBase>();

        /// <summary>
        /// 頂点バッファ
        /// </summary>
        public VertexBuffer VertexBuffer { get; set; }

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
                polygon = value;
            }
        }

        /// <summary>
        /// ポリゴンの種類(いずれPolygon.Typeを消して PolygonNode がTypeを持つようにする)
        /// </summary>
        public PolygonType Type
        {
            get
            {
                return Polygon.Type;
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

            if (Type == PolygonType.Points)
            {
                VertexBuffer.SetupPointBuffer(polygon.Vertexs, polygon.Index);
            }
            else if (Type == PolygonType.Lines)
            {
                VertexBuffer.SetupLineBuffer(polygon.Vertexs, polygon.Index, polygon.Lines);
            }
            else
            {
                VertexBuffer.SetupMeshBuffer(polygon.Vertexs, polygon.Index, polygon.Meshs, Type);
            }

            PolygonAttribute = new PolygonAttribute("Attribute:" + Name, VertexBuffer.ShallowCopy(), Type, shader);
            Attributes.Add(PolygonAttribute);
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public override void Dispose()
        {
            foreach (var attribute in Attributes)
            {
                attribute.Dispose();
            }

            base.Dispose();
        }

        /// <summary>
        /// ジオメトリ更新処理
        /// </summary>
        public void UpdateVertexBufferObject()
        {
            if (Type == PolygonType.Points)
            {
                VertexBuffer.SetupPointBuffer(polygon.Vertexs, polygon.Index);
            }
            else if (Type == PolygonType.Lines)
            {
                VertexBuffer.SetupLineBuffer(polygon.Vertexs, polygon.Index, polygon.Lines);
            }
            else
            {
                VertexBuffer.SetupMeshBuffer(polygon.Vertexs, polygon.Index, polygon.Meshs, Type);
            }

            PolygonAttribute.UpdateVertexBuffer(VertexBuffer);
        }
    }
}
