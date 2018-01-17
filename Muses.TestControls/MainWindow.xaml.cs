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
        Random _rnd = new Random();

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
            // Generate a random color...
            var color = Color.FromArgb(255,
                (byte)_rnd.Next(0, 255),
                (byte)_rnd.Next(0, 255),
                (byte)_rnd.Next(0, 255));

            SysAccent.IsChecked = false;

            ThemeHelper.SetAccentColor(color, true);
        }

        private void CloseableTabControl_ClosingTabItem(object sender, RoutedEventArgs e)
        {
            var args = (ClosingTabEventArgs) e;
            var item = args.TabItem;

            if (item != null)
            {
                if (item.IsCloseable == false)
                {
                    ((ClosingTabEventArgs)e).Cancel = true;
                }
                else if(item == CantClose)
                {
                    MessageBox.Show("Oops. Can't close this one...", "No no no", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    ((ClosingTabEventArgs)e).Cancel = true;
                }
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var tb = sender as TextBox;
                if (tb != null && tb.Text.Trim().Length > 0)
                {
                    MessageBox.Show($"You have entered \"{tb.Text}\" as search term.", "Hi", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var ch = sender as CheckBox;
            if (ch != null) ThemeHelper.UseSystemAccentColor = ch.IsChecked.HasValue && ch.IsChecked.Value;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
