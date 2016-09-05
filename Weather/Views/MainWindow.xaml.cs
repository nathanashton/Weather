using System;
using System.Windows;
using System.Windows.Threading;
using Weather.Common.Interfaces;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainWindowViewModel _viewModel;

        public ResourceDictionary ThemeDictionary
        {
            get { return Resources.MergedDictionaries[0]; }
        }

        public MainWindow(MainWindowViewModel viewModel, ISettings settings)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _viewModel.MainWindow = this;
            DataContext = _viewModel;
            Loaded += MainWindow_Loaded;
            DateTime sunrise = DateTime.Now; ;
            DateTime sunset = DateTime.Now;
            bool one = false;
            bool two = false; ;

            var t = SunTimes.Instance.CalculateSunRiseSetTimes(-27, 153, DateTime.Today, ref sunrise, ref sunset, ref one, ref two);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CreateDataGrid();
            var timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                _viewModel.Clock = DateTime.Now.ToString();
            }, Dispatcher);
        }

        private void CreateDataGrid()
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateDataGrid();
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            _viewModel.SelectedStation.BackOnePeriod();
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            _viewModel.SelectedStation.ForwardOnePeriod();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _viewModel.SelectedStation.TimeSpanDay = true;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            _viewModel.SelectedStation.TimeSpanWeek = true;
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            _viewModel.SelectedStation.TimeSpanMonth = true;
        }

        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            _viewModel.SelectedStation.TimeSpanYear = true;
        }
    }
}