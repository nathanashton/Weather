﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Weather.Common.Entities;
using Weather.ViewModels;
using System.Linq;

namespace Weather.Views
{
    /// <summary>
    /// Interaction logic for SensorWindow.xaml
    /// </summary>
    public partial class SensorWindow
    {
        public SensorWindowViewModel ViewModel { get; set; }
        private bool _save;

        public SensorWindow(SensorWindowViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = viewModel;
            Closing += AddSensorWindow_Closing;
            Loaded += SensorWindow_Loaded;
        }

        private void SensorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel.EditSensor != null)
            {
                ViewModel.Sensor =
                new Sensor
                {
                    Name = ViewModel.EditSensor.Name,
                    Type = ViewModel.EditSensor.Type,
                    Id = ViewModel.EditSensor.Id,
                    Station = ViewModel.EditSensor.Station,
                    Correction = ViewModel.EditSensor.Correction
                };
            }
        }

        private void AddSensorWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_save)
            {
                ViewModel.Sensor = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _save = false;
            ViewModel.Sensor = null;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _save = true;
            Close();
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (Types.SelectedItem == null) return;
            var f =(KeyValuePair<string, string>)Types.SelectedItem;

            var allEnums = Enum.GetValues(typeof(Enums.UnitType));
            foreach (var value in allEnums)
            {
                if (f.Key == value.ToString())
                {
                    var type = value.GetType();
                    var memInfo = type.GetMember(value.ToString());
                    var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute),false);
                    var description = ((DescriptionAttribute)attributes[0]).Description;
                    ViewModel.CorrectionValue = description;
                }
            }


        }
    }
}