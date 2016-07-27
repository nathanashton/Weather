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
using System.Windows.Shapes;
using Microsoft.Maps.MapControl.WPF;

namespace Weather.Views
{
    /// <summary>
    /// Interaction logic for StationMapWindow.xaml
    /// </summary>
    public partial class StationMapWindow : Window
    {

        public double Latitude { get; set; }
        public double Longitude { get; set;}

        public StationMapWindow()
        {
            InitializeComponent();
            Loaded += StationMapWindow_Loaded;
        }

        private void StationMapWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Latitude != 0 && Longitude != 0)
            {
                var pushpin = new Pushpin();
                pushpin.Location = new Location {Latitude = Latitude, Longitude = Longitude};
                myMap.Children.Add(pushpin);
                myMap.Center = pushpin.Location;
                
            }
        }

        private void Map_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (myMap.Children.Count > 0)
            {
                for (var i= 0; i < myMap.Children.Count; i++)
                {
                    myMap.Children.RemoveAt(i);
                }
            }
            e.Handled = true;
            var mousePosition = e.GetPosition(this);
            var pinLocation = myMap.ViewportPointToLocation(mousePosition);
            var pin = new Pushpin {Location = pinLocation};
            myMap.Children.Add(pin);
            Latitude = pinLocation.Latitude;
            Longitude = pinLocation.Longitude;
        }
    }
}
