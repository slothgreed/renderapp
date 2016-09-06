using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace RenderApp.Utility
{
    /// <summary>
    /// マウスのクラス(右左ホイールは各インスタンス生成すること)
    /// </summary>
    public class Mouse
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
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Mouse()
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
        public void Drag(int x, int y)
        {
            Before = Current;
            Current = new Vector2(x, y);
         }


        /// <summary>
        /// 移動量を算出
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2 Move(int x,int y)
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

            return move;
        }

        
    }
}
