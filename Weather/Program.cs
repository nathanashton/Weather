using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using Microsoft.Practices.Unity;
using Weather.Common.Interfaces;
using Weather.DependencyResolver;
using Weather.UserControls;
using Weather.UserControls.Charts;
using Weather.ViewModels;
using Weather.Views;

namespace Weather
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            var container = new Resolver().Bootstrap();
            container.RegisterType<MainWindow>();
            container.RegisterType<MainWindowViewModel>();

            container.RegisterType<SensorsWindowViewModel>();

            container.RegisterType<SensorSelectWindowViewModel>();
            container.RegisterType<SensorSelectWindow>();

            container.RegisterType<ImportWindow>();
            container.RegisterType<ImportWindowViewModel>();

            container.RegisterType<UnitsWindow>();
            container.RegisterType<UnitsWindowViewModel>();

            container.RegisterType<UnitSelectorWindow>();
            container.RegisterType<UnitSelectorWindowViewModel>();

            container.RegisterType<SensorTypesWindow>();
            container.RegisterType<SensorTypesViewModel>();

            container.RegisterType<StationsWindow>();
            container.RegisterType<StationsWindowViewModel>();

            container.RegisterType<UnhandledExceptionWindow>();
            container.RegisterType<UnhandledExceptionWindowViewModel>();

            container.RegisterType<OptionsWindow>();
            container.RegisterType<OptionsWindowViewModel>();

            container.RegisterType<StationPanelViewModel>();
            container.RegisterType<StationSidePanel>();
            container.RegisterType<Container>();
            container.RegisterType<ContainerViewModel>();


            // Charts
            container.RegisterType<AverageWindDirectionViewModel>();
            container.RegisterType<LineGraphViewModel>();

            container.RegisterType<MinMaxViewModel>();

            container.RegisterType<AllRecordsViewModel>();


            container.RegisterType<StationMapWindow>();

            var log = container.Resolve<ILog>();
            var settings = container.Resolve<ISettings>();
            log.Info("Application Started");
            RunApplication((UnityContainer) container, log, settings);
        }

        private static void RunApplication(UnityContainer container, ILog log, ISettings settings)
        {
            if (!Directory.Exists(settings.ApplicationPath))
            {
                Directory.CreateDirectory(settings.ApplicationPath);
            }
            if (!Directory.Exists(settings.ErrorPath))
            {
                Directory.CreateDirectory(settings.ErrorPath);
            }


            log.SetDebugLevel();

            var application = new App();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var mainWindow = container.Resolve<MainWindow>();
            application.InitializeComponent();

            settings.Load();
            if (settings.Skin == "Dark")
            {
                ChangeTheme(new Uri("/Skins/Dark.xaml", UriKind.Relative));
            }
            else if (settings.Skin == "Light")
            {
                ChangeTheme(new Uri("/Skins/Light.xaml", UriKind.Relative));
            }

            application.Run(mainWindow);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var container = new Resolver().Bootstrap();
            var settings = container.Resolve<ISettings>();

            var image = ScreenCapture.CaptureActiveWindow();
            image.Save(Path.Combine(settings.ErrorPath, "unhandledexception.jpg"), ImageFormat.Jpeg);

            var ex = e.ExceptionObject as Exception;

            var window = container.Resolve<UnhandledExceptionWindow>();

            if (ex != null)
            {
                window.ViewModel.Message = "\"" + ex.Message + "\"";
                window.ViewModel.StackTrace = ex.StackTrace;
                window.ViewModel.Source = ex.Source;
            }

            window.ShowDialog();

            // Environment.Exit(1);
        }

        public static void ChangeTheme(Uri uri)
        {
            var resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }
    }
}