using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PropertyChanged;
using Weather.Common.Interfaces;
using Weather.Common.Units;
using Weather.Core.Interfaces;
using Weather.Helpers;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class UnitSelectorWindowViewModel
    {
        private readonly IUnitCore _unitCore;

        public Window Window { get; set; }
        public ObservableCollection<Unit> Units { get; set; }
        public Unit SelectedUnit { get; set; }

        public ISensorType SensorType { get; set; }

        public ICommand AddCommand
        {
            get { return new RelayCommand(Add, x => SelectedUnit != null); }
        }

        public UnitSelectorWindowViewModel(IUnitCore unitCore)
        {
            _unitCore = unitCore;
        }

        public void GetAllUnits()
        {
            if (SensorType == null)
            {
                return;
            }
            var allUnits = _unitCore.GetAll();
            var pp = allUnits.Where(p => SensorType.Units.All(p2 => p2.UnitId != p.UnitId));
            Units = new ObservableCollection<Unit>(pp);
            SelectedUnit = Units.Count == 0 ? null : Units.First();
        }

        private void Add(object obj)
        {
            SensorType.Units.Add(SelectedUnit);
            Window.Close();
        }
    }
}