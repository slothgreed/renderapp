using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KI.Foundation.Control
{
    public class ThumbnailFile : System.Windows.Controls.Control
    {
        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string), typeof(ThumbnailFile), new PropertyMetadata(""));

        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        public static readonly DependencyProperty ImagePathProperty = 
            DependencyProperty.Register("ImagePath",typeof(string),typeof(ThumbnailFile),new PropertyMetadata(""));

        public string ImagePath
        {
            get { return (string)GetValue(ImagePathProperty); }
            set { SetValue(ImagePathProperty, value); }
        }
    
        static ThumbnailFile()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ThumbnailFile), new FrameworkPropertyMetadata(typeof(ThumbnailFile)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
