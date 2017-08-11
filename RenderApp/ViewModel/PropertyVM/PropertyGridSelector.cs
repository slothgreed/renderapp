using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using KI.UI.ViewModel;

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

        public DataTemplate CheckBoxTemplate { get; set; }

        public DataTemplate ComboItemTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is KeyValuePair<string, object>)
            {
                var key = (KeyValuePair<string, object>)item;

                if (key.Value is Vector2ViewModel)
                    return Vector2Template;
                if (key.Value is Vector3ViewModel)
                    return Vector3Template;
                if (key.Value is Vector4ViewModel)
                    return Vector4Template;
                if (key.Value is Matrix3ViewModel)
                    return Matrix3Template;
                if (key.Value is Matrix4ViewModel)
                    return Matrix4Template;
                if (key.Value is CheckBoxViewModel)
                    return CheckBoxTemplate;
                if (key.Value is ComboItemViewModel)
                    return ComboItemTemplate;
            }

            return DefaultTemplate;
        }
    }
}
