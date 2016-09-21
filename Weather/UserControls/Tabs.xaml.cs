using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using PropertyChanged;
using Weather.DependencyResolver;
using Weather.ViewModels;

namespace Weather.UserControls
{
    /// <summary>
    ///     Interaction logic for Tabs.xaml
    /// </summary>
    [ImplementPropertyChanged]
    public partial class Tabs : UserControl
    {
        public Tabs()
        {
            InitializeComponent();
            var container = new Resolver().Bootstrap();
            ITabsViewModel viewModel = container.Resolve<TabsViewModel>();
            DataContext = viewModel;
        }

        private void SensorTypes_Selected(object sender, RoutedEventArgs e)
        {
            var tab = sender as TabItem;
            if (tab != null)
            {
                var content = new SensorTypes
                {
                    Width = 800,
                    Height = 600,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                tab.Content = content;
            }
        }

        private void TabItem_Selected(object sender, RoutedEventArgs e)
        {
            var tab = sender as TabItem;
            if (tab != null)
            {
                var content = new Stations
                {
                    Width = 800,
                    Height = 600,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                tab.Content = content;
            }
        }

        private void TabItem_Selected_1(object sender, RoutedEventArgs e)
        {
            var tab = sender as TabItem;
            if (tab != null)
            {
                var content = new Sensors
                {
                    Width = 800,
                    Height = 600,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                tab.Content = content;
            }
        }

        private void TabItem_Selected_2(object sender, RoutedEventArgs e)
        {
            var tab = sender as TabItem;
            if (tab != null)
            {
                var content = new PaletteSelector
                {
                    Width = 900,
                    Height = 800,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                tab.Content = content;
            }
        }
    }
}