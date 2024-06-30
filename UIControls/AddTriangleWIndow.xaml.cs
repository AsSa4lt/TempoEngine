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
using Brushes = System.Windows.Media.Brushes;
using MessageBox = System.Windows.MessageBox;
using Point = System.Windows.Point;

namespace TempoEngine.UIControls {
    /// <summary>
    /// Interaction logic for AddTriangleWIndow.xaml
    /// </summary>
    public partial class AddSquareWindow : Window {
        GrainSquare _newSquare;
        public Action OnObjectAdded;
        public AddSquareWindow() {
            InitializeComponent();
            _newSquare = new GrainSquare("New square", new Point(0, 0));
            int i = 0;
            while (!Engine.Engine.IsNameAvailable($"New square {i}")) i++;
            _newSquare.Name = $"New square {i}";
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e) {
            // Set the default values for the textboxes
            tbName.Text = _newSquare.Name;
            tbXPosition.Text = _newSquare.Position.X.ToString();
            tbYPosition.Text = _newSquare.Position.Y.ToString();
            tbMass.Text = _newSquare.Mass.ToString();
            tbTemperature.Text = _newSquare.SimulationTemperature.ToString();
        }

        private void ClearColors() {
            tbName.Background                = Brushes.White;
            tbXPosition.Background     = Brushes.White;
            tbYPosition.Background     = Brushes.White;
            tbMass.Background                = Brushes.White;
            tbTemperature.Background         = Brushes.White;
            tbThermalConductivity.Background = Brushes.White;
            tbHeatCapacity.Background        = Brushes.White;
        }

        private void CheckObject(out string errorMessage) {
            errorMessage = "";
            if(_newSquare.Name == "") {
                errorMessage = "Name cannot be empty";
                tbName.Background = Brushes.Red;
            }

            if(!Engine.Engine.IsNameAvailable(_newSquare.Name)) {
                errorMessage = "Name already exists";
                tbName.Background = Brushes.Red;
            }

            if (!Engine.Engine.IsPositionAvailable(_newSquare)) {
                errorMessage = "Position is not available";
                tbXPosition.Background = Brushes.Red;
                tbYPosition.Background = Brushes.Red;
            }

            if(_newSquare.Mass <= 0) {
                errorMessage = "Mass must be greater than 0";
                tbMass.Background = Brushes.Red;
            }

            if(_newSquare.SimulationTemperature < 0) {
                errorMessage = "Temperature must be greater than or equal to 0";
                tbTemperature.Background = Brushes.Red;
            }

        }

        private void AddButton_Click(object sender, RoutedEventArgs e) {
            ClearColors();
            
            _newSquare.Name                   = tbName.Text;
            _newSquare.Position               = new Point(double.Parse(tbXPosition.Text), double.Parse(tbYPosition.Text));
            _newSquare.Mass                   = double.Parse(tbMass.Text);
            _newSquare.SimulationTemperature  = double.Parse(tbTemperature.Text);

            CheckObject(out string errorMessage);

            if(errorMessage != "") {
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Engine.Engine.AddObject(_newSquare);
            OnObjectAdded?.Invoke();
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void Button_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if(e.Key != Key.Enter) return;
        }
    }
}
