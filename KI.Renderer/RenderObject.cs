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
        Point,
        Line,
        Polygon,
        PolygonLine,
    }

    /// <summary>
    /// レンダリングパッケージ
    /// </summary>
    public class RenderPackage
    {
        public RenderPackage(Polygon polygon, PrimitiveType type)
        {
            Type = type;
        }

        /// <summary>
        /// 頂点バッファ
        /// </summary>
        public VertexBuffer VertexBuffer { get; set; } = new VertexBuffer();

        /// <summary>
        /// シェーダ
        /// </summary>
        public Shader Shader { get; set; }

        /// <summary>
        /// レンダリングするときの種類
        /// </summary>
        public PrimitiveType Type { get; set; }
        
        /// <summary>
        /// テクスチャ座標
        /// </summary>
        public List<Vector3> Color { get; set; }
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
        public Dictionary<PrimitiveType, RenderPackage> Package { get; private set; } = new Dictionary<PrimitiveType, RenderPackage>();

        /// <summary>
        /// メインのシェーダ
        /// </summary>
        public Shader Shader
        {
            get
            {
                return Package[Polygon.Type].Shader;
            }

            set
            {
                Package[Polygon.Type].Shader = value;
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
                    polygon.UpdatePolygon -= UpdatePolygon;
                }

                polygon = value;
                polygon.UpdatePolygon += UpdatePolygon;
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
            SetupRenderPackage(polygon.Type);

            if (Polygon.Type == PrimitiveType.Points)
            {
                RenderMode = RenderMode.Point;
            }
            else
            if (Polygon.Type == PrimitiveType.Lines)
            {
                RenderMode = RenderMode.Line;
            }
            else
            if (Polygon.Type == PrimitiveType.Triangles ||
                Polygon.Type == PrimitiveType.Quads)
            {
                RenderMode = RenderMode.Polygon;
            }
        }

        /// <summary>
        /// 解放処理
        /// </summary>
        public override void Dispose()
        {
            foreach (var pack in Package.Values)
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
            if (Package.ContainsKey(type))
            {
                RenderPackageCore(scene, Package[type]);
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
        private void SetupRenderPackage(PrimitiveType type)
        {
            if (!Package.ContainsKey(type))
            {
                Package[type] = new RenderPackage(Polygon, type);
                Package[type].VertexBuffer.GenBuffer(polygon, type);
                string vert = ShaderCreater.Instance.GetVertexShader(this);
                string frag = ShaderCreater.Instance.GetFragShader(this);
                Package[type].Shader = ShaderFactory.Instance.CreateShaderVF(vert, frag);
            }

            Package[type].VertexBuffer.SetupBuffer(Polygon, type);
            if (Package[type].Color != null)
            {
                Package[type].VertexBuffer.ColorList = Package[type].Color;
            }
        }

        /// <summary>
        /// ジオメトリ更新処理
        /// </summary>
        /// <param name="sender">ジオメトリ</param>
        /// <param name="e">イベント</param>
        private void UpdatePolygon(object sender, UpdatePolygonEventArgs e)
        {
            SetupRenderPackage(e.Type);
            if (e.Color != null)
            {
                Package[e.Type].Color = e.Color;
                Package[e.Type].VertexBuffer.ColorList = e.Color;
            }

            RenderMode = RenderMode.PolygonLine;
        }

        /// <summary>
        /// レンダリング
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <param name="package">レンダリング情報</param>
        private void RenderPackageCore(IScene scene, RenderPackage package)
        {
            if (package.Shader == null)
            {
                Logger.Log(Logger.LogLevel.Error, "not set shader");
                return;
            }

            if (package.VertexBuffer.Num == 0)
            {
                Logger.Log(Logger.LogLevel.Error, "vertexs list is 0");
                return;
            }

            ShaderHelper.InitializeState(scene, this, package, Polygon.Textures);
            package.Shader.BindBuffer();
            if (package.VertexBuffer.IndexBuffer.ContainsKey(package.Type))
            {
                DeviceContext.Instance.DrawElements(package.Type, package.VertexBuffer.Num, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                DeviceContext.Instance.DrawArrays(package.Type, 0, package.VertexBuffer.Num);
            }

            package.Shader.UnBindBuffer();
            Logger.GLLog(Logger.LogLevel.Error);
        }
    }
}
