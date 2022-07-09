using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            public static int Int(string SQL_Text, string DB)
            {
                SqlConnection cn_connection = Get_DB_Connection(DB);
                SqlCommand cmd_Command = new SqlCommand(SQL_Text, cn_connection);
                return (int)cmd_Command.ExecuteScalar();
            }
            public static string String(string SQL_Text, string DB)
            {
                SqlConnection cn_connection = Get_DB_Connection(DB);
                SqlCommand cmd_Command = new SqlCommand(SQL_Text, cn_connection);
                return (string)cmd_Command.ExecuteScalar();
            }

            public static bool Bool(string SQL_Text, string DB)
            {
                SqlConnection cn_connection = Get_DB_Connection(DB);
                SqlCommand cmd_Command = new SqlCommand(SQL_Text, cn_connection);
                return (bool)cmd_Command.ExecuteScalar();
            }

            public static object Obj(string SQL_Text, string DB)
            {
                SqlConnection cn_connection = Get_DB_Connection(DB);
                SqlCommand cmd_Command = new SqlCommand(SQL_Text, cn_connection);
                return (object)cmd_Command.ExecuteScalar();
            }

            public static int[] Ints(string SQL_Text, int length, string DB)
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

            int CountDev = Methods.ClsDB.Int("SELECT COUNT(*) FROM NoteColors", "DBPresets");
            int[] IdsDev = Methods.ClsDB.Ints("SELECT Id FROM(SELECT ROW_NUMBER() Over (Order By Id) as RowNum, * From NoteColors) t2 Where RowNum = ", CountDev, "DBPresets");

            if (IdsDev != null)
            {
                foreach (int IdDev in IdsDev)
                {
                    string[] TagDev = new string[2];
                    TagDev[0] = IdDev.ToString();
                    TagDev[1] = "DBPresets";

                    ComboBoxItem PresetDev = new ComboBoxItem
                    {
                        Content = Methods.ClsDB.String("SELECT Name FROM NoteColors WHERE Id = '" + IdDev + "'", "DBPresets"),
                        Tag = TagDev
                    };

                    CBPresets.Items.Add(PresetDev);
                }
            }

            int CountUser = Methods.ClsDB.Int("SELECT COUNT(*) FROM NoteColors", "DBUser");
            int[] IdsUser = Methods.ClsDB.Ints("SELECT Id FROM(SELECT ROW_NUMBER() Over (Order By Id) as RowNum, * From NoteColors) t2 Where RowNum = ", CountUser, "DBUser");

            if (IdsUser != null)
            {
                foreach (int IdUser in IdsUser)
                {
                    string[] TagUser = new string[2];
                    TagUser[0] = IdUser.ToString();
                    TagUser[1] = "DBUser";

                    ComboBoxItem PresetUser = new ComboBoxItem
                    {
                        Content = Methods.ClsDB.String("SELECT Name FROM NoteColors WHERE Id = '" + IdUser + "'", "DBUser"),
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
                CP_NoteColor.SelectedColor = (Color?)ColorConverter.ConvertFromString(Methods.ClsDB.String("SELECT NoteColor" + SQL, Tag[1]));
                CP_TextColor.SelectedColor = (Color?)ColorConverter.ConvertFromString(Methods.ClsDB.String("SELECT TextColor" + SQL, Tag[1]));
                CP_XColor.SelectedColor = (Color?)ColorConverter.ConvertFromString(Methods.ClsDB.String("SELECT XColor" + SQL, Tag[1]));
            }
        }

    }
}
