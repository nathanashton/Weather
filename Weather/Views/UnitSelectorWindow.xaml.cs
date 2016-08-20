using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Weather.ViewModels;

namespace Weather.Views
{
    /// <summary>
    /// Interaction logic for UnitSelectorWindow.xaml
    /// </summary>
    public partial class UnitSelectorWindow : Window
    {

        public UnitSelectorWindowViewModel _viewModel { get; set; }

        public UnitSelectorWindow(UnitSelectorWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }
    }
}
