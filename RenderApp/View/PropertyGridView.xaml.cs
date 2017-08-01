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
using KI.Foundation.View;

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
            DependencyProperty.Register("PropertyItem", typeof(Dictionary<string, object>), typeof(PropertyGridView),
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
        }

    }
}
