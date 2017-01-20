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
    /// PropertyAttribute
    /// </summary>
    public class PropertyAttribute
    {
        public PropertyAttribute(string key, object value)
        {
            Key = key;
            Value = value;
        }
        public string Key { get; set; }
        public object Value { get; set; }
    }

    /// <summary>
    /// PropertyGridView.xaml の相互作用ロジック
    /// </summary>
    public partial class PropertyGridView : UserControl
    {
        public PropertyGridView()
        {
            InitializeComponent();
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

        public List<PropertyAttribute> Attributes
        {
            get;
            set;
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

            if (Attributes == null && PropertyItem != null)
            {
                Attributes = new List<PropertyAttribute>();
                foreach (KeyValuePair<string, object> loop in PropertyItem)
                {
                    if (loop.Value is GLUtil.Texture)
                    {
                        Attributes.Add(new PropertyAttribute(loop.Key, new ImageViewModel(loop.Key, (GLUtil.Texture)loop.Value)));
                    }
                    else if (loop.Value is OpenTK.Vector2)
                    {
                        Attributes.Add(new PropertyAttribute(loop.Key, new Vector2ViewModel(loop.Key, (OpenTK.Vector2)loop.Value)));
                    }
                    else if (loop.Value is OpenTK.Vector3)
                    {
                        Attributes.Add(new PropertyAttribute(loop.Key, new Vector3ViewModel(loop.Key,this.DataContext,(OpenTK.Vector3)loop.Value)));
                    }
                    else if (loop.Value is OpenTK.Vector4)
                    {
                        Attributes.Add(new PropertyAttribute(loop.Key, new Vector4ViewModel(loop.Key, (OpenTK.Vector4)loop.Value)));
                    }
                    else if (loop.Value is OpenTK.Matrix3)
                    {
                        Attributes.Add(new PropertyAttribute(loop.Key, new Matrix3ViewModel(loop.Key, (OpenTK.Matrix3)loop.Value)));

                    }
                    else if (loop.Value is OpenTK.Matrix4)
                    {
                        Attributes.Add(new PropertyAttribute(loop.Key, new Matrix4ViewModel(loop.Key, (OpenTK.Matrix4)loop.Value)));
                    }
                    else if (loop.Value is NumericViewModel)
                    {
                        Attributes.Add(new PropertyAttribute(loop.Key, new NumericViewModel(loop.Key, (float)loop.Value)));
                    }
                    else
                    {
                        Attributes.Add(new PropertyAttribute(loop.Key, new DefaultViewModel(loop.Key, loop.Value.ToString())));
                    }
                }
                this.PropertyGrid.ItemsSource = Attributes;

            }
        }

    }
}
