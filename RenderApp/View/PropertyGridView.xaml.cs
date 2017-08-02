using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
        public static readonly DependencyProperty PropertyItemProperty =
    DependencyProperty.Register("PropertyItem", typeof(Dictionary<string, object>), typeof(PropertyGridView),
    new FrameworkPropertyMetadata(null));

        public PropertyGridView()
        {
            InitializeComponent();
        }

        #region [Binding Item]

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
