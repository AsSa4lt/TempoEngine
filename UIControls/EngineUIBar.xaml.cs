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
using TempoEngine.Engine;
using UserControl = System.Windows.Controls.UserControl;
using Microsoft.Win32;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using TempoEngine.Util;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace TempoEngine.UIControls
{
    /// <summary>
    /// Interaction logic for EngineUIBar.xaml
    /// </summary>
    public partial class EngineUIBar : UserControl{
        public EngineUIBar(){
            InitializeComponent();
        }

        private void saveButtonClick(object sender, RoutedEventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "TEN files (*.ten)|*.ten";  
            saveFileDialog.DefaultExt = "ten";                  
            saveFileDialog.AddExtension = true;                 

            // Show save file dialog box
            bool? result = saveFileDialog.ShowDialog();

            // Process save file dialog box results
            if (result == true) {
                // Save document
                string filename = saveFileDialog.FileName;
                FileManager.SaveToFile(filename);
            }
        }

        private void openButtonClick(object sender, RoutedEventArgs e) {
            // Create an instance of OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "TEN files (*.ten)|*.ten";
            openFileDialog.DefaultExt = "ten";
            openFileDialog.Multiselect = false;               

            bool? result = openFileDialog.ShowDialog();

            if (result == true) {
                string filename = openFileDialog.FileName;
                FileManager.LoadFromFile(filename);
            }
        }

    }
}
