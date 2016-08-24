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
    /// Interaction logic for SetupStationsSensorsWindow.xaml
    /// </summary>
    public partial class SetupStationsSensorsWindow : Window
    {
        private SetupStationsSensorsWindowViewModel _viewModel;
        public SetupStationsSensorsWindow(SetupStationsSensorsWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
