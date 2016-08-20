using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Weather.Common.Interfaces;
using Weather.Common.Units;
using Weather.Core.Interfaces;
using Weather.Helpers;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class UnitSelectorWindowViewModel
    {
        private IUnitCore _unitCore;
        private ISensorTypeCore _sensorTypeCore;
        public ObservableCollection<Unit> Units { get; set; }
        public Unit SelectedUnit { get; set; }
        
        public ISensorType SensorType { get; set; }

        public UnitSelectorWindowViewModel(IUnitCore unitCore, ISensorTypeCore sensorTypeCore)
        {
            _unitCore = unitCore;
            _sensorTypeCore = sensorTypeCore;
            GetAllUnits();
        }

        public ICommand AddCommand
        {
            get { return new RelayCommand(Add, x => SelectedUnit != null); }
        }

        private void GetAllUnits()
        {
            Units = new ObservableCollection<Unit>(_unitCore.GetAll());
            SelectedUnit = Units.Count == 0 ? null : Units.First();
        }

        private void Add(object obj)
        {
            _sensorTypeCore.AddUnitToSensorType(SelectedUnit, SensorType);
        }
    }
}
