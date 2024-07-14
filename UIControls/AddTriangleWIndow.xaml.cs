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
using TempoEngine.Engine;
using TempoEngine.Engine.Managers;
using Brushes = System.Windows.Media.Brushes;
using MessageBox = System.Windows.MessageBox;
using Point = System.Windows.Point;

namespace TempoEngine.UIControls {
    /**
     * \class AddSquareWindow
     * \brief Represents the window for adding a square.
     * 
     * The AddSquareWindow class provides methods for managing the window for adding a square.
     */
    public partial class AddSquareWindow : Window {
        GrainSquare _newSquare;         // New square that will be created
        public Action OnObjectAdded;    // Action to update the main windows after adding the object
        
        /**
         * \brief Initializes a new instance of the AddSquareWindow class.
         * Sets the default values for the new square.
         */
        public AddSquareWindow() {
            InitializeComponent();
            _newSquare = new GrainSquare("New square", new Point(0, 0));
            int i = 0;
            while (!Engine.Engine.EngineObjectsManager.IsNameAvailable($"New square {i}")) i++;
            _newSquare.Name = $"New square {i}";
            Loaded += OnLoaded;
        }

        /**
         * \brief OnLoaded action
         * Set base data for the square
         */
        private void OnLoaded(object sender, RoutedEventArgs e) {
            // Set the default values for the textboxes
            tbName.Text = _newSquare.Name;
            tbXPosition.Text = _newSquare.Position.X.ToString();
            tbYPosition.Text = _newSquare.Position.Y.ToString();
            tbTemperature.Text = _newSquare.SimulationTemperature.ToString();
            MaterialManager.GetMaterials().ForEach(x => cbMaterial.Items.Add(x.Name));
            cbMaterial.SelectedIndex = 0;
        }

        /**
         *  Color the colors on state without error
         */
        private void ClearColors() {
            tbName.Background                = Brushes.White;
            tbXPosition.Background           = Brushes.White;
            tbYPosition.Background           = Brushes.White;
            tbTemperature.Background         = Brushes.White;
            cbMaterial.Background            = Brushes.White;
        }

        /**
         * \brief Check the object for errors
         * \param errorMessage The error message
         */
        private void CheckObject(out string errorMessage) {
            errorMessage = "";
            if(_newSquare.Name == "") {
                errorMessage = "Name cannot be empty";
                tbName.Background = Brushes.Red;
            }

            if(!Engine.Engine.EngineObjectsManager.IsNameAvailable(_newSquare.Name)) {
                errorMessage = "Name already exists";
                tbName.Background = Brushes.Red;
            }

            if (!Engine.Engine.EngineObjectsManager.IsPositionAvailable(_newSquare)) {
                errorMessage = "Position is not available";
                tbXPosition.Background = Brushes.Red;
                tbYPosition.Background = Brushes.Red;
            }

            if(cbMaterial.SelectedItem == null) {
                errorMessage = "Material must be selected";
                cbMaterial.Background = Brushes.Red;
            }


            if(_newSquare.SimulationTemperature < 0) {
                errorMessage = "Temperature must be greater than or equal to 0";
                tbTemperature.Background = Brushes.Red;
            }

        }

        /**
         * \brief Add button click event
         * \param sender The sender
         * \param e The event arguments
         */
        private void AddButton_Click(object sender, RoutedEventArgs e) {
            ClearColors();
            
            _newSquare.Name                   = tbName.Text;
            _newSquare.Position               = new Point(double.Parse(tbXPosition.Text), double.Parse(tbYPosition.Text));
            _newSquare.SimulationTemperature  = double.Parse(tbTemperature.Text);
            _newSquare.Material                = MaterialManager.GetMaterialByName(cbMaterial.SelectedItem.ToString());

            CheckObject(out string errorMessage);

            if(errorMessage != "") {
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Engine.Engine.EngineObjectsManager.AddObject(_newSquare);
            OnObjectAdded?.Invoke();
            Close();
        }


        /**
         * \brief Cancel button click event
         * \param sender The sender
         * \param e The event arguments
         */
        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        /**
         * \brief Textbox key down event
         * \param sender The sender
         * \param e The event arguments
         */
        private void Button_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if(e.Key != Key.Enter) return;
        }
    }
}
