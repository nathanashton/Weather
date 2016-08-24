using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Weather.Common.Units;
using Weather.Core.Interfaces;
using Weather.Helpers;
using Weather.UserControls;
using static System.String;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class UnitsViewModel
    {
        private readonly IUnitCore _unitCore;
        private readonly ISensorTypeCore _sensorTypeCore;

        public UnitsViewModel(IUnitCore unitCore, ISensorTypeCore sensorTypeCore)
        {
            _unitCore = unitCore;
            _sensorTypeCore = sensorTypeCore;
            Units = new ObservableCollection<Unit>();
            GetUnits();
            UnitTypes = new ObservableCollection<UnitType>(Common.Units.UnitTypes.UnitsList);
            SelectedUnit = Units.Count == 0 ? null : Units.First();
        }

        public ObservableCollection<UnitType> UnitTypes { get; set; }

        public UnitsPage UnitsWindow { get; set; }
        public bool IsDirty { get; set; }
        public ObservableCollection<Unit> Units { get; set; }

        public Unit SelectedUnit { get; set; }

        public ICommand SaveCommand
        {
            get { return new RelayCommand(Save, x => IsDirty); }
        }

        public ICommand AddCommand
        {
            get { return new RelayCommand(Add, x => true); }
        }

        public ICommand DeleteCommand
        {
            get { return new RelayCommand(Delete, x => SelectedUnit != null); }
        }

        public void GetUnits()
        {
            Units = new ObservableCollection<Unit>(_unitCore.GetAll());
            if (SelectedUnit != null)
            {
                SelectedUnit = Units.FirstOrDefault(x => x.UnitId == SelectedUnit.UnitId);
            }
        }

        public void RegisterDirtyHandlers()
        {
            UnitsWindow.DisplayName.TextChanged += Name_TextChanged;
            UnitsWindow.DisplayUnit.TextChanged += DisplayUnit_TextChanged;
            UnitsWindow.UnitType.SelectionChanged += UnitType_SelectionChanged;
        }

        private void UnitType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedUnit == null) return;
            if (UnitsWindow.UnitType.SelectedItem != SelectedUnit.UnitType)
            {
                IsDirty = true;
            }
            else
            {
                IsDirty = false;
            }
        }

        private void DisplayUnit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SelectedUnit == null) return;
            if (UnitsWindow.DisplayUnit.Text != SelectedUnit.DisplayUnit)
            {
                IsDirty = true;
            }
            else
            {
                IsDirty = false;
            }
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SelectedUnit == null) return;
            if (UnitsWindow.DisplayName.Text != SelectedUnit.DisplayName)
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
            SelectedUnit = new Unit();
            UnitsWindow.DisplayName.Focus();
        }

        private void Delete(object obj)
        {
            //TODO Check if any SensorTypes use this unit. If yes then do not allow delete
            var existingSensorTypes = _sensorTypeCore.AnySensorTypesUseUnit(SelectedUnit);
            if (existingSensorTypes)
            {
                MessageBox.Show(
                    "Cannot delete " + SelectedUnit.DisplayName + " as it is currently in use by existing Sensor Types",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show("Delete Unit \"" + SelectedUnit.DisplayName + "\" ?", "Confirm Delete",
                MessageBoxButton.YesNo, MessageBoxImage.Stop);
            if (result == MessageBoxResult.Yes)
            {
                _unitCore.Delete(SelectedUnit);
                GetUnits();
            }
        }

        public bool Validate(Unit unit)
        {
            return !IsNullOrEmpty(unit.DisplayName) && !IsNullOrEmpty(unit.DisplayUnit);
        }

        public void Save(object obj)
        {
            // We need to create a temp object to validate against as the bindings haven't been committed yet.
            var tempUnit = new Unit
            {
                DisplayName = UnitsWindow.DisplayName.Text,
                DisplayUnit = UnitsWindow.DisplayUnit.Text,
                UnitType = (UnitType)UnitsWindow.UnitType.SelectedItem
            };
            if (!Validate(tempUnit))
            {
                MessageBox.Show("Unit not valid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //Commit bindings
            var textName = UnitsWindow.DisplayName.GetBindingExpression(TextBox.TextProperty);
            textName?.UpdateSource();
            var textDisplayUnit = UnitsWindow.DisplayUnit.GetBindingExpression(TextBox.TextProperty);
            textDisplayUnit?.UpdateSource();
            var unitType = UnitsWindow.UnitType.GetBindingExpression(Selector.SelectedItemProperty);
            unitType?.UpdateSource();


            _unitCore.AddOrUpdate(SelectedUnit);

            IsDirty = false;
            GetUnits();
        }
    }
}