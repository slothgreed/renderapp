using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
namespace RenderApp.ViewModel
{
    class PropertyGridTemplate : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate Vector2Template { get; set; }
        public DataTemplate Vector3Template { get; set; }
        public DataTemplate Vector4Template { get; set; }
        public DataTemplate Matrix3Template { get; set; }
        public DataTemplate Matrix4Template { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if(item is PropertyControl)
            {
                PropertyControl control = item as PropertyControl;
                if(control.Type == "Vector2")
                {
                    return Vector2Template;
                }
                if (control.Type == "Vector3")
                {
                    return Vector3Template;
                }
                if (control.Type == "Vector4")
                {
                    return Vector4Template;
                }
                if (control.Type == "Matrix3")
                {
                    return Matrix3Template;
                }
                if (control.Type == "Matrix4")
                {
                    return Matrix4Template;
                }
            }
            return DefaultTemplate;

        }
    }
}
