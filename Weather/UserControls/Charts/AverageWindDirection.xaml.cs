using PropertyChanged;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Weather.DependencyResolver;
using Microsoft.Practices.Unity;


namespace Weather.UserControls.Charts
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    [ImplementPropertyChanged]
    public partial class AverageWindDirection : UserControl
    {

        public AverageWindDirectionViewModel ViewModel { get; set; }

        public AverageWindDirection()
        {
            InitializeComponent();
            var container = new Resolver().Bootstrap();
            ViewModel = container.Resolve<AverageWindDirectionViewModel>();
            DataContext = ViewModel;
            Loaded += Test_Loaded;
        }

        private void Test_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.GetCompatibleUnits();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
        }
    }
}