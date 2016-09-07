using System;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Microsoft.Practices.Unity;
using PropertyChanged;
using Weather.Common;
using Weather.Common.EventArgs;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.DependencyResolver;
using Weather.Helpers;
using Weather.Views;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel : NotifyBase
    {
        private bool _debugPanelVisible;

        public bool InfoPanelOneVisible { get; set; }
        public bool InfoPanelTwoVisible { get; set; }
        public bool InfoPanelThreeVisible { get; set; }
        public bool InfoPanelFourVisible { get; set; }

        public bool SideBarVisible { get; set; }
        public bool WeatherChartOneVisible { get; set; }
        public bool WeatherChartTwoVisible { get; set; }

        public bool DebugPanelVisible
        {
            get { return _debugPanelVisible; }
            set
            {
                _debugPanelVisible = value;
                if (value)
                {
                    DebugPanel = "";
                    var storyboard = MainWindow.Resources["ShowDebugPanel"] as Storyboard;
                    storyboard?.Begin();
                }
                else
                {
                    var storyboard = MainWindow.Resources["CloseDebugPanel"] as Storyboard;
                    storyboard?.Begin();
                }
                OnPropertyChanged(() => DebugPanelVisible);
            }
        }

        public ISelectedStation SelectedStation { get; set; }

        public string Clock { get; set; }
        public MainWindow MainWindow { get; set; }
        public string DebugPanel { get; set; }

        public ICommand SensorsWindowCommand
        {
            get { return new RelayCommand(SensorsWindowOpen, x => true); }
        }

        public ICommand WeatherStationsCommand
        {
            get { return new RelayCommand(StationsWindowOpen, x => true); }
        }

        public ICommand PeriodBackCommand
        {
            get { return new RelayCommand(PeriodBack, x => SelectedStation.WeatherStation != null); }
        }


        public ICommand PeriodForwardCommand
        {
            get { return new RelayCommand(PeriodForward, x => SelectedStation.WeatherStation != null); }
        }

        public ICommand ClearDebugCommand
        {
            get { return new RelayCommand(ClearDebug ,x=> true); }
        }

        private void ClearDebug(object obj)
        {
            DebugPanel = String.Empty;
        }

        public ICommand SensorTypesCommand
        {
            get { return new RelayCommand(SensorTypes, x => true); }
        }

        public ICommand ThrowException
        {
            get { return new RelayCommand(Throw, x => true); }
        }

        public ICommand TimeSpanCommandDay
        {
            get { return new RelayCommand(UpdateTimeSpanDay, x => SelectedStation.WeatherStation != null); }
        }

        public ICommand TimeSpanCommandWeek
        {
            get { return new RelayCommand(UpdateTimeSpanWeek, x => SelectedStation.WeatherStation != null); }
        }

        public ICommand TimeSpanCommandMonth
        {
            get { return new RelayCommand(UpdateTimeSpanMonth, x => SelectedStation.WeatherStation != null); }
        }

        public ICommand TimeSpanCommandYear
        {
            get { return new RelayCommand(UpdateTimeSpanYear, x => SelectedStation.WeatherStation != null); }
        }

        public ICommand OptionsWindowCommand
        {
            get { return new RelayCommand(ShowOptions, x => true); }
        }

        public ICommand StationsCommand
        {
            get { return new RelayCommand(OpenStations, x => true); }
        }

        public ICommand UnitsWindowCommand
        {
            get { return new RelayCommand(UnitsWindowOpen, x => true); }
        }

        public ICommand SensorTypesWindowCommand
        {
            get { return new RelayCommand(SensorTypesWindowOpen, x => true); }
        }

        public MainWindowViewModel(ILog log,
            ISelectedStation selectedStation)
        {
            SelectedStation = selectedStation;
            log.DebugPanelMessage += _log_DebugPanelMessage;
            SideBarVisible = true;
        }

        private void PeriodBack(object obj)
        {
            SelectedStation.BackOnePeriod();
        }

        private void PeriodForward(object obj)
        {
            SelectedStation.ForwardOnePeriod();
        }

        private void _log_DebugPanelMessage(object sender, DebugMessageArgs e)
        {
            if (!DebugPanelVisible)
            {
                return;
            }
            DebugPanel += DateTime.Now + " => " + e.Message + Environment.NewLine;
            MainWindow.Scroll.ScrollToBottom();
        }

        private void UpdateTimeSpanDay(object obj)
        {
            SelectedStation.SetTimeSpanDay();
        }

        private void UpdateTimeSpanWeek(object obj)
        {
            SelectedStation.SetTimeSpanWeek();
        }

        private void UpdateTimeSpanMonth(object obj)
        {
            SelectedStation.SetTimeSpanMonth();
        }

        private void UpdateTimeSpanYear(object obj)
        {
            SelectedStation.SetTimeSpanYear();
        }

        private void Throw(object obj)
        {
            throw new NullReferenceException("Unahnded");
        }

        private void ShowOptions(object obj)
        {
            var container = new Resolver().Bootstrap();
            var window = container.Resolve<OptionsWindow>();
            window.ShowDialog();
        }

        private void OpenStations(object obj)
        {
        }

        private void UnitsWindowOpen(object obj)
        {
            var container = new Resolver().Bootstrap();
            var window = container.Resolve<UnitsWindow>();
            window.ShowDialog();
        }

        private void StationsWindowOpen(object obj)
        {
            var container = new Resolver().Bootstrap();
            var window = container.Resolve<StationsWindow>();
            window.ShowDialog();
        }

        private void SensorsWindowOpen(object obj)
        {
            var container = new Resolver().Bootstrap();
            var window = container.Resolve<SensorsWindow>();
            window.ShowDialog();
        }


        private void SensorTypes(object obj)
        {
            //var container = new Resolver().Bootstrap();
            //var window = container.Resolve<SensorTypesWindow>();
            //window.ShowDialog();
        }

        private void SensorTypesWindowOpen(object obj)
        {
            var container = new Resolver().Bootstrap();
            var window = container.Resolve<SensorTypesWindow>();
            window.ShowDialog();
        }

        public void ClearAll()
        {
            //  _database.ClearAll();
        }
    }
}