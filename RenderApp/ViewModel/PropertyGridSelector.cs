using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using RenderApp.View;
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
            if (item is PropertyAttribute)
            {
                PropertyAttribute control = item as PropertyAttribute;
                if(control.Value is Vector2ViewModel)
                {
                    return Vector2Template;
                }
                if (control.Value is Vector3ViewModel)
                {
                    return Vector3Template;
                }
                if (control.Value is Vector4ViewModel)
                {
                    return Vector4Template;
                }
                if (control.Value is Matrix3ViewModel)
                {
                    return Matrix3Template;
                }
                if (control.Value is Matrix4ViewModel)
                {
                    return Matrix4Template;
                }
            }
            return DefaultTemplate;

        }
    }
}
