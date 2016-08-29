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
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    /// Interaction logic for SensorSelectWindow.xaml
    /// </summary>
    public partial class SensorSelectWindow : Window
    {
        public SensorSelectWindowViewModel _viewModel;

        public SensorSelectWindow(SensorSelectWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            _viewModel.Window = this;
            Loaded += SensorSelectWindow_Loaded;
        }

        private void SensorSelectWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.GetAllSensors();
            if (_viewModel.SelectedSensor != null)
            {
                cb.SelectedItem = _viewModel.SelectedSensor;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
