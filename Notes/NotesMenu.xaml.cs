using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Collections;
using System.Threading;
using System.Globalization;
using nUpdate.Updating;
using System.Windows.Markup;
using System.IO;
using nUpdate.UI.WPF;

namespace Notes
{
    /// <summary>
    /// Interaction logic for NotesMenu.xaml
    /// </summary>
    public partial class NotesMenu : Window
    {
        public NotesMenu()
        {
            InitializeComponent();

            Methods.UpdateListView();

            Methods.SearchForUpdate(true);
        }

        public static NotesMenu NM()
        {
            return Application.Current.Windows.OfType<NotesMenu>().FirstOrDefault();
        }

        public static void LVItem_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Methods.OpenNote_LVSelectedItems();
        }

        private void BAddNote_Click(object sender, RoutedEventArgs e)
        {
            NoteCreator noteCreator = new NoteCreator();
            noteCreator.Show();
        }

        private static void Delete_Click(object sender, RoutedEventArgs e)
        {
            Methods.DeleteNotes(SelectedItems: true);
        }

        private void LSettings_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                Methods.DeleteNotes(SelectedItems: true);
            }

            if (e.Key == Key.Enter)
            {
                Methods.OpenNote_LVSelectedItems();
            }
        }

        private static void Open_Click(object sender, RoutedEventArgs e)
        {
            Methods.OpenNote_LVSelectedItems();
        }

        public static void LVItem_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContextMenu CM = new ContextMenu();
            MenuItem MIOpen = new MenuItem
            {
                Header = "Open"
            };
            MIOpen.Click += Open_Click;
            CM.Items.Add(MIOpen);
            MenuItem MIDelete = new MenuItem
            {
                Header = "Delete"
            };
            MIDelete.Click += Delete_Click;
            CM.Items.Add(MIDelete);
            CM.IsOpen = true;
        }
    }
}
