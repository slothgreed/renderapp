using KI.Renderer;

namespace RenderApp.Globals
{
    /// <summary>
    /// ワークスペース
    /// </summary>
    public static class Workspace
    {
        /// <summary>
        /// シーンマネージャ
        /// </summary>
        public static SceneManager SceneManager = new SceneManager();

        /// <summary>
        /// レンダーシステム
        /// </summary>
        public static RenderSystem RenderSystem = new RenderSystem();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static Workspace()
        {
            Global.Scene = SceneManager.ActiveScene;
            Global.RenderSystem = RenderSystem;
        }
    }
}
