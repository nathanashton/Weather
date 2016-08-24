using Microsoft.Practices.Unity;
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
using Weather.UserControls;
using Weather.Views;
using static System.String;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class SensorTypesViewModel
    {
        public SensorTypesWindow Window { get; set; }
        private readonly ISensorTypeCore _sensorTypeCore;
        private readonly IUnitCore _unitCore;
        private ILog _log;
        public ObservableCollection<ISensorType> SensorTypes { get; set; }
        public Unit SelectedUnit { get; set; }
        public ObservableCollection<Unit> Units { get; set; }
        public bool IsDirty { get; set; }
        public ISensorType SelectedSensorType { get; set; }
        public ISensorType Unit { get; set; }
        public bool Adding { get; set; }

        public SensorTypesViewModel(ISensorTypeCore sensorTypeCore, ILog log, IUnitCore unitCore)
        {
            _log = log;
            _unitCore = unitCore;
            _sensorTypeCore = sensorTypeCore;
            SensorTypes = new ObservableCollection<ISensorType>();
        }

        public void GetSensorTypes()
        {
            SensorTypes = new ObservableCollection<ISensorType>(_sensorTypeCore.GetAll());
            Units = new ObservableCollection<Common.Units.Unit>(_unitCore.GetAll());
        }

        public void RegisterDirtyHandlers()
        {
            Window.Name.TextChanged += Name_TextChanged;
            Window.SIUnit.SelectionChanged += SIUnit_SelectionChanged;
        }

        private void SIUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckDirty();
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckDirty();
        }

        public ICommand SaveCommand
        {
            // Only allow save is something has changed, A unit is selected and is Valid
            get { return new RelayCommand(Save, x => IsDirty && SelectedSensorType != null && SelectedSensorType.IsValid); }
        }


        public ICommand AddCommand
        {
            get { return new RelayCommand(Add, x => true); }
        }

        public void CheckDirty()
        {
            if (SelectedSensorType == null || Unit == null) return;
            if (SelectedSensorType.Name != Unit.Name
                || SelectedSensorType.SIUnit != Unit.SIUnit)
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
            if (!SelectedSensorType.IsValid) return;
            _sensorTypeCore.AddOrUpdate(SelectedSensorType);
            _log.Debug("Saved Sensor Type");
            Adding = false;
            GetSensorTypes();
            IsDirty = false;
        }
    }
}