using System.Linq;
using KI.Asset;
using KI.Asset.Attribute;
using KI.Asset.Technique;
using KI.Foundation.Tree;
using OpenTK;

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
        public Renderer Renderer { get; set; }

        public void Initialize()
        {
            MainScene = new Scene("MainScene");
            Renderer = new Renderer();
            Renderer.ActiveScene = MainScene;
        }
    }
}
