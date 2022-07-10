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

            Methods.UpdateCBPresets(CBPresets);
        }

        private void BFinish_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)CBPresets.SelectedItem;
            if (item.Tag == "None")
            {
                Methods.AddNote(CP_NoteColor.SelectedColor.ToString(), CP_TextColor.SelectedColor.ToString(), CP_XColor.SelectedColor.ToString());
            }
            else
            {
                string[] Tag = (string[])item.Tag;
                string SQL = " FROM NoteColors WHERE Id = '" + Tag[0] + "'";

                string NoteColor = Methods.ClsDB.Get_string("SELECT NoteColor" + SQL, Tag[1]);
                string TextColor = Methods.ClsDB.Get_string("SELECT TextColor" + SQL, Tag[1]);
                string XColor = Methods.ClsDB.Get_string("SELECT XColor" + SQL, Tag[1]);

                Methods.AddNote(NoteColor, TextColor, XColor);
            }
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BFinish_Click(sender, e);
            }
        }

        private void CBPresets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Methods.CBPresets_SelectionChangedHandler(CBPresets, CP_NoteColor, CP_TextColor, CP_XColor);
        }
    }
}
