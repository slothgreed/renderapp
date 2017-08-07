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
        public static SceneManager SceneManager { get; } = new SceneManager();

        /// <summary>
        /// レンダーシステム
        /// </summary>
        public static RenderSystem RenderSystem { get; } = new RenderSystem();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static Workspace()
        {
            Global.RenderSystem = RenderSystem;
            Global.RenderSystem.ActiveScene = SceneManager.ActiveScene;
        }
    }
}
