﻿using System.Windows;
using System.Windows.Controls;

namespace Xky.Platform.Pages
{
    /// <summary>
    ///     MyTask.xaml 的交互逻辑
    /// </summary>
    public partial class Setting
    {
        public Setting()
        {
            InitializeComponent();
        }

        private void Combobox_test_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show(combobox_test.SelectedValue.ToString());
        }

        private void Togglebutton_test_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void Togglebutton_test_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(togglebutton_test.IsChecked.ToString());
        }
    }
}