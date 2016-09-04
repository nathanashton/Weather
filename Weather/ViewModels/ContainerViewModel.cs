using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Weather.Common;
using Weather.UserControls.Charts;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class ContainerViewModel : NotifyBase
    {
        private ComboBoxItem _selected;
        public ComboBoxItem Selected
        {
            get { return _selected; }
            set { _selected = value;
                if (_selected.Tag.ToString() == "Average Wind Direction")
                {
                    Content = new Test();
                } else
                {
                    Content = null;
                }
                OnPropertyChanged(() => Selected);
            }
        }
        public ContentControl Content { get; set; }
    }
}
