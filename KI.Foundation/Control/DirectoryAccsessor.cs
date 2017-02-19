using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KI.Foundation.Control
{
    public class DirectoryAccsessor : System.Windows.Controls.Control
    {
        public static readonly DependencyProperty FolderPathProperty =
            DependencyProperty.Register("FolderPath", typeof(string), typeof(DirectoryAccsessor), new PropertyMetadata("",new PropertyChangedCallback(OnFolderPathChanged)));

        public string FolderPath
        {
            get { return (string)GetValue(FolderPathProperty); }
            set { SetValue(FolderPathProperty, value); }
        }


        public static readonly DependencyProperty InitialPathProperty = DependencyProperty.Register("InitialPath", typeof(string), typeof(DirectoryAccsessor), new PropertyMetadata(""));
        public string InitialPath
        {
            get { return (string)GetValue(InitialPathProperty); }
            set { SetValue(InitialPathProperty, value); }
        }        
        
        static DirectoryAccsessor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DirectoryAccsessor), new FrameworkPropertyMetadata(typeof(DirectoryAccsessor)));
        }
        private Button RefButton = null;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            RefButton = this.GetTemplateChild("PART_RefButton") as Button;
            RefButton.Click += RefButton_Click;
        }

        void RefButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "フォルダの選択";
            dialog.SelectedPath = FolderPath;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FolderPath = dialog.SelectedPath;
            }
        }

        private static void OnFolderPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}
