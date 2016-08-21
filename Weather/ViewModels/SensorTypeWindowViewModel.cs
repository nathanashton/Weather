using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Common.Units;
using Weather.Core.Interfaces;
using Weather.DependencyResolver;
using Weather.Helpers;
using Weather.Views;
using static System.String;
using Microsoft.Practices.Unity;


namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class SensorTypeWindowViewModel
    {
        public SensorTypesWindow SensorTypesWindow { get; set; }
        public bool IsDirty { get; set; }
        public ObservableCollection<ISensorType> SensorTypes { get; set; }
        public Unit SelectedUnit { get; set; }
        public ISensorType SelectedSensorType { get; set; }
        private readonly ISensorTypeCore _sensorTypeCore;

        public SensorTypeWindowViewModel(ISensorTypeCore sensorTypeCore)
        {
            _sensorTypeCore = sensorTypeCore;
            SensorTypes = new ObservableCollection<ISensorType>();
            GetSensorTypes();
        } 

             public ICommand SaveCommand
        {
            get { return new RelayCommand(Save, x => IsDirty); }
        }

        public ICommand AddUnitCommand
        {
            get { return new RelayCommand(AddUnit, x => SelectedSensorType != null); }
        }

        public ICommand DeleteUnitCommand
        {
            get { return new RelayCommand(DeleteUnit, x => SelectedUnit != null); }
        }

        public ICommand AddCommand
        {
            get { return new RelayCommand(Add, x => true); }
        }

        public ICommand DeleteCommand
        {
            get { return new RelayCommand(Delete, x => SelectedSensorType != null); }
        }

        private void GetSensorTypes()
        {
            SensorTypes.Clear();
            SensorTypes = new ObservableCollection<ISensorType>(_sensorTypeCore.GetAll());
            SelectedSensorType = SensorTypes.Count == 0 ? null : SensorTypes.First();
        }

        public void RegisterDirtyHandlers()
        {
            SensorTypesWindow.Name.TextChanged += Name_TextChanged;
        }

        private void Name_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (SelectedSensorType == null) return;
            if (SensorTypesWindow.Name.Text != SelectedSensorType.Name)
            {
                IsDirty = true;
            }
            else
            {
                IsDirty = false;
            }
        }

        private void Add(object obj)
        {
            SelectedSensorType = new SensorType();
            SensorTypesWindow.Name.Focus();
        }

        private void Delete(object obj)
        {
            var result = MessageBox.Show("Delete Sensor Type \"" + SelectedSensorType.Name + "\" ?", "Confirm Delete",
                MessageBoxButton.YesNo, MessageBoxImage.Stop);
            if (result == MessageBoxResult.Yes)
            {
                _sensorTypeCore.Delete(SelectedSensorType);
                GetSensorTypes();
            }
        }

        public bool Validate(ISensorType sensorType)
        {
            return !IsNullOrEmpty(sensorType.Name);
        }

        public void Save(object obj)
        {
            // We need to create a temp object to validate against as the bindings haven't been committed yet.
            var tempSensor = new SensorType
            {
                Name = SensorTypesWindow.Name.Text
            };
            if (!Validate(tempSensor))
            {
                MessageBox.Show("Sensor Type not valid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var textName = SensorTypesWindow.Name.GetBindingExpression(TextBox.TextProperty);
            textName?.UpdateSource();

            _sensorTypeCore.AddOrUpdate(SelectedSensorType);

            IsDirty = false;
            GetSensorTypes();
        }

        private void AddUnit(object obj)
        {
            var container = new Resolver().Bootstrap();
            var window = container.Resolve<UnitSelectorWindow>();
            window._viewModel.SensorType = SelectedSensorType;
            window.ShowDialog();
            GetSensorTypes();
        }

        private void DeleteUnit(object obj)
        {
            _sensorTypeCore.RemoveUnitFromSensorType(SelectedUnit, SelectedSensorType);
            GetSensorTypes();
        }
    }


}