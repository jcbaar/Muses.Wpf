using Muses.Wpf.Controls;
using Muses.Wpf.Themes;
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

namespace Muses.TestControls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : BaseWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ResourceDictionary dict = new ResourceDictionary();

            if(Application.Current.Resources.MergedDictionaries[0].Source.ToString().Contains("Light.xaml"))
            {
                dict.Source = new Uri("pack://application:,,,/Muses.Wpf;component/Themes/Dark.xaml", UriKind.Absolute);
            }
            else
            {
                dict.Source = new Uri("pack://application:,,,/Muses.Wpf;component/Themes/Light.xaml", UriKind.Absolute);
            }

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ThemeHelper.SetAccentColor(Colors.DarkOrange, true);
        }

        private void CloseableTabControl_ClosingTabItem(object sender, RoutedEventArgs e)
        {
            var args = (ClosingTabEventArgs) e;
            var item = args.Source as CloseableTabItem;

            if (item != null && item.IsCloseable == false)
            {
                ((ClosingTabEventArgs)e).Cancel = true;
            }
        }
    }
}
