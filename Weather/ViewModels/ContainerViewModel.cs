using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using PropertyChanged;
using Weather.Common;
using Weather.Core.Interfaces;
using Weather.UserControls.Charts;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class ContainerViewModel : NotifyBase
    {
        private Chart _selected;
        public bool Loading { get; set; }
        public bool LoadingInvert { get; set; }
        public ObservableCollection<Chart> Charts { get; set; }

        public Chart Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                Content = _selected.Content;
                OnPropertyChanged(() => Selected);
            }
        }

        public ContentControl Content { get; set; }
        public ISelectedStation SelectedStation { get; set; }


        public ContainerViewModel(ISelectedStation selectedStation)
        {
            Loading = false;
            LoadingInvert = true;

            SelectedStation = selectedStation;
            SelectedStation.GetRecordsStarted += SelectedStation_GetRecordsStarted;
            SelectedStation.GetRecordsCompleted += SelectedStation_GetRecordsCompleted;

            Charts = new ObservableCollection<Chart>
            {
                //new Chart
                //{
                //    Name = "Average Wind Direction",
                //    Content = new AverageWindDirection()
                //},
                //new Chart
                //{
                //    Name = "Min / Max",
                //    Content = new MinMax()
                //},
                //new Chart
                //{
                //    Name = "All Records",
                //    Content = new AllRecords()
                //},
                new Chart
                {
                    Name = "Line Graph",
                    Content = new LineGraph()
                }
            };
        }

        private void SelectedStation_GetRecordsCompleted(object sender, EventArgs e)
        {
            Loading = false;
            LoadingInvert = true;
        }

        private void SelectedStation_GetRecordsStarted(object sender, EventArgs e)
        {
            if (Content == null)
            {
                return;
            }
            Loading = true;
            LoadingInvert = false;
        }
    }

    public class Chart
    {
        public string Name { get; set; }
        public ContentControl Content { get; set; }
    }
}