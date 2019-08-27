using KI.Renderer;

namespace RenderApp.Model
{
    /// <summary>
    /// ワークスペース
    /// </summary>
    public class Workspace
    {
        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        private static Workspace instance;

        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        public static Workspace Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Workspace();
                    instance.Initialize();
                }

                return instance;
            }
        }

        /// <summary>
        /// シーン
        /// </summary>
        public Scene MainScene { get; set; }

        /// <summary>
        /// レンダラー
        /// </summary>
        public RenderSystem RenderSystem { get; set; }

        public void Initialize()
        {
            MainScene = new Scene("MainScene", new EmptyNode("Root"));
            RenderSystem = new RenderSystem();
            RenderSystem.ActiveScene = MainScene;
        }
    }
}
