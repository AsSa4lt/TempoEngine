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
    public partial class AddTriangleWIndow : Window {
        GrainTriangle _newTriangle;
        public Action OnObjectAdded;
        public AddTriangleWIndow() {
            InitializeComponent();
            _newTriangle = new GrainTriangle("New Triangle", new Point(0, 0), new Point(0, 0), new Point(0, 0));
            int i = 0;
            while (!Engine.Engine.IsNameAvailable($"New triangle {i}")) i++;
            _newTriangle.Name = $"New triangle {i}";
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e) {
            // Set the default values for the textboxes
            tbName.Text = _newTriangle.Name;
            tbPointAXPosition.Text = _newTriangle.PointA.X.ToString();
            tbPointAYPosition.Text = _newTriangle.PointA.Y.ToString();
            tbPointBXPosition.Text = _newTriangle.PointB.X.ToString();
            tbPointBYPosition.Text = _newTriangle.PointB.Y.ToString();
            tbPointCXPosition.Text = _newTriangle.PointC.X.ToString();
            tbPointCYPosition.Text = _newTriangle.PointC.Y.ToString();
            tbMass.Text = _newTriangle.Mass.ToString();
            tbTemperature.Text = _newTriangle.SimulationTemperature.ToString();
            tbThermalConductivity.Text = _newTriangle.ThermalConductivity.ToString();
            tbHeatCapacity.Text = _newTriangle.SpecificHeatCapacity.ToString();
        }

        private void ClearColors() {
            tbName.Background                = Brushes.White;
            tbPointAXPosition.Background     = Brushes.White;
            tbPointAYPosition.Background     = Brushes.White;
            tbPointBXPosition.Background     = Brushes.White;
            tbPointBYPosition.Background     = Brushes.White;
            tbPointCXPosition.Background     = Brushes.White;
            tbPointCYPosition.Background     = Brushes.White;
            tbMass.Background                = Brushes.White;
            tbTemperature.Background         = Brushes.White;
            tbThermalConductivity.Background = Brushes.White;
            tbHeatCapacity.Background        = Brushes.White;
        }

        private void CheckObject(out string errorMessage) {
            errorMessage = "";
            if(_newTriangle.Name == "") {
                errorMessage = "Name cannot be empty";
                tbName.Background = Brushes.Red;
            }

            if(!Engine.Engine.IsNameAvailable(_newTriangle.Name)) {
                errorMessage = "Name already exists";
                tbName.Background = Brushes.Red;
            }

            if (!Engine.Engine.IsPositionAvailable(_newTriangle)) {
                errorMessage = "Position is not available";
                tbPointAXPosition.Background = Brushes.Red;
                tbPointAYPosition.Background = Brushes.Red;
                tbPointBXPosition.Background = Brushes.Red;
                tbPointBYPosition.Background = Brushes.Red;
                tbPointCXPosition.Background = Brushes.Red;
                tbPointCYPosition.Background = Brushes.Red;
            }

            if(_newTriangle.Mass <= 0) {
                errorMessage = "Mass must be greater than 0";
                tbMass.Background = Brushes.Red;
            }

            if(_newTriangle.SimulationTemperature < 0) {
                errorMessage = "Temperature must be greater than or equal to 0";
                tbTemperature.Background = Brushes.Red;
            }

            if(_newTriangle.ThermalConductivity < 0) {
                errorMessage = "Thermal conductivity must be greater than or equal to 0";
                tbThermalConductivity.Background = Brushes.Red;
            }

            if(_newTriangle.SpecificHeatCapacity <= 0) {
                errorMessage = "Specific heat capacity must be greater than 0";
                tbHeatCapacity.Background = Brushes.Red;
            }

            if(_newTriangle.PointA == _newTriangle.PointB || _newTriangle.PointA == _newTriangle.PointC || _newTriangle.PointB == _newTriangle.PointC) {
                errorMessage = "Points cannot be the same";
                tbPointAXPosition.Background = Brushes.Red;
                tbPointAYPosition.Background = Brushes.Red;
                tbPointBXPosition.Background = Brushes.Red;
                tbPointBYPosition.Background = Brushes.Red;
                tbPointCXPosition.Background = Brushes.Red;
                tbPointCYPosition.Background = Brushes.Red;
            }

        }

        private void AddButton_Click(object sender, RoutedEventArgs e) {
            ClearColors();
            
            _newTriangle.Name                   = tbName.Text;
            _newTriangle.PointA                 = new Point(double.Parse(tbPointAXPosition.Text), double.Parse(tbPointAYPosition.Text));
            _newTriangle.PointB                 = new Point(double.Parse(tbPointBXPosition.Text), double.Parse(tbPointBYPosition.Text));
            _newTriangle.PointC                 = new Point(double.Parse(tbPointCXPosition.Text), double.Parse(tbPointCYPosition.Text));
            _newTriangle.Mass                   = double.Parse(tbMass.Text);
            _newTriangle.SimulationTemperature  = double.Parse(tbTemperature.Text);
            _newTriangle.ThermalConductivity    = double.Parse(tbThermalConductivity.Text);
            _newTriangle.SpecificHeatCapacity   = double.Parse(tbHeatCapacity.Text);

            CheckObject(out string errorMessage);

            if(errorMessage != "") {
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Engine.Engine.AddObject(_newTriangle);
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
