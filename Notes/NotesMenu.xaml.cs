﻿using System;
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

            UpdateListView();

            SearchForUpdate(true);
        }

        public static NotesMenu NM()
        {
            return Application.Current.Windows.OfType<NotesMenu>().FirstOrDefault();
        }

        private static void LVItem_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenNote_LVSelectedItems();
        }

        private void BAddNote_Click(object sender, RoutedEventArgs e)
        {
            NoteCreator noteCreator = new NoteCreator();
            noteCreator.Show();
        }

        public static void AddNote(string NoteColor, string TextColor, string XColor)
        {
            int Id = NullToZero(Methods.ClsDB.Obj("SELECT MAX(Id) FROM Notes", "DBUser")) + 1;
            Methods.ClsDB.Execute_SQL("INSERT INTO Notes VALUES('" + Id + "','','','" + NoteColor + "','" + TextColor + "','" + XColor + "')", "DBUser");
            UpdateListView();
            OpenNote(Id);
        }

        public static void DeleteNotes(int[] Ids = null, bool SelectedItems = false)
        {
            bool isOneOpen = false;

            //Weist Ids einen Wert zu wenn die angewählten Notes gelöscht werden sollen

            if (SelectedItems && Ids == null)
            {
                if (NM().LV.SelectedItems.Count > 0)
                    Ids = new int[NM().LV.SelectedItems.Count];
                for (int i = 0; i < NM().LV.SelectedItems.Count; i++)
                {
                    NoteDisplay Note = (NoteDisplay)NM().LV.SelectedItems[i];
                    Ids[i] = Note.Id;
                }
            }

            if (Ids != null)
            {
                foreach (int Note in Ids)
                {
                    if (CheckIfNoteIsOpen(Note))
                    {
                        MessageBox.Show("The Note \"" + Methods.ClsDB.String("SELECT Title FROM Notes WHERE Id = '" + Note + "'", "DBUser") + "\" is open close it before deleting it!", "Notes");
                        isOneOpen = true;
                    }
                }
            }

            if (!isOneOpen && Ids != null)
            {
                if (MessageBox.Show("Do you really want to delete this Note/these Notes?", "Notes", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    foreach (int Note in Ids)
                        Methods.ClsDB.Execute_SQL("DELETE FROM Notes WHERE Id = '" + Note + "'", "DBUser");
                    UpdateListView();
                }
            }
        }

        public static bool CheckIfNoteIsOpen(int Id)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is Note note)
                {
                    if (note.GetId() == Id)
                    {
                        return true;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }
            return false;
        }

        public static void OpenNote(int Id)
        {
            if (!CheckIfNoteIsOpen(Id))
            {
                Note note = new Note(Id);
                note.Show();
            }
        }

        public static void OpenNote_LVSelectedItems()
        {
            foreach (NoteDisplay Note in NM().LV.SelectedItems)
            {
                OpenNote(Note.Id);
            }
        }

        public static int NullToZero(object Value)
        {
            if (Value == DBNull.Value || Value == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(Value);
            }
        }

        private static void Delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteNotes(SelectedItems: true);
        }

        public static void UpdateListView()
        {
            NM().LV.Items.Clear();

            int Count = Methods.ClsDB.Int("SELECT COUNT(*) FROM Notes", "DBUser");
            int[] Ints = Methods.ClsDB.Ints("SELECT Id FROM(SELECT ROW_NUMBER() Over (Order By Id) as RowNum, * From Notes) t2 Where RowNum = ", Count, "DBUser");

            if (Ints != null)
            {
                foreach (int Note in Ints)
                {
                    NoteDisplay ND = new NoteDisplay(Note);
                    ND.MouseDoubleClick += LVItem_DoubleClick;
                    ND.MouseRightButtonUp += LVItem_MouseRightButtonUp;
                    NM().LV.Items.Add(ND);
                }
            }
        }

        public static void ChangeColor(int Id, string NoteColor, string TextColor, string XColor)
        {
            Methods.ClsDB.Execute_SQL("UPDATE Notes SET NoteColor = '" + NoteColor + "', TextColor = '" + TextColor + "', XColor = '" + XColor + "' WHERE Id = '" + Id + "'", "DBUser");
        }

        public static void SearchForUpdate(bool HiddenSearch)
        {
            UpdateManager manager = new UpdateManager(new Uri("http://berdi.bplaced.net/Notes/updates.json"), "<RSAKeyValue><Modulus>2MWdrPFjPFB1IbmYJAyGHVCp/elJKbLNwXlS6gtPmoqP25LBojII7I3PPY1X+A6nLlxr5aUbcDKbNodrTgpHbjbd6f4z9aDCoJJMGGVPR/j3DOiDstRMR7m61gemQy7l5CQBoIyj3XNSf4/FoMckaIym69QWoT37aH2YiRzdqLIoeBP+KvWNj78buwC9LiVp3yy5ZAkX6dFuCUVKX9gnn/g248Kazrpc5OxC1EBrK4hbPMojPfwVDSqRNZHaPlGIZJiNJ1bgXxuyFhlPeq1lC4Ra8iyOrNyTP7KgENvNCR37hGM1zVeGKhQSfAgvkwT8//PNf3Px7yF475Dlr17O8TnLctg/fltLxo6Wq7uHehf9P9FN1fy3KMEC7SPy6JX7Jxqqqd50eNfCf2IkifqEDNRrcgxwV1zHyXhVo8AiVIaQC9C2V9j9v0Wc/mwzy+5SnsbW1nA6bZ4zELiVOx97I4E76gmfPfMHI3oHObwWJh+gNNCvzbGpVugqV+kLKmUkp81hrfwdvn6L6/H5y6a3OWxhuh1Bfrsqd6CcxLfpx6n+eN/MHeuQISV9A+PyOWhFhmOCbK4ML+SYGJNv0kXo8v2OpodZqFzyRjFLT629Iq4MPJ/+P3z+IIM1ZP/yZuZS5RNwOTpCVZp1kzsyBDUIh/aZt6XafpnmFXBY59EFRU1MXJ1FerRaNeeeD7G7LnFeO9/VLD4vswKj6451KRHLLIEFxibvDaNm3Yqr97HfkU7dF0j6e9C3MoUeHNxebS5hlCDis47+UI6HukSrfWjC81OwxVsjsjgpWECzy+FtirCJsrYwzpQ5UKwpxgHw7no48HMEP2xnkdweBI9eFK8P3nOk5gVlA4v/sLuoG1eE04LkRxNpy/c7syvtmSLXQTcSy/kJ87pB20NTNCm5eY5gZTklXh7tRk0i3wl81SAUeJFHYZfK5xJ8mnhA2jDrc61gi72WaEsvzJ55AXcUHQxLFsVnYGx8EJFKExOtfXLKPXig5rCK1ZVQQLAvkNcXIPQW2LmQJ1q+eMau/8Zgf61tSasfssYNoS0l79VzZPE1PyIE1/A0quH/35uFOf17SHaseMXDU/6PPz7J37KFrfwLKbodc103ISa0uU6TA8/Jd/GKsMPZxigv9W7ZCsbCdHRnnMlHcGrO4hzfeTRggtHsn5exoldWyWpRbQ+RDOrEBRQfxOWuoi/8Sc8sVBaTmFnJ7eNzN9+nkNQ2cyeN95zJBJhXToQoFxXqmdDwRFTzM+FL2gzfVrsVCEWGm7HhXWDHZ/7HHW50gf0fk9qhRMkHzctHMvdJpza78gePm0ooPe/qVsQdIodXpWeWLgiU6sVuMmsxo8gJvFcmxhA3xvZ2aQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>", new CultureInfo("en"))
            {
                IncludeAlpha = Properties.Settings.Default.Alpha
            };
            var updaterUI = new UpdaterUI(manager, SynchronizationContext.Current);
            if (HiddenSearch)
            {
                if (manager.SearchForUpdates())
                {
                    updaterUI.ShowUserInterface();
                }
            }
            else
            {
                updaterUI.ShowUserInterface();
            }
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
                DeleteNotes(SelectedItems: true);
            }

            if (e.Key == Key.Enter)
            {
                OpenNote_LVSelectedItems();
            }
        }

        private static void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenNote_LVSelectedItems();
        }

        private static void LVItem_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
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
