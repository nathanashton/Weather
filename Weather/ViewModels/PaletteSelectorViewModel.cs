using System.Collections.Generic;
using System.Windows.Input;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using Microsoft.Practices.Unity;
using PropertyChanged;
using Weather.Common;
using Weather.Common.Interfaces;
using Weather.DependencyResolver;
using Weather.Helpers;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class PaletteSelectorViewModel
    {
        private static ISettings _settings;
        public IEnumerable<Swatch> Swatches { get; }


        public ICommand ToggleBaseCommand
        {
            get { return new RelayCommand(ApplyBase, x => true); }
        }


        public ICommand ApplyPrimaryCommand
        {
            get { return new RelayCommand(ApplyPrimary, x => true); }
        }

        public ICommand ApplyAccentCommand
        {
            get { return new RelayCommand(ApplyAccent, x => true); }
        }


        public PaletteSelectorViewModel()
        {
            Swatches = new SwatchesProvider().Swatches;
            var container = new Resolver().Bootstrap();
            _settings = container.Resolve<Settings>();
        }


        private static void ApplyBase(object obj)
        {
            var isDark = obj as bool?;
            if (isDark != null)
            {
                new PaletteHelper().SetLightDark((bool) isDark);
                _settings.IsDark = (bool) isDark;
                _settings.Save();
            }
        }


        private static void ApplyPrimary(object obj)
        {
            var swatch = obj as Swatch;
            if (swatch != null)
            {
                new PaletteHelper().ReplacePrimaryColor(swatch);
                _settings.PrimaryColor = swatch.Name;
                _settings.Save();
            }
        }

        private static void ApplyAccent(object obj)
        {
            var swatch = obj as Swatch;
            if (swatch != null)
            {
                new PaletteHelper().ReplaceAccentColor(swatch);
                _settings.AccentColor = swatch.Name;
                _settings.Save();
            }
        }
    }
}