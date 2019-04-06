namespace KI.Renderer
{
    /// <summary>
    /// 空ノード
    /// </summary>
    public class EmptyNode : SceneNode
    {
        /// <summary>
        /// 空ノード
        /// </summary>
        /// <param name="name">名前</param>
        public EmptyNode(string name)
            : base(name)
        {

        }

        /// <summary>
        /// 何もレンダリングしない
        /// </summary>
        /// <param name="scene">シーン</param>
        public override void RenderCore(Scene scene)
        {
            return;
        }
    }
}
