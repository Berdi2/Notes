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
        private bool clickedTextboxContent = false;
        private bool clickedTextboxTitle = false;

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

            RTBContent.FontSize = Properties.Settings.Default.FontSize;

            SP.Visibility = Visibility.Collapsed;

            TBTitle.Text = NotesMenu.ClsDB.String("SELECT Title FROM Notes WHERE Id = '" + Id + "'");

            if (NotesMenu.ClsDB.String("SELECT Content FROM Notes WHERE Id = '" + Id + "'") != "")
            {
                FlowDocument doc = XamlReader.Parse(NotesMenu.ClsDB.String("SELECT Content FROM Notes WHERE Id = '" + Id + "'")) as FlowDocument;

                RTBContent.Document = doc;
            }
        }

        public int GetId()
        {
            return Id;
        }

        private void LSettings_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isCollapsed == true)
            {
                ExpandCollapse();
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

            if (NotesMenu.NM() != null)
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
            ExpandCollapse();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();

            if (!clickedTextboxTitle && TBTitle.IsFocused)
                TBLosesFocus(TBTitle);

            if (!clickedTextboxContent && RTBContent.IsFocused)
                TBLosesFocus(RTBContent);

            clickedTextboxContent = false;
            clickedTextboxTitle = false;
        }

        private void TBTitle_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TBGetsFocus(TBTitle);
        }

        private void TBTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            TBLosesFocus(TBTitle);
        }

        private void RTBContent_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TBGetsFocus(RTBContent);
        }

        private void RTBContent_LostFocus(object sender, RoutedEventArgs e)
        {
            TBLosesFocus(RTBContent);
        }

        public void TBGetsFocus(Control sender)
        {
            if (sender is TextBox || sender is RichTextBox)
            {
                sender.Focusable = true;
                sender.Focus();
                sender.Cursor = Cursors.IBeam;
                sender.BorderThickness = new Thickness(1);
            }
        }

        public void TBLosesFocus(Control sender)
        {
            if (sender is TextBox || sender is RichTextBox)
            {
                Keyboard.ClearFocus();
                sender.Focusable = false;
                sender.Cursor = Cursors.Arrow;
                sender.BorderThickness = new Thickness(0);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (Keyboard.IsKeyDown(Key.S))
                {
                    Save();
                }

                if (Keyboard.IsKeyDown(Key.F))
                {
                    ExpandCollapse();
                }
            }

            if(e.Key == Key.Tab)
            {
                if (TBTitle.IsFocused)
                {
                    TBGetsFocus(RTBContent);
                }
                else if (RTBContent.IsFocused)
                {
                    TBGetsFocus(TBTitle);
                }
                else if(!RTBContent.IsFocused && !TBTitle.IsFocused)
                {
                    TBGetsFocus(TBTitle);
                }
            }
        }

        public void ExpandCollapse()
        {
            if (isCollapsed)
            {
                this.Height = 300;
                isCollapsed = false;
                LSettings.Content = "&\xf013;";
                LSettings.ToolTip = "Open the Settings";
            }
            else
            {
                this.Height = 60;
                isCollapsed = true;
                LSettings.Content = "&\xf065;";
                SP.Visibility = Visibility.Collapsed;
                LSettings.ToolTip = "Expand (Strg + F)";
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            TBLosesFocus(TBTitle);
            TBLosesFocus(RTBContent);
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            //TBLosesFocus(TBTitle);
            //TBLosesFocus(RTBContent);
        }

        private void RTBContent_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clickedTextboxContent = true;
        }

        private void TBTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clickedTextboxTitle = true;
        }
    }
}
