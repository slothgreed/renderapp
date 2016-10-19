using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
namespace RenderApp.View
{
    public class PropertyGrid : ItemsControl
    {
        public PropertyGrid()
        {
            int a = 0;
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.DefaultStyleKey = typeof(PropertyGrid);
        }
    }
}
