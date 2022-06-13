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
        public NoteDisplay(int Id)
        {
            InitializeComponent();

            userControl.Id = Id;
        }

        public string TitleText
        {
            get { return (string)LTitle.Content; }
            set { LTitle.Content = value; }
        }

        public string ContentText
        {
            get { return (string)LContent.Text; }
            set { LContent.Text = value; }
        }

        public int Id { get; set; }

        private void Button_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NotesMenu.OpenNote(Id);
        }
    }
}
