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
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms.Integration;
using RenderApp.Utility;
using RenderApp.AssetModel;
using System.IO;
using RenderApp.GLUtil;
namespace RenderApp
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        #region [Member変数]

        private Viewport m_Viewport;

        private CTimer m_GUITimer = null;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_Load_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.InitialDirectory = @"C:\cgModel";
            dlg.Filter = "objファイル(*.obj)|*.obj;|stlファイル(*.stl)|*.stl;|すべてのファイル(*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.Title = "開くファイルを選択してください。";


            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CObjFile model = new CObjFile(dlg.FileName);
            }
            m_Viewport.glControl_Paint(null, null);

        }

        public void OnAnimationTimer(object source, EventArgs e)
        {
            int sec = m_Viewport.RenderingMillSec;
            if(m_Viewport.RenderingMillSec == 0)
            {
                sec = 1;
            }
        }

    }
}
