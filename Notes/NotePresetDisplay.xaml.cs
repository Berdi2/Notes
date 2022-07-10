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

namespace Notes
{
    /// <summary>
    /// Interaction logic for NotePresetDisplay.xaml
    /// </summary>
    public partial class NotePresetDisplay : UserControl
    {
        public NotePresetDisplay(int Id_new)
        {
            InitializeComponent();

            Id = Id_new;
            TitleText = Methods.ClsDB.Get_string("SELECT Name FROM NoteColors WHERE Id = " + Id, "DBUser");
            ContentText = Methods.StringToFlowDoc(Methods.ClsDB.Get_string("SELECT Content FROM Notes WHERE Id = " + Id_new, "DBUser"));

            var bc = new BrushConverter();
            Brush NoteBrush = (Brush)bc.ConvertFrom(Methods.ClsDB.Get_string("SELECT NoteColor FROM Notes WHERE Id = " + Id_new, "DBUser"));
            Brush TextBrush = (Brush)bc.ConvertFrom(Methods.ClsDB.Get_string("SELECT TextColor FROM Notes WHERE Id = " + Id_new, "DBUser"));
            rect.Fill = NoteBrush;
            LTitle.Foreground = TextBrush;
            LContent.Foreground = TextBrush;
        }

        public string TitleText
        {
            get { return (string)LTitle.Content; }
            set { LTitle.Content = value; }
        }

        public FlowDocument ContentText
        {
            get { return LContent.Document; }
            set { LContent.Document = value; }
        }

        public int Id { get; set; }

        private void Button_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Methods.OpenNote(Id);
        }
    }
}
