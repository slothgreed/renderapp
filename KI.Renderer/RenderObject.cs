using System.Collections.Generic;
using System.Linq;
using KI.Asset;
using KI.Foundation.Utility;
using KI.Gfx.Geometry;
using KI.Gfx.GLUtil;
using KI.Gfx.KIShader;
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
            SetupRenderPackage(polygon.Type);

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
        public void SetupRenderPackage(PrimitiveType type)
        {
            if (!Packages.ContainsKey(type))
            {
                Packages[type] = new RenderPackage(Polygon, type);
                Packages[type].VertexBuffer.GenBuffer(polygon, type);
                string vert = ShaderCreater.Instance.GetVertexShader(this);
                string frag = ShaderCreater.Instance.GetFragShader(this);
                Packages[type].Shader = ShaderFactory.Instance.CreateShaderVF(vert, frag);
            }

            Packages[type].VertexBuffer.SetupBuffer(Polygon, type);
            if (Packages[type].Color != null)
            {
                Packages[type].VertexBuffer.ColorList = Packages[type].Color;
            }
        }

        /// <summary>
        /// ジオメトリ更新処理
        /// </summary>
        /// <param name="sender">ジオメトリ</param>
        /// <param name="e">イベント</param>
        private void OnPolygonUpdated(object sender, UpdatePolygonEventArgs e)
        {
            SetupRenderPackage(e.Type);
            if (e.Color != null)
            {
                Packages[e.Type].Color = e.Color;
                Packages[e.Type].VertexBuffer.ColorList = e.Color;
            }

            RenderMode = RenderMode.PolygonLine;
        }
    }
}
