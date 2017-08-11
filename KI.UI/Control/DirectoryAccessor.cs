using System.Windows;
using System.Windows.Controls;

namespace KI.UI.Control
{
    public class DirectoryAccessor : System.Windows.Controls.Control
    {
        public static readonly DependencyProperty FolderPathProperty =
            DependencyProperty.Register("FolderPath", typeof(string), typeof(DirectoryAccessor), new PropertyMetadata(string.Empty));

        public string FolderPath
        {
            get
            {
                return (string)GetValue(FolderPathProperty);
            }

            set
            {
                SetValue(FolderPathProperty, value);
            }
        }

        static DirectoryAccessor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DirectoryAccessor), new FrameworkPropertyMetadata(typeof(DirectoryAccessor)));
        }

        private Button PART_RefButton = null;

        private TextBlock PART_FolderPath = null;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_RefButton = this.GetTemplateChild("PART_RefButton") as Button;
            PART_RefButton.Click += RefButton_Click;

            PART_FolderPath = this.GetTemplateChild("PART_FolderPath") as TextBlock;
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
    }
}
