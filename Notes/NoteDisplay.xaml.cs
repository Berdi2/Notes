using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class NoteDisplay : UserControl
    {
        public NoteDisplay(int Id_new)
        {
            InitializeComponent();

            userControl.Id = Id_new;
            userControl.TitleText = Methods.ClsDB.String("SELECT Title FROM Notes WHERE Id = " + Id_new, "DBUser");
            userControl.ContentText = Methods.StringToFlowDoc(Methods.ClsDB.String("SELECT Content FROM Notes WHERE Id = " + Id_new, "DBUser"));

            var bc = new BrushConverter();
            Brush NoteBrush = (Brush)bc.ConvertFrom(Methods.ClsDB.String("SELECT NoteColor FROM Notes WHERE Id = '" + Id_new + "'", "DBUser"));
            Brush TextBrush = (Brush)bc.ConvertFrom(Methods.ClsDB.String("SELECT TextColor FROM Notes WHERE Id = '" + Id_new + "'", "DBUser"));
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
            NotesMenu.OpenNote(Id);
        }
    }
}
