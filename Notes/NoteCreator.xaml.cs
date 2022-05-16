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
using Xceed.Wpf.Toolkit;

namespace Notes
{
    /// <summary>
    /// Interaction logic for NoteCreator.xaml
    /// </summary>
    public partial class NoteCreator : Window
    {
        public NoteCreator()
        {
            InitializeComponent();
        }

        private void BFinish_Click(object sender, RoutedEventArgs e)
        {
            NotesMenu.AddNote(CP_NoteColor.SelectedColor.ToString(), CP_TextColor.SelectedColor.ToString(), CP_XColor.SelectedColor.ToString());

            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BFinish_Click(sender, e);
            }
        }
    }
}
