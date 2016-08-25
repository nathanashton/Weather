﻿using System;
using System.IO;
using Microsoft.Practices.Unity;
using Weather.Common.Interfaces;
using Weather.DependencyResolver;
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

            container.RegisterType<StationWindow>();
            container.RegisterType<StationWindowViewModel>();

            container.RegisterType<SensorWindow>();
            container.RegisterType<SensorsWindowViewModel>();


            container.RegisterType<ImportWindow>();
            container.RegisterType<ImportWindowViewModel>();


            container.RegisterType<UnitsWindow>();
            container.RegisterType<UnitsWindowViewModel>();


            container.RegisterType<UnitSelectorWindow>();
            container.RegisterType<UnitSelectorWindowViewModel>();


            container.RegisterType<SensorTypesWindow>();
            container.RegisterType<SensorTypesViewModel>();


  

            container.RegisterType<StationMapWindow>();

            var log = container.Resolve<ILog>();
            var settings = container.Resolve<ISettings>();
            log.Info("Application Started");
            RunApplication((UnityContainer) container, log, settings);
        }

        private static void RunApplication(UnityContainer container, ILog log, ISettings settings)
        {
            //try
            //{
            if (!Directory.Exists(settings.ApplicationPath))
            {
                Directory.CreateDirectory(settings.ApplicationPath);
            }

            log.SetDebugLevel();

            var application = new App();
            var mainWindow = container.Resolve<MainWindow>();
            application.Run(mainWindow);
            // }
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            //    log.Error("Unhandled exception", ex);
            //}
        }
    }
}