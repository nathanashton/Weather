using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Weather.Common.Interfaces;
using Weather.Common.Units;
using Weather.Core.Interfaces;
using Weather.Helpers;
using Weather.Views;
using static System.String;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class UnitsWindowViewModel
    {
        public UnitsWindow UnitsWindow { get; set; }
        public bool IsDirty { get; set; }
        public ObservableCollection<Unit> Units { get; set; }


        public Unit SelectedUnit { get; set; }
        private readonly IUnitCore _unitCore;

        public UnitsWindowViewModel(IUnitCore unitCore)
        {
            _unitCore = unitCore;
            Units = new ObservableCollection<Unit>();
            GetUnits();
        }

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

        private void GetUnits()
        {
            Units = new ObservableCollection<Unit>(_unitCore.GetAll());
            SelectedUnit = Units.Count == 0 ? null : Units.First();
        }

        public void RegisterDirtyHandlers()
        {
            UnitsWindow.Name.TextChanged += Name_TextChanged;
        }

        private void Name_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (SelectedUnit == null) return;
            if (UnitsWindow.Name.Text != SelectedUnit.DisplayName)
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
            UnitsWindow.Name.Focus();
        }

        private void Delete(object obj)
        {
            var result = MessageBox.Show("Delete Unit \"" + SelectedUnit.DisplayName + "\" ?", "Confirm Delete",
                MessageBoxButton.YesNo, MessageBoxImage.Stop);
            if (result == MessageBoxResult.Yes)
            {
                // _sensorTypeCore.Delete(SelectedSensorType);
                GetUnits();
            }
        }

        public bool Validate(Unit unit)
        {
            return !IsNullOrEmpty(unit.DisplayName);
        }

        public void Save(object obj)
        {
            // We need to create a temp object to validate against as the bindings haven't been committed yet.
            var tempUnit = new Unit
            {
                DisplayName = UnitsWindow.Name.Text
            };
            if (!Validate(tempUnit))
            {
                MessageBox.Show("Unit not valid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var textName = UnitsWindow.Name.GetBindingExpression(TextBox.TextProperty);
            textName?.UpdateSource();

            _unitCore.AddOrUpdate(SelectedUnit);

            IsDirty = false;
            GetUnits();
        }
    }
}