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
        public PolygonNode(string name, Polygon polygon)
            : base(name)
        {
            SetPolygon(polygon);
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
        /// 頂点バッファ
        /// </summary>
        public VertexBuffer VertexBuffer { get; set; }

        /// <summary>
        /// 形状
        /// </summary>
        public Polygon Polygon
        {
            get;
            private set;
        }

        /// <summary>
        /// ポリゴンの種類(いずれPolygon.Typeを消して PolygonNode がTypeを持つようにする)
        /// </summary>
        public KIPrimitiveType Type
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
            if (Polygon.Material.Shader == null)
            {
                Logger.Log(Logger.LogLevel.Error, "not set shader");
                return;
            }

            if (VertexBuffer.Num == 0)
            {
                Logger.Log(Logger.LogLevel.Error, "vertexs list is 0");
                return;
            }

            ShaderHelper.InitializeState(scene, this, VertexBuffer, Polygon.Material);
            Polygon.Material.BindToGPU();
            if (VertexBuffer.EnableIndexBuffer)
            {
                DeviceContext.Instance.DrawElements(Type, VertexBuffer.Num, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                DeviceContext.Instance.DrawArrays(Type, 0, VertexBuffer.Num);
            }

            Polygon.Material.UnBindToGPU();

            Logger.GLLog(Logger.LogLevel.Error);
        }

        /// <summary>
        /// 形状をセット
        /// </summary>
        /// <param name="polygon">形状情報</param>
        public void SetPolygon(Polygon polygon)
        {
            Polygon = polygon;

            if(VertexBuffer != null)
            {
                VertexBuffer.Dispose();
            }

            VertexBuffer = new VertexBuffer();

            if (Type == KIPrimitiveType.Points)
            {
                VertexBuffer.SetupPointBuffer(polygon.Vertexs, polygon.Index);
            }
            else if (Type == KIPrimitiveType.Lines)
            {
                VertexBuffer.SetupLineBuffer(polygon.Vertexs, polygon.Index, polygon.Lines);
            }
            else
            {
                VertexBuffer.SetupMeshBuffer(polygon.Vertexs, polygon.Index, polygon.Meshs, Type);
            }
        }

        /// <summary>
        /// ジオメトリ更新処理
        /// </summary>
        public void UpdateVertexBufferObject()
        {
            if (Type == KIPrimitiveType.Points)
            {
                VertexBuffer.SetupPointBuffer(Polygon.Vertexs, Polygon.Index);
            }
            else if (Type == KIPrimitiveType.Lines)
            {
                VertexBuffer.SetupLineBuffer(Polygon.Vertexs, Polygon.Index, Polygon.Lines);
            }
            else
            {
                VertexBuffer.SetupMeshBuffer(Polygon.Vertexs, Polygon.Index, Polygon.Meshs, Type);
            }
        }
    }
}
