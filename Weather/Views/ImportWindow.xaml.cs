﻿using Microsoft.Win32;
using System.Windows;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    /// Interaction logic for ImportWindow.xaml
    /// </summary>
    public partial class ImportWindow : Window
    {


        private ImportWindowViewModel _viewModel;

        public ImportWindow(ImportWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _viewModel.ReadFile(openFileDialog.FileName);
                lb.ItemsSource = _viewModel.Records[_viewModel.CurrentRecord];
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (_viewModel.CurrentRecord == 0)
            {
                return;
            }
            var next = _viewModel.CurrentRecord - 1;
            if (next >= 0)
            {
                lb.ItemsSource = _viewModel.Records[next];
                _viewModel.CurrentRecord-=1;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var next = _viewModel.CurrentRecord + 1;
            if (next < _viewModel.RecordsCount)
            {
                lb.ItemsSource = _viewModel.Records[next];
                _viewModel.CurrentRecord += 1;
            }
        }
    }
}