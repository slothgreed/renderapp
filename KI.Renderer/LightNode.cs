namespace KI.Renderer
{
    /// <summary>
    /// ライトノード
    /// </summary>
    public class LightNode : SceneNode
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">名前</param>
        /// <param name="light">ライト</param>
        /// <param name="model">ライトのモデル・NULLOK</param>
        public LightNode(string name, Light light, PolygonNode modelNode)
            : base(name)
        {
            Data = light;
            Model = modelNode;
            Model.Translate = Data.Position;
        }

        /// <summary>
        /// ライト情報
        /// </summary>
        public Light Data
        {
            get;
            private set;
        }

        /// <summary>
        /// ライトのモデルノード
        /// </summary>
        public PolygonNode Model
        {
            get;
            private set;
        }


        /// <summary>
        /// ライトモデルがあれば描画
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void RenderCore(Scene scene)
        {
            if (Model != null)
            {
                Model.Render(scene);
            }
        }
    }
}
