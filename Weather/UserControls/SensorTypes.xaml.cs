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
using Microsoft.Practices.Unity;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.DependencyResolver;
using Weather.ViewModels;
using Weather.Views;

namespace Weather.UserControls
{
    /// <summary>
    /// Interaction logic for SensorTypes.xaml
    /// </summary>
    public partial class SensorTypes : UserControl
    {
        private SensorTypesViewModel _viewModel;

        public SensorTypes()
        {
            InitializeComponent();
            Loaded += SensorTypes_Loaded;
        }

        private void SensorTypes_Loaded(object sender, RoutedEventArgs e)
        {
            var container = new Resolver().Bootstrap();
            _viewModel = container.Resolve<SensorTypesViewModel>();
            DataContext = _viewModel;
            _viewModel.Window = this;
            _viewModel.GetSensorTypes();
            _viewModel.RegisterDirtyHandlers();
        }

        private void Lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selection = ((ListBox)e.Source).SelectedItem;
            if (selection == null)
            {
                return;
            }

            _viewModel.TempSelectedSensorType = selection as ISensorType;
            if (_viewModel.TempSelectedSensorType != null)
            {
                _viewModel.SelectedSensorType = new SensorType
                {
                    SensorTypeId = _viewModel.TempSelectedSensorType.SensorTypeId,
                    Name = _viewModel.TempSelectedSensorType.Name,
                    UnitType = _viewModel.TempSelectedSensorType.UnitType
                };
            }
        }

        public void SelectUnitInListBox(ISensorType sensorType)
        {
            Lb.SelectedItem = sensorType;
        }

    }
}
