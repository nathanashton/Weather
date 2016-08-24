using System.Windows;
using System.Windows.Input;
using Microsoft.Maps.MapControl.WPF;

namespace Weather.Views
{
    /// <summary>
    ///     Interaction logic for StationMapWindow.xaml
    /// </summary>
    public partial class StationMapWindow : Window
    {
        public StationMapWindow()
        {
            InitializeComponent();
            Loaded += StationMapWindow_Loaded;
        }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        private void StationMapWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Latitude != 0 && Longitude != 0 && Latitude != null && Longitude != null)
            {
                var pushpin = new Pushpin();
                pushpin.Location = new Location {Latitude = (double) Latitude, Longitude = (double) Longitude};
                myMap.Children.Add(pushpin);
                myMap.Center = pushpin.Location;
            }
        }

        private void Map_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (myMap.Children.Count > 0)
            {
                for (var i = 0; i < myMap.Children.Count; i++)
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