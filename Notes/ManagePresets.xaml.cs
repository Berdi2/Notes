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
    /// Interaction logic for ManagePresets.xaml
    /// </summary>
    public partial class ManagePresets : Window
    {
        readonly ComboBox CBPresets;
        public ManagePresets(ComboBox CBPresets_new)
        {
            InitializeComponent();
            CBPresets = CBPresets_new;
        }
    }
}
