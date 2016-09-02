using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Weather.Common.Entities;
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

        private void MenuItemWithRadioButtons_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                RadioButton rb = mi.Icon as RadioButton;
                if (rb != null)
                {
                    rb.IsChecked = true;
                }
            }
        }

        private void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var weatherStation = ((ComboBox)e.Source).SelectedItem as WeatherStation;
            //if (weatherStation == null) return;
            //_viewModel.SelectedStation = weatherStation;
            //CreateDataGrid();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Program.ChangeTheme(new Uri("/Skins/Dark.xaml", UriKind.Relative));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Program.ChangeTheme(new Uri("/Skins/Light.xaml", UriKind.Relative));
        }
    }
}