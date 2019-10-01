using KI.Foundation.Core;
using KI.Gfx;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil.Buffer;

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
        public PolygonNode(Polygon polygon)
            : base(polygon.Name)
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
        public VertexBuffer VertexBuffer { get; private set; }

        /// <summary>
        /// 形状
        /// </summary>
        public Polygon Polygon
        {
            get;
            private set;
        }

        /// <summary>
        /// ポリゴンの種類
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
        public override void RenderCore(Scene scene, RenderInfo renderInfo)
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
            VertexBuffer.Render();

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

            UpdateVertexBufferObject();
        }

        /// <summary>
        /// ジオメトリ更新処理
        /// </summary>
        public virtual void UpdateVertexBufferObject()
        {
            VertexBuffer.SetBuffer(Polygon.Type, Polygon.Vertexs, Polygon.Index);
        }
    }
}
