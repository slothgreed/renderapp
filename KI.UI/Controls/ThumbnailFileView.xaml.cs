using System.Windows;
using System.Windows.Controls;

namespace KI.UI.Controls
{
    /// <summary>
    /// ThumbnailFileView.xaml の相互作用ロジック
    /// </summary>
    public partial class ThumbnailFileView : UserControl
    {
        public ThumbnailFileView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// サムネイル画像のプロパティ
        /// </summary>
        public static readonly DependencyProperty ImagePathProperty =
            DependencyProperty.Register(
                "ImagePath",
                typeof(string),
                typeof(ThumbnailFileView),
                new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(OnImagePathChanged)));

        /// <summary>
        /// 元ファイルのパスプロパティ
        /// </summary>
        public static readonly DependencyProperty OriginalFilePathProperty =
            DependencyProperty.Register(
                "OriginalFilePath",
                typeof(string),
                typeof(ThumbnailFileView),
                new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(OnOriginalFilePathChanged)));

        /// <summary>
        /// サムネイル画像のファイルパス
        /// </summary>
        public string ImagePath
        {
            get { return (string)GetValue(ImagePathProperty); }
            set { SetValue(ImagePathProperty, value); }
        }

        /// <summary>
        /// 元ファイルのパスプロパティ
        /// </summary>
        public string OriginalFilePath
        {
            get { return (string)GetValue(OriginalFilePathProperty); }
            set { SetValue(OriginalFilePathProperty, value); }
        }

        /// <summary>
        /// サムネイル画像のファイルパス
        /// </summary>
        /// <param name="d">依存関係プロパティ</param>
        /// <param name="e">イベントデータ</param>
        private static void OnImagePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ThumbnailFileView property = (ThumbnailFileView)d;
        }

        /// <summary>
        /// 元ファイルのパスプロパティ
        /// </summary>
        /// <param name="d">依存関係プロパティ</param>
        /// <param name="e">イベントデータ</param>
        private static void OnOriginalFilePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ThumbnailFileView property = (ThumbnailFileView)d;
        }
    }
}
