using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Weather.Common.Units;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    ///     Interaction logic for SensorWindow.xaml
    /// </summary>
    public partial class SensorWindow
    {
        private bool _save;

        public SensorWindow()
        {
            InitializeComponent();
          //  ViewModel = viewModel;
           // DataContext = viewModel;
            Closing += AddSensorWindow_Closing;
            Loaded += SensorWindow_Loaded;
        }

       // public SensorWindowViewModel ViewModel { get; set; }

        private void SensorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //if (ViewModel.EditSensor != null)
            //{
            //    ViewModel.Sensor =
            //        new Sensor
            //        {
            //            Name = ViewModel.EditSensor.Name,
            //            Type = ViewModel.EditSensor.Type,
            //            SensorId = ViewModel.EditSensor.SensorId,
            //            Station = ViewModel.EditSensor.Station,
            //            // Correction = ViewModel.EditSensor.Correction
            //        };
            //}
        }

        private void AddSensorWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!_save)
            {
              //  ViewModel.Sensor = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _save = false;
           // ViewModel.Sensor = null;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _save = true;
            Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Types.SelectedItem == null) return;
            var f = (KeyValuePair<string, string>) Types.SelectedItem;

           // var allEnums = Enum.GetValues(typeof(UnitType));
            //foreach (var value in allEnums)
            //{
            //    if (f.Key == value.ToString())
            //    {
            //        var type = value.GetType();
            //        var memInfo = type.GetMember(value.ToString());
            //        var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            //        var description = ((DescriptionAttribute) attributes[0]).Description;
            //        ViewModel.CorrectionValue = description;
            //    }
            //}
        }
    }
}