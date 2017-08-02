using OpenTK;

namespace KI.Foundation.Core
{
    /// <summary>
    /// マウスのクラス(右左ホイールは各インスタンス生成すること)
    /// </summary>
    public class KIMouse
    {
        #region [メンバ変数]
        /// <summary>
        /// クリック位置
        /// </summary>
        public Vector2 Click
        {
            get;
            private set;
        }
        /// <summary>
        /// 前回のクリック位置
        /// </summary>
        public Vector2 ClickBefore
        {
            get;
            private set;
        }
        /// <summary>
        /// 現在のマウス位置
        /// </summary>
        public Vector2 Current
        {
            get;
            private set;
        }
        /// <summary>
        /// 前回のマウス位置
        /// </summary>
        public Vector2 Before
        {
            get;
            private set;
        }
        public Vector2 Delta
        {
            get;
            private set;
        }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public KIMouse()
        {
        }

        /// <summary>
        /// クリック値
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Down(int x, int y)
        {
            Before = Current;
            Current = new Vector2(x, y);

            ClickBefore = Click;
            Click = Current;
        }

        /// <summary>
        /// ドラッグ値
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Move(int x, int y)
        {
            Before = Current;
            Current = new Vector2(x, y);
            Drag(x, y);
        }

        public void Up(int x, int y)
        {
            Before = Vector2.Zero;
            Current = Vector2.Zero;
        }

        /// <summary>
        /// 移動量を算出
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private Vector2 Drag(int x, int y)
        {
            Vector2 move;

            if (Before.X == 0 && Before.Y == 0)
            {
                move = new Vector2(0, 0);
            }
            else
            {
                move = new Vector2(x - Before.X, Before.Y - y);
                move.X /= 10.0f;
                move.Y /= 10.0f;
            }

            Delta = move;
            return move;
        }
    }
}
