using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using PropertyChanged;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Common.Units;
using Weather.Core.Interfaces;
using Weather.DependencyResolver;
using Weather.Helpers;
using Weather.Views;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class SensorTypesViewModel
    {
        private readonly ILog _log;
        private readonly ISensorCore _sensorCore;
        private readonly ISensorTypeCore _sensorTypeCore;
        private readonly IUnitCore _unitCore;
        public ISelectedStation SelectedStation;

        public SensorTypesWindow Window { get; set; }
        public ObservableCollection<ISensorType> SensorTypes { get; set; }
        public Unit SelectedUnit { get; set; }
        public ObservableCollection<Unit> Units { get; set; }
        public bool IsDirty { get; set; }
        public ISensorType SelectedSensorType { get; set; }
        public ISensorType TempSelectedSensorType { get; set; }
        public bool Adding { get; set; }

        public ICommand CancelCommand
        {
            get { return new RelayCommand(Cancel, x => (IsDirty || Adding) && (SelectedSensorType != null)); }
        }

        public ICommand UnitsWindowCommand
        {
            get { return new RelayCommand(OpenUnitsWindow, x => true); }
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

        public ICommand AddUnitCommand
        {
            get { return new RelayCommand(AddUnit, x => SelectedSensorType != null); }
        }

        public ICommand DeleteUnitCommand
        {
            get { return new RelayCommand(DeleteUnit, x => (SelectedSensorType != null) && (SelectedUnit != null)); }
        }

        public ICommand AddCommand
        {
            get { return new RelayCommand(Add, x => true); }
        }

        public SensorTypesViewModel(ISensorTypeCore sensorTypeCore, ILog log, IUnitCore unitCore, ISensorCore sensorCore,
            ISelectedStation selectedStation)
        {
            SelectedStation = selectedStation;
            _log = log;
            _sensorCore = sensorCore;
            _unitCore = unitCore;
            _sensorTypeCore = sensorTypeCore;
            SensorTypes = new ObservableCollection<ISensorType>();
        }

        private void OpenUnitsWindow(object obj)
        {
            var id2 = SelectedSensorType.SensorTypeId;
            var container = new Resolver().Bootstrap();
            var window = container.Resolve<UnitsWindow>();
            window.ShowDialog();

            GetSensorTypes();

            SelectedSensorType = SensorTypes.First(x => x.SensorTypeId == id2);
            Window.SelectUnitInListBox(SelectedSensorType);
        }

        public void GetSensorTypes()
        {
            Units = new ObservableCollection<Unit>(_unitCore.GetAll());
            SensorTypes = new ObservableCollection<ISensorType>(_sensorTypeCore.GetAll());
        }

        public void RegisterDirtyHandlers()
        {
            Window.Name.TextChanged += Name_TextChanged;
            Window.SiUnit.SelectionChanged += SIUnit_SelectionChanged;
        }

        private void SIUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            Window.SelectSiUnitInComboBox(TempSelectedSensorType.SIUnit);
            Adding = false;
            SelectedSensorType = null;
            TempSelectedSensorType = null;
            IsDirty = false;
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckDirty();
        }

        private void AddUnit(object obj)
        {
            var container = new Resolver().Bootstrap();
            var window = container.Resolve<UnitSelectorWindow>();
            window.ViewModel.SensorType = SelectedSensorType;
            window.ShowDialog();

            // Get Unit that was added from the above dialog.
            var unit = SelectedSensorType.Units.LastOrDefault();

            if (Adding)
            {
                if (unit != null)
                {
                    if (SelectedSensorType.SIUnit == null)
                    {
                        SelectedSensorType.SIUnit = unit;
                    }
                    return;
                }
            }

            if (unit == null)
            {
                return; // No unit was added
            }
            var id = SelectedSensorType.SensorTypeId;
            _sensorTypeCore.AddUnitToSensorType(unit, SelectedSensorType);
            CheckDirty();
            GetSensorTypes();

            SelectedSensorType = SensorTypes.First(x => x.SensorTypeId == id);
            Window.SelectSiUnitInComboBox(SelectedSensorType.SIUnit);
        }

        private void DeleteUnit(object obj)
        {
            if (SelectedSensorType.SIUnit.UnitId == SelectedUnit.UnitId)
            {
                MessageBox.Show("Cannot delete " + SelectedUnit.DisplayName + " unit as it is set as the SI Unit.",
                    "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (SelectedSensorType.Units.Count == 1)
            {
                MessageBox.Show("Cannot delete " + SelectedUnit.DisplayName + " as it is the only unit.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var id = SelectedSensorType.SensorTypeId;
            _sensorTypeCore.RemoveUnitFromSensorType(SelectedUnit, SelectedSensorType);
            CheckDirty();
            GetSensorTypes();
            SelectedSensorType = SensorTypes.First(x => x.SensorTypeId == id);
            Window.SelectSiUnitInComboBox(SelectedSensorType.SIUnit);
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
                (SelectedSensorType.SIUnit.UnitId != TempSelectedSensorType.SIUnit.UnitId))
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
                foreach (var unit in SelectedSensorType.Units)
                {
                    _sensorTypeCore.AddUnitToSensorType(unit, SelectedSensorType);
                }
                SelectedSensorType.SIUnit = SelectedSensorType.Units.First();
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