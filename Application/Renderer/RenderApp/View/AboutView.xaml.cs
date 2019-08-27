using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace RenderApp.View
{
    /// <summary>
    /// AboutView.xaml の相互作用ロジック
    /// </summary>
    public partial class AboutView : Window
    {
        public AboutView()
        {
            InitializeComponent();

            var assembly = Assembly.GetEntryAssembly();
            var version = assembly.GetName().Version;
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            var baseDate = new DateTime(2000, 1, 1);
            this.Version.Content = string.Format("Version : {0}.{1}.{2}.{3}", 
                fileVersionInfo.ProductMajorPart,
                fileVersionInfo.ProductMinorPart,
                fileVersionInfo.ProductBuildPart,
                fileVersionInfo.ProductPrivatePart);
            this.Build.Content = string.Format("Build Date : {0}", baseDate.AddDays(version.Build).ToShortDateString());
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
