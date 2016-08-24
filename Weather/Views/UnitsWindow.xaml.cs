using PropertyChanged;
using System.Windows;
using System.Windows.Controls;
using Weather.Common.Units;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    /// Interaction logic for UnitsWindow.xaml
    /// </summary>
    [ImplementPropertyChanged]
    public partial class UnitsWindow : Window
    {
        private readonly UnitsWindowViewModel _viewModel;

        public UnitsWindow(UnitsWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _viewModel.Window = this;
            DataContext = _viewModel;
            Loaded += UnitsWindow_Loaded;
        }

        private void UnitsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.GetAll();
            _viewModel.RegisterDirtyHandlers();
        }

        private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selection = ((ListBox)e.Source).SelectedItem;
            if (selection == null) return;
            _viewModel.Unit = ((ListBox)e.Source).SelectedItem as Unit;
            _viewModel.SelectedUnit = new Unit
            {
                UnitId = _viewModel.Unit.UnitId,
                DisplayName = _viewModel.Unit.DisplayName,
                DisplayUnit = _viewModel.Unit.DisplayUnit,
                UnitType = _viewModel.Unit.UnitType
            };
        }

        public void SelectUnitInListBox(Unit unit)
        {
            lb.SelectedItem = unit;
        }
    }
}