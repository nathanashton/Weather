using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.DependencyResolver;
using Weather.ViewModels;

namespace Weather.UserControls
{
    /// <summary>
    ///     Interaction logic for SensorTypes.xaml
    /// </summary>
    public partial class SensorTypes : UserControl
    {
        private readonly SensorTypesViewModel _viewModel;

        public SensorTypes()
        {
            InitializeComponent();
            var container = new Resolver().Bootstrap();
            _viewModel = container.Resolve<SensorTypesViewModel>();
            DataContext = _viewModel;
            _viewModel.Window = this;
            Loaded += SensorTypes_Loaded;
        }

        private void SensorTypes_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.GetSensorTypes();
            _viewModel.RegisterDirtyHandlers();
        }

        private void Lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selection = ((ListBox) e.Source).SelectedItem;
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