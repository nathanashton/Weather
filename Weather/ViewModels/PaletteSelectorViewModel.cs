using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using PropertyChanged;
using Weather.Helpers;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    class PaletteSelectorViewModel
    {
        public IEnumerable<Swatch> Swatches { get; }


        public PaletteSelectorViewModel()
        {
            Swatches = new SwatchesProvider().Swatches;
        }


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



        private static void ApplyBase(object obj)
        {
            var isDark = obj as bool?;
            if (isDark != null)
            {
                new PaletteHelper().SetLightDark((bool)isDark);
            }
        }


        private static void ApplyPrimary(object obj)
        {
            var swatch = obj as Swatch;
            if (swatch != null)
            {
                new PaletteHelper().ReplacePrimaryColor(swatch);
            }
        }

        private static void ApplyAccent(object obj)
        {
            var swatch = obj as Swatch;
            if (swatch != null)
            {
                new PaletteHelper().ReplaceAccentColor(swatch);
            }
        }
    }
}
