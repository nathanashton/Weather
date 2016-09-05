using PropertyChanged;
using System.Windows.Controls;
using Weather.Common;
using Weather.Core.Interfaces;
using Weather.UserControls.Charts;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class ContainerViewModel : NotifyBase
    {
        private ComboBoxItem _selected;

        public ComboBoxItem Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                if (_selected.Tag.ToString() == "Average Wind Direction")
                {
                    Content = new AverageWindDirection();
                } else if (_selected.Tag.ToString() == "MinMax")
                {
                    Content = new MinMax();
                }
                else
                {
                    Content = null;
                }
                OnPropertyChanged(() => Selected);
            }
        }

        public ContentControl Content { get; set; }
        public ISelectedStation SelectedStation { get; set; }


        public ContainerViewModel(ISelectedStation selectedStation)
        {
            SelectedStation = selectedStation;
        }
    }
}