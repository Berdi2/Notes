using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace Notes
{
    /// <summary>
    /// Interaction logic for Note.xaml
    /// </summary>
    public partial class Note : Window
    {
        int Id = 0;
        bool isCollapsed = false;

        public Note(int Id_new)
        {
            InitializeComponent();

            Id = Id_new;

            var bc = new BrushConverter();
            Brush NoteBrush = (Brush)bc.ConvertFrom(NotesMenu.ClsDB.String("SELECT NoteColor FROM Notes WHERE Id = '" + Id + "'"));
            Brush TextBrush = (Brush)bc.ConvertFrom(NotesMenu.ClsDB.String("SELECT TextColor FROM Notes WHERE Id = '" + Id + "'"));
            Brush XBrush = (Brush)bc.ConvertFrom(NotesMenu.ClsDB.String("SELECT XColor FROM Notes WHERE Id = '" + Id + "'"));
            this.Background = NoteBrush;
            TBTitle.Background = NoteBrush;
            TBTitle.BorderBrush = NoteBrush;
            TBTitle.Foreground = TextBrush;
            RTBContent.Background = NoteBrush;
            RTBContent.BorderBrush = NoteBrush;
            RTBContent.Foreground = TextBrush;
            LSettings.Foreground = XBrush;
            LClose.Foreground = XBrush;
            LSave.Foreground = XBrush;
            LColorChange.Foreground = XBrush;
            LCollapse.Foreground = XBrush;

            SP.Visibility = Visibility.Collapsed;

            TBTitle.Text = NotesMenu.ClsDB.String("SELECT Title FROM Notes WHERE Id = '" + Id + "'");

            if (NotesMenu.ClsDB.String("SELECT Content FROM Notes WHERE Id = '" + Id + "'") != "")
            {
                FlowDocument doc = XamlReader.Parse(NotesMenu.ClsDB.String("SELECT Content FROM Notes WHERE Id = '" + Id + "'")) as FlowDocument;

                RTBContent.Document = doc;
            }
        }

        private void LSettings_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isCollapsed == true)
            {
                this.Height = 300;
                isCollapsed = false;
                LSettings.Content = "&\xf013;";
                LSettings.ToolTip = "Open the Settings";
            }
            else
            {
                if (SP.Visibility == Visibility.Collapsed)
                {
                    SP.Visibility = Visibility.Visible;
                    LSettings.ToolTip = "Close the Settings";
                }
                else
                {
                    SP.Visibility = Visibility.Collapsed;
                    LSettings.ToolTip = "Open the Settings";
                }
            }
        }

        public void Save()
        {
            StringWriter wr = new StringWriter();
            XamlWriter.Save(RTBContent.Document, wr);
            string xaml = wr.ToString();

            NotesMenu.ClsDB.Execute_SQL("UPDATE Notes SET Title = '" + TBTitle.Text + "', Content = '" + xaml + "' WHERE Id = '" + Id + "'");

            NotesMenu notesMenu = Application.Current.Windows.OfType<NotesMenu>().SingleOrDefault();

            if (notesMenu != null)
            {
                NotesMenu.UpdateDataGrid();
            }
        }

        private void LClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void LSave_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Save();
        }

        private void LColorChange_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ColorEditor colorEditor = new ColorEditor(Id, this);
            colorEditor.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Save();
        }

        private void LCollapse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Height = 60;
            isCollapsed = true;
            LSettings.Content = "&\xf065;";
            SP.Visibility = Visibility.Collapsed;
            LSettings.ToolTip = "Expand";
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void TBTitle_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TBTitle.Focusable = true;
            TBTitle.Focus();
            TBTitle.Cursor = Cursors.IBeam;
            TBTitle.BorderThickness = new Thickness(1);
        }

        private void TBTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            TBTitle.Focusable = false;
            TBTitle.Cursor = Cursors.Arrow;
            TBTitle.BorderThickness = new Thickness(0);
        }

        private void RTBContent_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RTBContent.Focusable = true;
            RTBContent.Focus();
            RTBContent.Cursor = Cursors.IBeam;
            RTBContent.BorderThickness = new Thickness(1);
        }

        private void RTBContent_LostFocus(object sender, RoutedEventArgs e)
        {
            RTBContent.Focusable = false;
            RTBContent.Cursor = Cursors.Arrow;
            RTBContent.BorderThickness = new Thickness(0);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (Keyboard.IsKeyDown(Key.S))
                {
                    Save();
                }
            }
        }
    }
}
