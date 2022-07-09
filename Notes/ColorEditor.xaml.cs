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

namespace Notes
{
    /// <summary>
    /// Interaction logic for ColorEditor.xaml
    /// </summary>
    public partial class ColorEditor : Window
    {
        int Id = 0;
        Note note;

        public ColorEditor(int Id_new, Note note_new)
        {
            InitializeComponent();

            Id = Id_new;
            note = note_new;

            CP_NoteColor.SelectedColor = (Color)ColorConverter.ConvertFromString(Methods.ClsDB.String("SELECT NoteColor FROM Notes WHERE Id = '" + Id + "'", "DBUser"));
            CP_TextColor.SelectedColor = (Color)ColorConverter.ConvertFromString(Methods.ClsDB.String("SELECT TextColor FROM Notes WHERE Id = '" + Id + "'", "DBUser"));
            CP_XColor.SelectedColor = (Color)ColorConverter.ConvertFromString(Methods.ClsDB.String("SELECT XColor FROM Notes WHERE Id = '" + Id + "'", "DBUser"));
        }

        private void BFinish_Click(object sender, RoutedEventArgs e)
        {
            NotesMenu.ChangeColor(Id, CP_NoteColor.SelectedColor.ToString(), CP_TextColor.SelectedColor.ToString(), CP_XColor.SelectedColor.ToString());

            note.Close();

            NotesMenu.OpenNote(Id);

            Close();
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
