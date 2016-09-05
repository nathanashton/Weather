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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Weather.DependencyResolver;
using Microsoft.Practices.Unity;


namespace Weather.UserControls.Charts
{
    /// <summary>
    /// Interaction logic for MinMax.xaml
    /// </summary>
    public partial class MinMax : UserControl
    {

        public MinMaxViewModel ViewModel { get; set; }

        public MinMax()
        {
            InitializeComponent();
            var container = new Resolver().Bootstrap();
            ViewModel = container.Resolve<MinMaxViewModel>();
            DataContext = ViewModel;
            Loaded += MinMax_Loaded;
        }

        private void MinMax_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
