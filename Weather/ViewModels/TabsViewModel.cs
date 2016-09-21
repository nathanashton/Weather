using System.Windows.Controls;
using PropertyChanged;
using Weather.Common;

namespace Weather.ViewModels
{
    public interface ITabsViewModel
    {
        ContentControl Dialog { get; set; }
        void Update();
    }

    [ImplementPropertyChanged]
    public class TabsViewModel : NotifyBase, ITabsViewModel
    {
        private ContentControl _dialog;

        public ContentControl Dialog
        {
            get { return _dialog; }
            set
            {
                _dialog = value;
                OnPropertyChanged(() => Dialog);
            }
        }

        public void Update()
        {
            OnPropertyChanged(() => Dialog);
        }
    }
}