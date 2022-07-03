using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Notes
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();

            TBFontSize.Text = Properties.Settings.Default.FontSize.ToString();

            CBIsAlpha.IsChecked = Properties.Settings.Default.Alpha;
        }

        private void BUpdate_Click(object sender, RoutedEventArgs e)
        {
            NotesMenu.SearchForUpdate(false);
        }

        private static readonly Regex _regex = new Regex("[^0-9]"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void TBFontSize_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void BApply_Click(object sender, RoutedEventArgs e)
        {
            if (TBFontSize.Text != "")
            {
                Properties.Settings.Default.FontSize = int.Parse(TBFontSize.Text);
                Properties.Settings.Default.Save();
            }
            else
            {
                MessageBox.Show("Font Size can't be empty!", "Notes");
            }

            Properties.Settings.Default.Alpha = (bool)CBIsAlpha.IsChecked;
            Properties.Settings.Default.Save();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BApply_Click(sender, e);
            }
        }

        private void BChangelog_Click(object sender, RoutedEventArgs e)
        {
            Changelog changelog = new Changelog();
            changelog.Show();
        }
    }
}
