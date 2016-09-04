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
    public partial class Test : UserControl
    {

        public TestViewModel ViewModel { get; set; }

        public Test()
        {
            InitializeComponent();
            var container = new Resolver().Bootstrap();
            ViewModel = container.Resolve<TestViewModel>();
            DataContext = ViewModel;

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
        }
    }
}