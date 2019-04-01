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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KI.UI.Controls
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
                new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(OnFolderPathChanged)));

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
