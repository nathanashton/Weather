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
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Common.Units;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    /// Interaction logic for SensorTypesWindow.xaml
    /// </summary>
    public partial class SensorTypesWindow : Window
    {
        
        private readonly SensorTypesViewModel _viewModel;

        public SensorTypesWindow(SensorTypesViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            _viewModel.Window = this;
            Loaded += SensorTypesWindow_Loaded;
        }

        private void SensorTypesWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.GetSensorTypes();
            _viewModel.RegisterDirtyHandlers();
        }

        private void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selection = ((ListBox)e.Source).SelectedItem;
            if (selection == null) return;
            _viewModel.Unit = ((ListBox)e.Source).SelectedItem as ISensorType;
            _viewModel.SelectedSensorType = new SensorType
            {
                SensorTypeId = _viewModel.Unit.SensorTypeId,
                Name = _viewModel.Unit.Name,
                SIUnit = _viewModel.Unit.SIUnit,
            };
        }

        public void SelectUnitInListBox(ISensorType sensorType)
        {
            lb.SelectedItem = sensorType;
        }
    }
}
