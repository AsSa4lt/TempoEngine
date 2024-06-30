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
using MessageBox = System.Windows.MessageBox;
using Button = System.Windows.Controls.Button;
using log4net;

namespace TempoEngine.UIControls
{
    /// <summary>
    /// Interaction logic for EngineUIBar.xaml
    /// </summary>
    public partial class EngineUIBar : UserControl{
        // callback to update the UI
        public Action UpdateUI;
        public Action<EngineObject> DeleteSelected;
        public Action EngineModeChanged;
        private static readonly ILog log = LogManager.GetLogger(typeof(EngineUIBar));


        public EngineUIBar(){
            InitializeComponent();
            Loaded += (sender, args) => {
                SetStoppedMode();
            };
        }

        private void SetStoppedMode() {
            // set engine controls
            startButton.IsEnabled = true;
            stopButton.IsEnabled = false;
            pauseButton.IsEnabled = false;

            // set ui controls
            addButton.IsEnabled = true;
            saveButton.IsEnabled = true;
            openButton.IsEnabled = true;
            clearButton.IsEnabled = true;
        }

        public void Update() {
            long currentTime = Engine.Engine.GetSimulationTimeUnsafe();
            TimeSpan time = TimeSpan.FromMilliseconds(currentTime);
            timeLabel.Content = time.ToString(@"mm\:ss\:ff");
        }

        private void SetRunningMode() {
            // set engine controls
            startButton.IsEnabled = false;
            stopButton.IsEnabled = true;
            pauseButton.IsEnabled = true;

            // set ui controls
            addButton.IsEnabled = false;
            saveButton.IsEnabled = false;
            openButton.IsEnabled = false;
            clearButton.IsEnabled = false;
        }

        private void SetPausedMode() {
            // set engine controls
            startButton.IsEnabled = true;
            stopButton.IsEnabled = true;
            pauseButton.IsEnabled = false;

            // set ui controls
            addButton.IsEnabled = false;
            saveButton.IsEnabled = false;
            openButton.IsEnabled = false;
            clearButton.IsEnabled = false;
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
                UpdateUI?.Invoke();
            }
        }

        private void clearButtonClick(object sender, RoutedEventArgs e) {
            string message = "Are you sure you want to clear all data?";
            string caption = "Confirm Clear";
            MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) {
                Engine.Engine.ClearObjects();
                DeleteSelected?.Invoke(null);
            }
        }

        private void addButtonClick(object sender, RoutedEventArgs e) {
            Button? button = sender as Button;
            if (button != null && button.ContextMenu != null) {
                button.ContextMenu.PlacementTarget = button;
                button.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                button.ContextMenu.IsOpen = true;
            }
        }

        private void addTriangleClicked(object sender, RoutedEventArgs e) {
            AddSquareWindow addTriangleWIndow = new AddSquareWindow();
            addTriangleWIndow.OnObjectAdded = UpdateUI;
            addTriangleWIndow.ShowDialog();
        }

        private void startButton_Click(object sender, RoutedEventArgs e) {
            Engine.Engine.Start();
            EngineModeChanged?.Invoke();
            SetRunningMode();
        }

        private void stopButton_Click(object sender, RoutedEventArgs e) {
            Engine.Engine.Stop();
            EngineModeChanged?.Invoke();
            SetStoppedMode();
        }

        private void pauseButton_Click(object sender, RoutedEventArgs e) {
            Engine.Engine.Pause();
            EngineModeChanged?.Invoke();
            SetPausedMode();
        }
    }
}
