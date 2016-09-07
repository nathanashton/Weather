using System.Windows;
using System.Windows.Input;
using Microsoft.Maps.MapControl.WPF;

namespace Weather.Views
{
    /// <summary>
    ///     Interaction logic for StationMapWindow.xaml
    /// </summary>
    public partial class StationMapWindow
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public StationMapWindow()
        {
            InitializeComponent();
            Loaded += StationMapWindow_Loaded;
        }

        private void StationMapWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if ((Latitude != 0) && (Longitude != 0) && (Latitude != null) && (Longitude != null))
            {
                var pushpin = new Pushpin
                {
                    Location = new Location {Latitude = (double) Latitude, Longitude = (double) Longitude}
                };
                MyMap.Children.Add(pushpin);
                MyMap.Center = pushpin.Location;
            }
        }

        private void Map_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MyMap.Children.Count > 0)
            {
                for (var i = 0; i < MyMap.Children.Count; i++)
                {
                    MyMap.Children.RemoveAt(i);
                }
            }
            e.Handled = true;
            var mousePosition = e.GetPosition(this);
            var pinLocation = MyMap.ViewportPointToLocation(mousePosition);
            var pin = new Pushpin {Location = pinLocation};
            MyMap.Children.Add(pin);
            Latitude = pinLocation.Latitude;
            Longitude = pinLocation.Longitude;
        }
    }
}