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
    /// Interaction logic for AddPreset.xaml
    /// </summary>
    public partial class AddPreset : Window
    {
        ComboBox CBPresets;
        public AddPreset(ComboBox CBPresets_neu)
        {
            InitializeComponent();

            CBPresets = CBPresets_neu;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BFinish_Click(sender, e);
            }
        }

        private void BFinish_Click(object sender, RoutedEventArgs e)
        {
            if (TBName.Text == "")
            {
                MessageBox.Show("Please provide a name!", "Notes");
                return;
            }

            int Id = Methods.NullToZero(Methods.ClsDB.Get_object("SELECT MAX(Id) FROM NoteColors", "DBUser")) + 1;
            Methods.ClsDB.Execute_SQL("INSERT INTO NoteColors VALUES('" + Id + "','" + TBName.Text + "','" + CP_NoteColor.SelectedColor.ToString() + "','" + CP_TextColor.SelectedColor.ToString() + "','" + CP_XColor.SelectedColor.ToString() + "')", "DBUser");
            Methods.UpdateCBPresets(CBPresets);
            Close();
        }
    }
}
