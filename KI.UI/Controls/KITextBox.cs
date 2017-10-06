using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KI.UI.Controls
{
    public class KITextBox : TextBox
    {
        public KITextBox()
        {
            this.MinWidth = 80;

            this.TextAlignment = TextAlignment.Right;
        }

        /// <summary>
        /// フォーカスが得られたとき
        /// </summary>
        /// <param name="e">イベント</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            this.SelectAll();
        }

        /// <summary>
        /// マウス押下のとき
        /// </summary>
        /// <param name="e">イベント</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }
    }
}
