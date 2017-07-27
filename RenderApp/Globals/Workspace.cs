using KI.Renderer;

namespace RenderApp.Globals
{
    public static class Workspace
    {
        public static SceneManager SceneManager = new SceneManager();
        public static RenderSystem RenderSystem = new RenderSystem();

        static Workspace()
        {
            Global.Scene = SceneManager.ActiveScene;
            Global.RenderSystem = RenderSystem;
        }
    }
}
