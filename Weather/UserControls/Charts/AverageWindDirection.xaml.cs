using System.Windows;
using Microsoft.Practices.Unity;
using PropertyChanged;
using Weather.DependencyResolver;

namespace Weather.UserControls.Charts
{
    /// <summary>
    ///     Interaction logic for Test.xaml
    /// </summary>
    [ImplementPropertyChanged]
    public partial class AverageWindDirection
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
    }
}