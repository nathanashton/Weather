using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Weather.Common.Interfaces;
using Weather.Helpers;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class OptionsWindowViewModel
    {

        public ObservableCollection<string> Skins { get; set; }
        public string SelectedSkin { get; set; }

        private ISettings _settings;

        public OptionsWindowViewModel(ISettings settings)
        {
            _settings = settings;
            _settings.Load();
            Skins = new ObservableCollection<string> { "Dark", "Light" };
            Load();
        }

        public ICommand SaveCommand
        {
            get { return new RelayCommand(Save, x => true); }
        }

        private void Save(object obj)
        {
            if (SelectedSkin == "Dark")
            {
                Program.ChangeTheme(new Uri("/Skins/Dark.xaml", UriKind.Relative));
            }
            else if (SelectedSkin == "Light")
            {
                Program.ChangeTheme(new Uri("/Skins/Light.xaml", UriKind.Relative));
            }

            _settings.Skin = SelectedSkin;
            _settings.Save();
        }

        private void Load()
        {
            _settings.Load();
            SelectedSkin = _settings.Skin;
        }

    }
}
