using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using Weather.DependencyResolver;
using Weather.ViewModels;

namespace Weather.UserControls
{
    /// <summary>
    ///     Interaction logic for ImportControl.xaml
    /// </summary>
    public partial class ImportControl : UserControl
    {
        private readonly ImportWindowViewModel _viewModel;


        public ImportControl()
        {
            InitializeComponent();
            var container = new Resolver().Bootstrap();
            _viewModel = container.Resolve<ImportWindowViewModel>();
            DataContext = _viewModel;
            Loaded += ImportControl_Loaded;
        }

        private void ImportControl_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.GetSensors();

        }

        private void IntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var date = 0;
            var time = 0;
            if ((_viewModel.SelectedRecordDate != null) && (_viewModel.SelectedRecordTime != null))
            {
                date = _viewModel.SelectedRecordDate.Index;
                time = _viewModel.SelectedRecordTime.Index;
            }
            if ((_viewModel.Records != null) && (_viewModel.CurrentRecord != 0))
            {
                _viewModel.Record = _viewModel.FilteredRecords[_viewModel.CurrentRecord - 1];
                _viewModel.DateRecord = _viewModel.FilteredDateRecords[_viewModel.CurrentRecord - 1];
                // fails if file has headers

                try
                {
                    _viewModel.SelectedRecordDate = _viewModel.DateRecord[date];
                    _viewModel.SelectedRecordTime = _viewModel.DateRecord[time];
                }
                catch
                {
                }
            }
        }

        private void IntegerUpDown_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _viewModel.FilteredRecords =
                new ObservableCollection<ObservableCollection<Record>>(
                    _viewModel.Records.ToList()
                        .GetRange(_viewModel.ExcludeLineCount, _viewModel.Records.Count - _viewModel.ExcludeLineCount));
            _viewModel.FilteredDateRecords =
                new ObservableCollection<ObservableCollection<Record>>(
                    _viewModel.DateRecords.ToList()
                        .GetRange(_viewModel.ExcludeLineCount,
                            _viewModel.DateRecords.Count - _viewModel.ExcludeLineCount));

            IntegerUpDown_ValueChanged(null, null);
        }
    }
}