using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PropertyChanged;
using Weather.Common.Interfaces;
using Weather.Common.Units;
using Weather.Core.Interfaces;
using Weather.Helpers;
using Weather.Views;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class UnitsWindowViewModel
    {
        private readonly ILog _log;
        private readonly IUnitCore _unitCore;
        public bool Adding { get; set; }

        public UnitsWindow Window { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ObservableCollection<Unit> Units { get; set; }
        public ObservableCollection<UnitType> UnitTypes { get; set; }
        public Unit SelectedUnit { get; set; }
        public Unit Unit { get; set; }
        public bool IsDirty { get; set; }



        public UnitsWindowViewModel(IUnitCore unitCore, ILog log)
        {
            _log = log;
            _unitCore = unitCore;
            UnitTypes = new ObservableCollection<UnitType>(Common.Units.UnitTypes.UnitsList);
        }
        
        public ICommand SaveCommand
        {
            // Only allow save is something has changed, A unit is selected and is Valid
            get { return new RelayCommand(Save, x => IsDirty && SelectedUnit != null && SelectedUnit.IsValid); }
        }

        public ICommand AddCommand
        {
            get { return new RelayCommand(Add, x => true); }
        }

        public ICommand DeleteCommand
        {
            // Only allow delete when a unit is selected and its Id is not 0. If it's zero then it hasnt been added yet.
            get { return new RelayCommand(Delete, x => SelectedUnit != null && SelectedUnit.UnitId != 0); }
        }

        public ICommand CancelCommand
        {
            // Allow cancel if something has changed OR a unit is being added
            get { return new RelayCommand(Cancel, x => (IsDirty || Adding) && SelectedUnit != null); }
        }

        private void Add(object obj)
        {
            Adding = true;
            var unit = new Unit();
            Units.Add(unit);
            Window.SelectUnitInListBox(unit);
            Window.DisplayName.Focus();
        }

        private void Delete(object obj)
        {
            var result = MessageBox.Show("Delete " + SelectedUnit.DisplayName + "?", "Confirm", MessageBoxButton.YesNo,
                MessageBoxImage.Stop);
            if (result != MessageBoxResult.Yes) return;
            _unitCore.Delete(SelectedUnit);

            SelectedUnit = null;
            Unit = null;
            CheckDirty();
            GetAll();
        }

        private void Cancel(object obj)
        {
            if (Adding)
            {
                GetAll();
            }
            Window.DisplayName.Text = Unit.DisplayName;
            Window.DisplayUnit.Text = Unit.DisplayUnit;
            Window.UnitType.SelectedItem = Unit.UnitType;
            Adding = false;
            SelectedUnit = null;
            Unit = null;
            IsDirty = false;
        }

        public void RegisterDirtyHandlers()
        {
            Window.DisplayName.TextChanged += DisplayName_TextChanged;
            Window.DisplayUnit.TextChanged += DisplayUnit_TextChanged;
            Window.UnitType.SelectionChanged += UnitType_SelectionChanged;
        }

        private void UnitType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckDirty();
        }

        private void DisplayUnit_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckDirty();
        }

        private void DisplayName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckDirty();
        }

        public void GetAll()
        {
            Units = new ObservableCollection<Unit>(_unitCore.GetAll());
        }

        public void Save(object obj)
        {
            if (!SelectedUnit.IsValid) return;
            _unitCore.AddOrUpdate(SelectedUnit);
            _log.Debug("Saved Unit");
            Adding = false;
            GetAll();
            IsDirty = false;
        }

        public void CheckDirty()
        {
            if (SelectedUnit == null || Unit == null) return;
            if (SelectedUnit.DisplayName != Unit.DisplayName
                || SelectedUnit.DisplayUnit != Unit.DisplayUnit
                || SelectedUnit.UnitType != Unit.UnitType)
            {
                IsDirty = true;
            }
            else
            {
                IsDirty = false;
            }
        }
    }
}