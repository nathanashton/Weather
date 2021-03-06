﻿using System.Windows;
using PropertyChanged;
using Weather.Common.Interfaces;

namespace Weather.Charts.LineGraph
{
    /// <summary>
    ///     Interaction logic for LineGraphControl.xaml
    /// </summary>
    [ImplementPropertyChanged]
    public partial class LineGraphControl : IChartCodeBehind
    {
        private string S { get; set; }

        public LineGraphControl(ISelectedStation selectedStation)
        {
            InitializeComponent();
            ViewModel = new LineGraphControlViewModel(selectedStation);
            DataContext = ViewModel;
            Loaded += MinMax_Loaded;
            Unloaded += MinMax_Unloaded;
        }

        public IChartViewModel ViewModel { get; set; }

        private void MinMax_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedStation.RecordsUpdatedForSelectedStation -= ViewModel.RecordsUpdatedForSelectedStation;
            ViewModel.SelectedStation.GetRecordsCompleted -= ViewModel.SelectedStation_GetRecordsCompleted;
            ViewModel.SelectedStation.SelectedStationChanged -= ViewModel.SelectedStation_SelectedStationChanged;
            ViewModel.SelectedStation.ChangesMadeToSelectedStation -= ViewModel.ChangesMadeToSelectedStation;
            ViewModel.SavePosition();
        }


        private void MinMax_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedStation.RecordsUpdatedForSelectedStation += ViewModel.RecordsUpdatedForSelectedStation;
            ViewModel.SelectedStation.GetRecordsCompleted += ViewModel.SelectedStation_GetRecordsCompleted;
            ViewModel.SelectedStation.SelectedStationChanged += ViewModel.SelectedStation_SelectedStationChanged;
            ViewModel.SelectedStation.ChangesMadeToSelectedStation += ViewModel.ChangesMadeToSelectedStation;

            ViewModel.LoadPosition();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OptionsOpened = false;
        }
    }
}