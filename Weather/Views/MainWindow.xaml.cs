using System;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Practices.Unity;
using Weather.Core.Interfaces;
using Weather.DependencyResolver;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly IWeatherRecordCore _core;
        private readonly MainWindowViewModel _viewModel;

        public ResourceDictionary ThemeDictionary => Resources.MergedDictionaries[0];

        public MainWindow(MainWindowViewModel viewModel, IWeatherRecordCore core)
        {
            _core = core;
            InitializeComponent();
            _viewModel = viewModel;
            _viewModel.MainWindow = this;
            DataContext = _viewModel;
            Loaded += MainWindow_Loaded;

            //var sunrise = DateTime.Now;
            //var sunset = DateTime.Now;
            //var one = false;
            //var two = false;
            //var t = SunTimes.Instance.CalculateSunRiseSetTimes(-27, 153, DateTime.Today, ref sunrise, ref sunset,
            //    ref one, ref two);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // ReSharper disable once UnusedVariable
            var dispatcherTimer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal,
                // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                delegate { _viewModel.Clock = DateTime.Now.ToString(); }, Dispatcher);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var container = new Resolver().Bootstrap();
            var window = container.Resolve<ImportWindow>();
            window.ShowDialog();
        }
    }
}