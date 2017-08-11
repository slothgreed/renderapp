using KI.Renderer;

namespace RenderApp.Globals
{
    /// <summary>
    /// ワークスペース
    /// </summary>
    public static class Workspace
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        static Workspace()
        {
            RenderSystem.ActiveScene = MainScene;
            Global.RenderSystem = RenderSystem;
        }

        /// <summary>
        /// シーンマネージャ
        /// </summary>
        public static IScene MainScene { get; } = new MainScene();

        /// <summary>
        /// レンダーシステム
        /// </summary>
        public static IRenderer RenderSystem { get; } = new RenderSystem();

    }
}
