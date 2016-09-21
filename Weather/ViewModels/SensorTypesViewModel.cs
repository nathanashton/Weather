using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PropertyChanged;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.Helpers;
using Weather.Units.Interfaces;
using Weather.UserControls;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class SensorTypesViewModel
    {
        private readonly ILog _log;
        private readonly ISensorCore _sensorCore;
        private readonly ISensorTypeCore _sensorTypeCore;
        public ISelectedStation SelectedStation;

        public ObservableCollection<IUnitType> UnitTypes { get; set; }
        public SensorTypes Window { get; set; }
        public ObservableCollection<ISensorType> SensorTypes { get; set; }

        public bool IsDirty { get; set; }
        public ISensorType SelectedSensorType { get; set; }
        public ISensorType TempSelectedSensorType { get; set; }
        public bool Adding { get; set; }

        public ICommand CancelCommand
        {
            get { return new RelayCommand(Cancel, x => (IsDirty || Adding) && (SelectedSensorType != null)); }
        }


        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save, x => IsDirty && (SelectedSensorType != null) && SelectedSensorType.IsValid);
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new RelayCommand(Delete,
                    x => (SelectedSensorType != null) && (SelectedSensorType.SensorTypeId != 0));
            }
        }


        public ICommand AddCommand
        {
            get { return new RelayCommand(Add, x => true); }
        }

        public SensorTypesViewModel(ISensorTypeCore sensorTypeCore, ILog log, ISensorCore sensorCore,
            ISelectedStation selectedStation)
        {
            SelectedStation = selectedStation;
            _log = log;
            _sensorCore = sensorCore;
            _sensorTypeCore = sensorTypeCore;
            SensorTypes = new ObservableCollection<ISensorType>();
            UnitTypes = new ObservableCollection<IUnitType>(Units.UnitTypes.AllUnitTypes);
        }


        public void GetSensorTypes()
        {
            SensorTypes = new ObservableCollection<ISensorType>(_sensorTypeCore.GetAll());
        }

        public void RegisterDirtyHandlers()
        {
            Window.Name.TextChanged += Name_TextChanged;
            Window.SiUnit.SelectionChanged += SiUnit_SelectionChanged;
        }

        private void SiUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckDirty();
        }

        private void Cancel(object obj)
        {
            if (Adding)
            {
                GetSensorTypes();
            }
            Window.Name.Text = TempSelectedSensorType.Name;

            Adding = false;
            SelectedSensorType = null;
            TempSelectedSensorType = null;
            IsDirty = false;
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckDirty();
        }


        private void Delete(object obj)
        {
            var used = _sensorCore.AnySensorUsesSensorType(SelectedSensorType);
            if (used)
            {
                MessageBox.Show("Cannot delete " + SelectedSensorType + " as it is used by one or more Sensors", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show("Delete " + SelectedSensorType.Name + "?", "Confirm", MessageBoxButton.YesNo,
                MessageBoxImage.Stop);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            _sensorTypeCore.Delete(SelectedSensorType);

            SelectedSensorType = null;
            TempSelectedSensorType = null;
            CheckDirty();
            GetSensorTypes();
        }

        public void CheckDirty()
        {
            if (Adding)
            {
                IsDirty = true;
                return;
            }
            if ((SelectedSensorType == null) || (TempSelectedSensorType == null))
            {
                return;
            }
            if ((SelectedSensorType.Name != TempSelectedSensorType.Name) ||
                (SelectedSensorType.UnitType != TempSelectedSensorType.UnitType))
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
            Adding = true;
            var unit = new SensorType();
            SensorTypes.Add(unit);
            Window.SelectUnitInListBox(unit);
            Window.Name.Focus();
        }

        public void Save(object obj)
        {
            if (Adding)
            {
                _sensorTypeCore.Add(SelectedSensorType);
            }

            if (!SelectedSensorType.IsValid)
            {
                return;
            }
            _sensorTypeCore.AddOrUpdate(SelectedSensorType);
            _log.Debug("Saved Sensor Type");
            Adding = false;
            GetSensorTypes();
            IsDirty = false;
        }
    }
}