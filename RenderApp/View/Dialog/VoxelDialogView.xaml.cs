﻿using System;
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
using RenderApp.ViewModel.Dialog;
namespace RenderApp.View.Dialog
{
    /// <summary>
    /// VoxelDialogView.xaml の相互作用ロジック
    /// </summary>
    public partial class VoxelDialogView : Window
    {
        private VoxelDialogViewModel vm;
        public VoxelDialogView(VoxelDialogViewModel data)
        {
            InitializeComponent();
            this.DataContext = data;
            vm = data;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            vm.Close(true);
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            vm.Close(false);
            this.DialogResult = false;
            this.Close();
        }
    }
}
