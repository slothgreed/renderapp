using System.Windows;
using System.Windows.Controls;

namespace KI.Presenter.Controls
{
    /// <summary>
    /// DirectoryAccessorView.xaml の相互作用ロジック
    /// </summary>
    public partial class DirectoryAccessorView : UserControl
    {
        public DirectoryAccessorView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォルダパスのプロパティ
        /// </summary>
        public static readonly DependencyProperty FolderPathProperty =
            DependencyProperty.Register(
                "FolderPath",
                typeof(string),
                typeof(DirectoryAccessorView),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnFolderPathChanged)));

        /// <summary>
        /// フォルダパスのプロパティ
        /// </summary>
        public string FolderPath
        {
            get { return (string)GetValue(FolderPathProperty); }
            set { SetValue(FolderPathProperty, value); }
        }

        /// <summary>
        /// フォルダパスのプロパティ
        /// </summary>
        /// <param name="d">依存関係プロパティ</param>
        /// <param name="e">イベントデータ</param>
        private static void OnFolderPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DirectoryAccessorView property = (DirectoryAccessorView)d;
        }
    }
}
