using KI.Asset;
using OpenTK;

namespace CADApp.Model
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
        /// 作業平面
        /// </summary>
        public Rectangle WorkPlane;

        /// <summary>
        /// シーン
        /// </summary>
        public Scene MainScene { get; set; }

        /// <summary>
        /// レンダラー
        /// </summary>
        public Renderer Renderer { get; set; }

        public void Initialize()
        {
            MainScene = new Scene("MainScene");
            Renderer = new Renderer();
            Renderer.ActiveScene = MainScene;
            WorkPlane = new Rectangle("WorkPlane", Vector3.Zero, Vector3.UnitX, Vector3.UnitX + Vector3.UnitZ, Vector3.UnitZ);
        }
    }
}
