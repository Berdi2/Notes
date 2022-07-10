using nUpdate.UI.WPF;
using nUpdate.Updating;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace Notes
{
    internal class Methods
    {
        public static class ClsDB

        {
            public static string Get_cn_String(string DB)
            {
                if (DB == "DBUser")
                    return "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + Environment.CurrentDirectory + "\\DBUser.mdf;Integrated Security=True";
                if (DB == "DBPresets")
                    return "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + Environment.CurrentDirectory + "\\DBPresets.mdf;Integrated Security=True";
                throw new Exception("Database doesn't exist");
            }

            public static SqlConnection Get_DB_Connection(string DB)
            {
                string cn_String = Get_cn_String(DB);
                SqlConnection cn_connection = new SqlConnection(cn_String);
                if (cn_connection.State != ConnectionState.Open) cn_connection.Open();
                return cn_connection;
            }
            public static DataTable Get_DataTable(string SQL_Text, string DB)
            {
                SqlConnection cn_connection = Get_DB_Connection(DB);
                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(SQL_Text, cn_connection);
                adapter.Fill(table);
                return table;
            }
            public static void Execute_SQL(string SQL_Text, string DB)
            {
                SqlConnection cn_connection = Get_DB_Connection(DB);
                SqlCommand cmd_Command = new SqlCommand(SQL_Text, cn_connection);
                cmd_Command.ExecuteNonQuery();
            }
            public static int Get_int(string SQL_Text, string DB)
            {
                SqlConnection cn_connection = Get_DB_Connection(DB);
                SqlCommand cmd_Command = new SqlCommand(SQL_Text, cn_connection);
                return (int)cmd_Command.ExecuteScalar();
            }
            public static string Get_string(string SQL_Text, string DB)
            {
                SqlConnection cn_connection = Get_DB_Connection(DB);
                SqlCommand cmd_Command = new SqlCommand(SQL_Text, cn_connection);
                return (string)cmd_Command.ExecuteScalar();
            }

            public static bool Get_bool(string SQL_Text, string DB)
            {
                SqlConnection cn_connection = Get_DB_Connection(DB);
                SqlCommand cmd_Command = new SqlCommand(SQL_Text, cn_connection);
                return (bool)cmd_Command.ExecuteScalar();
            }

            public static object Get_object(string SQL_Text, string DB)
            {
                SqlConnection cn_connection = Get_DB_Connection(DB);
                SqlCommand cmd_Command = new SqlCommand(SQL_Text, cn_connection);
                return (object)cmd_Command.ExecuteScalar();
            }

            public static int[] Get_ints(string SQL_Text, int length, string DB)
            {
                SqlConnection cn_connection = Get_DB_Connection(DB);
                int[] result = new int[length];
                for (int i = 0; i < length; i++)
                {
                    int i_i = i + 1;
                    string SQL_Text_new = SQL_Text + i_i;
                    SqlCommand cmd_Command = new SqlCommand(SQL_Text_new, cn_connection);
                    result[i] = (int)cmd_Command.ExecuteScalar();
                }
                return result;
            }

            public static void Close_DB_Connection(string DB)
            {
                string cn_String = Get_cn_String(DB);
                SqlConnection cn_connection = new SqlConnection(cn_String);
                if (cn_connection.State != ConnectionState.Closed) cn_connection.Close();
            }
        }

        public static FlowDocument StringToFlowDoc(string xamlString)
        {
            if (xamlString != "")
            {
                StringReader stringReader = new StringReader(xamlString);
                System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(stringReader);
                return XamlReader.Load(xmlReader) as FlowDocument;
            }
            FlowDocument flowDocument = new FlowDocument();
            return flowDocument;
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

        public static void UpdateCBPresets(ComboBox CBPresets)
        {
            CBPresets.Items.Clear();

            ComboBoxItem None = new ComboBoxItem
            {
                Content = "None",
                Tag = "None"
            };

            CBPresets.Items.Add(None);
            CBPresets.SelectedIndex = 0;

            int CountDev = ClsDB.Get_int("SELECT COUNT(*) FROM NoteColors", "DBPresets");
            int[] IdsDev = ClsDB.Get_ints("SELECT Id FROM(SELECT ROW_NUMBER() Over (Order By Id) as RowNum, * From NoteColors) t2 Where RowNum = ", CountDev, "DBPresets");

            if (IdsDev != null)
            {
                foreach (int IdDev in IdsDev)
                {
                    string[] TagDev = new string[2];
                    TagDev[0] = IdDev.ToString();
                    TagDev[1] = "DBPresets";

                    ComboBoxItem PresetDev = new ComboBoxItem
                    {
                        Content = ClsDB.Get_string("SELECT Name FROM NoteColors WHERE Id = '" + IdDev + "'", "DBPresets"),
                        Tag = TagDev
                    };

                    CBPresets.Items.Add(PresetDev);
                }
            }

            int CountUser = ClsDB.Get_int("SELECT COUNT(*) FROM NoteColors", "DBUser");
            int[] IdsUser = ClsDB.Get_ints("SELECT Id FROM(SELECT ROW_NUMBER() Over (Order By Id) as RowNum, * From NoteColors) t2 Where RowNum = ", CountUser, "DBUser");

            if (IdsUser != null)
            {
                foreach (int IdUser in IdsUser)
                {
                    string[] TagUser = new string[2];
                    TagUser[0] = IdUser.ToString();
                    TagUser[1] = "DBUser";

                    ComboBoxItem PresetUser = new ComboBoxItem
                    {
                        Content = ClsDB.Get_string("SELECT Name FROM NoteColors WHERE Id = '" + IdUser + "'", "DBUser"),
                        Tag = TagUser
                    };

                    CBPresets.Items.Add(PresetUser);
                }
            }

            ComboBoxItem ManagePresets = new ComboBoxItem()
            {
                Content = "Manage your Presets...",
                Tag = "ManagePresets"
            };

            CBPresets.Items.Add(ManagePresets);
        }

        public static void CBPresets_SelectionChangedHandler(ComboBox CBPresets, ColorPicker CP_NoteColor, ColorPicker CP_TextColor, ColorPicker CP_XColor)
        {
            ComboBoxItem item = (ComboBoxItem)CBPresets.SelectedItem;
            if (item == null || item.Tag == null)
                return;
            if (item.Tag == "ManagePresets")
            {
                ManagePresets managePresets = new ManagePresets(CBPresets);
                managePresets.Show();
                CBPresets.SelectedIndex = 0;
            }
            else if (item.Tag != "None")
            {
                string[] Tag = (string[])item.Tag;
                string SQL = " FROM NoteColors WHERE Id = '" + Tag[0] + "'";
                CP_NoteColor.SelectedColor = (Color?)ColorConverter.ConvertFromString(ClsDB.Get_string("SELECT NoteColor" + SQL, Tag[1]));
                CP_TextColor.SelectedColor = (Color?)ColorConverter.ConvertFromString(ClsDB.Get_string("SELECT TextColor" + SQL, Tag[1]));
                CP_XColor.SelectedColor = (Color?)ColorConverter.ConvertFromString(ClsDB.Get_string("SELECT XColor" + SQL, Tag[1]));
            }
        }

        public static void AddNote(string NoteColor, string TextColor, string XColor)
        {
            int Id = NullToZero(ClsDB.Get_object("SELECT MAX(Id) FROM Notes", "DBUser")) + 1;
            ClsDB.Execute_SQL("INSERT INTO Notes VALUES('" + Id + "','','','" + NoteColor + "','" + TextColor + "','" + XColor + "')", "DBUser");
            UpdateListView();
            OpenNote(Id);
        }

        public static void DeleteNotes(int[] Ids = null, bool SelectedItems = false)
        {
            bool isOneOpen = false;

            //Weist Ids einen Wert zu wenn die angewählten Notes gelöscht werden sollen

            if (SelectedItems && Ids == null)
            {
                if (NotesMenu.NM().LV.SelectedItems.Count > 0)
                    Ids = new int[NotesMenu.NM().LV.SelectedItems.Count];
                for (int i = 0; i < NotesMenu.NM().LV.SelectedItems.Count; i++)
                {
                    NoteDisplay Note = (NoteDisplay)NotesMenu.NM().LV.SelectedItems[i];
                    Ids[i] = Note.Id;
                }
            }

            if (Ids != null)
            {
                foreach (int Note in Ids)
                {
                    if (CheckIfNoteIsOpen(Note))
                    {
                        System.Windows.MessageBox.Show("The Note \"" + ClsDB.Get_string("SELECT Title FROM Notes WHERE Id = '" + Note + "'", "DBUser") + "\" is open close it before deleting it!", "Notes");
                        isOneOpen = true;
                    }
                }
            }

            if (!isOneOpen && Ids != null)
            {
                if (System.Windows.MessageBox.Show("Do you really want to delete this Note/these Notes?", "Notes", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    foreach (int Note in Ids)
                        ClsDB.Execute_SQL("DELETE FROM Notes WHERE Id = '" + Note + "'", "DBUser");
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
            foreach (NoteDisplay Note in NotesMenu.NM().LV.SelectedItems)
            {
                OpenNote(Note.Id);
            }
        }

        public static void UpdateListView()
        {
            NotesMenu.NM().LV.Items.Clear();

            int Count = ClsDB.Get_int("SELECT COUNT(*) FROM Notes", "DBUser");
            int[] Ints = ClsDB.Get_ints("SELECT Id FROM(SELECT ROW_NUMBER() Over (Order By Id) as RowNum, * From Notes) t2 Where RowNum = ", Count, "DBUser");

            if (Ints != null)
            {
                foreach (int Note in Ints)
                {
                    NoteDisplay ND = new NoteDisplay(Note);
                    ND.MouseDoubleClick += NotesMenu.LVItem_DoubleClick;
                    ND.MouseRightButtonUp += NotesMenu.LVItem_MouseRightButtonUp;
                    NotesMenu.NM().LV.Items.Add(ND);
                }
            }
        }

        public static void ChangeColor(int Id, string NoteColor, string TextColor, string XColor)
        {
            ClsDB.Execute_SQL("UPDATE Notes SET NoteColor = '" + NoteColor + "', TextColor = '" + TextColor + "', XColor = '" + XColor + "' WHERE Id = '" + Id + "'", "DBUser");
        }
    }
}
