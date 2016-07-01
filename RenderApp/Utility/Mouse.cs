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
        private Vector2 click;
        /// <summary>
        /// 前回のクリック位置
        /// </summary>
        private Vector2 clickbefore;
        /// <summary>
        /// 現在のマウス位置
        /// </summary>
        private Vector2 current;
        /// <summary>
        /// 前回のマウス位置
        /// </summary>
        private Vector2 before;
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
            before = current;
            current = new Vector2(x,y);

            clickbefore = click;
            click = current;


        }
        /// <summary>
        /// ドラッグ値
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Drag(int x, int y)
        {
            before = current;
            current = new Vector2(x, y);
         }


        /// <summary>
        /// 移動量を算出
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2 Move(int x,int y)
        {
            Vector2 move;

            if (before.X == 0 && before.Y == 0)
            {
                move = new Vector2(0, 0);
            }
            else
            {
                move = new Vector2(x - before.X, before.Y - y);
                move.X /= 10.0f;
                move.Y /= 10.0f;
            }

            return move;
        }
        #region [getter]

        /// <summary>
        /// 前フレームの位置
        /// </summary>
        /// <returns></returns>
        public Vector2 GetBefore()
        {
            return before;
        }
        /// <summary>
        /// 前回クリックした位置
        /// </summary>
        /// <returns></returns>
        public Vector2 GetClickBefore()
        {
            return clickbefore;
        }
        /// <summary>
        /// クリックした位置
        /// </summary>
        /// <returns></returns>
        public Vector2 GetClick()
        {
            return click;
        }
        /// <summary>
        /// 現在の位置
        /// </summary>
        /// <returns></returns>
        public Vector2 GetCurrent()
        {
            return current;
        }

        #endregion  

        
    }
}
