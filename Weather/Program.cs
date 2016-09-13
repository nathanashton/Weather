using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Unity;
using Weather.Common;
using Weather.Common.Interfaces;
using Weather.DependencyResolver;
using Weather.UserControls;
using Weather.ViewModels;
using Weather.Views;

namespace Weather
{
    internal static class Program
    {
        public static ObservableCollection<IPluginWrapper> LoadedPlugins;

        [STAThread]
        private static void Main()
        {
            LoadFromPath("Plugins", true);

            var container = new Resolver().Bootstrap();
            container.RegisterType<MainWindow>();
            container.RegisterType<MainWindowViewModel>();

            container.RegisterType<SensorsWindowViewModel>();

            container.RegisterType<SensorSelectWindowViewModel>();
            container.RegisterType<SensorSelectWindow>();

            container.RegisterType<ImportWindow>();
            container.RegisterType<ImportWindowViewModel>();


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

            container.RegisterType<SelectStationWindow>();
            container.RegisterType<SelectStationWindowViewModel>();

            container.RegisterType<StationMapWindow>();

            var log = container.Resolve<ILog>();
            var settings = container.Resolve<ISettings>();
            log.Info("Application Started");
            RunApplication((UnityContainer) container, log, settings);
        }

        private static void RunApplication(UnityContainer container, ILog log, ISettings settings)
        {
            TextOptions.TextFormattingModeProperty.OverrideMetadata(typeof(Window),
                new FrameworkPropertyMetadata(TextFormattingMode.Display,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.Inherits));

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

        public static void LoadFromPath(string path, bool includeSubdirectories = false)
        {
            var container = new Resolver().Bootstrap();
            var station = container.Resolve<ISelectedStation>();

            if (!Directory.Exists(path))
            {
                return;
            }
            var dllFileNames = Directory.GetFiles(path, "*.dll",
                includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
            foreach (var dllFile in dllFileNames)
            {
                try
                {
                    var an = AssemblyName.GetAssemblyName(dllFile);
                    var assembly = Assembly.Load(an);
                    assemblies.Add(assembly);
                }
                catch (Exception)
                {
                    //TODO
                }
            }

            var pluginType = typeof(IPlugin);
            ICollection<Type> pluginTypes = new List<Type>();
            foreach (var assembly in assemblies)
            {
                if (assembly == null)
                {
                    continue;
                }
                var types = assembly.GetTypes();

                foreach (var t in types)
                {
                    if (t.IsInterface || t.IsAbstract)
                    {
                    }
                    else
                    {
                        var n = t.GetInterface(pluginType.FullName);
                        if (n != null)
                        {
                            pluginTypes.Add(t);
                        }
                    }
                }
            }

            LoadedPlugins = new ObservableCollection<IPluginWrapper>();

            foreach (var type in pluginTypes)
            {
                var plugin = Activator.CreateInstance(type, station);

                var name = type.GetCustomAttributes(typeof(DisplayNameAttribute), false)[0].ToString();
                var description = type.GetCustomAttributes(typeof(DescriptionAttribute), false)[0].ToString();

                var wrappedPlugin = new PluginWrapper(plugin as IPlugin, name, description);
                LoadedPlugins.Add(wrappedPlugin);
            }
        }
    }
}