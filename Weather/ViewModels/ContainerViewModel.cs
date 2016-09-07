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
            SelectedStation = selectedStation;
            Charts = new ObservableCollection<Chart>
            {
                new Chart
                {
                    Name = "Average Wind Direction",
                    Content = new AverageWindDirection()
                },
                new Chart
                {
                    Name = "Min / Max",
                    Content = new MinMax()
                },
                new Chart
                {
                    Name = "All Records",
                    Content = new AllRecords()
                },
                            new Chart
                {
                    Name = "Line Graph",
                    Content = new LineGraph()
                }
            };
        }
    }

    public class Chart
    {
        public string Name { get; set; }
        public ContentControl Content { get; set; }
    }
}