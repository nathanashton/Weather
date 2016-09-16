using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Weather.UserControls
{
    /// <summary>
    /// Interaction logic for Tabs.xaml
    /// </summary>
    public partial class Tabs : UserControl
    {
        public Tabs()
        {
            InitializeComponent();
        }

        private void SensorTypes_Loaded(object sender, RoutedEventArgs e)
        {
           
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
