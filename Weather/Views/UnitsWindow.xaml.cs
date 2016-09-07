using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PropertyChanged;
using Weather.Common.Units;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    ///     Interaction logic for UnitsWindow.xaml
    /// </summary>
    [ImplementPropertyChanged]
    public partial class UnitsWindow
    {
        private readonly UnitsWindowViewModel _viewModel;

        public UnitsWindow(UnitsWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _viewModel.Window = this;
            DataContext = _viewModel;
            Loaded += UnitsWindow_Loaded;
            Closing += UnitsWindow_Closing;
        }

        private void UnitsWindow_Closing(object sender, CancelEventArgs e)
        {
            _viewModel.SelectedStation.OnStationsChanged();
        }

        private void UnitsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.GetAll();
            _viewModel.RegisterDirtyHandlers();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selection = ((ListBox) e.Source).SelectedItem;
            if (selection == null)
            {
                return;
            }
            _viewModel.Unit = ((ListBox) e.Source).SelectedItem as Unit;
            if (_viewModel.Unit != null)
            {
                _viewModel.SelectedUnit = new Unit
                {
                    UnitId = _viewModel.Unit.UnitId,
                    DisplayName = _viewModel.Unit.DisplayName,
                    DisplayUnit = _viewModel.Unit.DisplayUnit,
                    UnitType = _viewModel.Unit.UnitType
                };
            }
        }

        public void SelectUnitInListBox(Unit unit)
        {
            Lb.SelectedItem = unit;
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}