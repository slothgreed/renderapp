using System.Windows;

namespace KI.UI.Control
{
    public class ThumbnailFile : System.Windows.Controls.Control
    {
        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string), typeof(ThumbnailFile), new PropertyMetadata(string.Empty));

        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        public static readonly DependencyProperty ImagePathProperty =
            DependencyProperty.Register("ImagePath", typeof(string), typeof(ThumbnailFile), new PropertyMetadata(string.Empty));

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
            //ImagePath = @"C:\Users\ido\Documents\KIProject\renderapp\RenderApp\Resource\Texture\floor.bmp";
        }
    }
}
