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
    /// AssetView.xaml の相互作用ロジック
    /// </summary>
    public partial class AssetTreeView : UserControl
    {
        public AssetTreeViewModel Model;

        public static AssetTreeViewModel ActiveItem
        {
            get;
            private set;
        }

        public AssetTreeView()
        {
            InitializeComponent();
        }

    }
}
