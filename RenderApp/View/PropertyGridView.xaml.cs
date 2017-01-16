using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RenderApp.ViewModel;
namespace RenderApp.View
{
    /// <summary>
    /// PropertyGridView.xaml の相互作用ロジック
    /// </summary>
    public partial class PropertyGridView : UserControl
    {
        public PropertyGridView()
        {
            //InitializeComponent();
        }

        #region [Binding Item]
        public static readonly DependencyProperty PropertyItemProperty =
            DependencyProperty.Register("PropertyItem", typeof(Dictionary<string,object>), typeof(PropertyGridView),
            new FrameworkPropertyMetadata(null));
        public Dictionary<string, object> PropertyItem
        {
            get
            {
                return (Dictionary<string, object>)GetValue(PropertyItemProperty);
            }
            set
            {
                SetValue(PropertyItemProperty, value);
            }
        }
        #endregion
        public static readonly DependencyProperty DynamicObjectProperty =
            DependencyProperty.Register("DynamicObject", typeof(Dictionary<string, object>), typeof(PropertyGridView),
            new FrameworkPropertyMetadata(null));
        public Dictionary<string, object> DynamicObject
        {
            get
            {
                return (Dictionary<string, object>)GetValue(PropertyItemProperty);
            }
            private set
            {
                SetValue(DynamicObjectProperty, value);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

        }
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if(DynamicObject == null && PropertyItem != null)
            {
                DynamicObject = new Dictionary<string, object>();
                foreach (KeyValuePair<string, object> loop in PropertyItem)
                {
                    if (loop.Value is GLUtil.Texture)
                    {
                        DynamicObject.Add(loop.Key, new ImageViewModel(loop.Key, (GLUtil.Texture)loop.Value));
                    }
                    else if (loop.Value is OpenTK.Vector2)
                    {
                        DynamicObject.Add(loop.Key + "X", ((OpenTK.Vector2)loop.Value).X);
                        DynamicObject.Add(loop.Key + "Y", ((OpenTK.Vector2)loop.Value).Y);
                    }
                    else if (loop.Value is OpenTK.Vector3)
                    {
                        DynamicObject.Add(loop.Key + "X", ((OpenTK.Vector3)loop.Value).X);
                        DynamicObject.Add(loop.Key + "Y", ((OpenTK.Vector3)loop.Value).Y);
                        DynamicObject.Add(loop.Key + "Z", ((OpenTK.Vector3)loop.Value).Z);
                    }
                    else if (loop.Value is OpenTK.Vector4)
                    {
                        DynamicObject.Add(loop.Key + "X", ((OpenTK.Vector4)loop.Value).X);
                        DynamicObject.Add(loop.Key + "Y", ((OpenTK.Vector4)loop.Value).Y);
                        DynamicObject.Add(loop.Key + "Z", ((OpenTK.Vector4)loop.Value).Z);
                        DynamicObject.Add(loop.Key + "W", ((OpenTK.Vector4)loop.Value).W);

                    }
                    else if (loop.Value is OpenTK.Matrix3)
                    {
                        DynamicObject.Add(loop.Key, new Matrix3ViewModel(loop.Key, (OpenTK.Matrix3)loop.Value));

                    }
                    else if (loop.Value is OpenTK.Matrix4)
                    {
                        DynamicObject.Add(loop.Key, new Matrix4ViewModel(loop.Key, (OpenTK.Matrix4)loop.Value));
                    }
                    else if (loop.Value is NumericViewModel)
                    {
                        DynamicObject.Add(loop.Key, new NumericViewModel(loop.Key, (float)loop.Value));
                    }
                    else
                    {
                        DynamicObject.Add(loop.Key, new DefaultViewModel(loop.Key, loop.Value.ToString()));
                    }
                }
            }
        }
    }
}
