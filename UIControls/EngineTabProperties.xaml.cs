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
using Brushes = System.Windows.Media.Brushes;
using MessageBox = System.Windows.MessageBox;
using Point = System.Windows.Point;
using TextBox = System.Windows.Controls.TextBox;
using UserControl = System.Windows.Controls.UserControl;
namespace TempoEngine.UIControls {
    /// <summary>
    /// Interaction logic for EngineTabProperties.xaml
    /// </summary>
    public partial class EngineTabProperties : UserControl {
        private EngineObject? _selectedObject;
        private bool _isTriangleMode = false;
        private bool isEnabled = true;
        public EngineTabProperties() : base() {
            InitializeComponent();
            // On Loaded event, we need to update the properties tab
            Loaded += (sender, args) => {
                Update();
            };
        }

        public void Update() {
            if (_selectedObject == null) {
                SetFieldsEnabled(false);
                return;
            }
            ClearFields();
            if(isEnabled)
                SetFieldsEnabled(true);
            SetObjectParameters();
        }

        public void Enable(bool IsEnabled) {
            isEnabled = IsEnabled;
            if(!isEnabled) {
                SetFieldsEnabled(false);
            }
        }

        private void SetObjectParameters() {
            if (_selectedObject == null) return;
            tbName.Text = _selectedObject.Name;
            // show only two decimal places for temperature
            tbTemperature.Text = Math.Round(_selectedObject.CurrentTemperature, 2).ToString();
            tbThermalConductivity.Text = _selectedObject.ThermalConductivity.ToString();
            tbMass.Text = _selectedObject.Mass.ToString();
            tbXPosition.Text = _selectedObject.Position.X.ToString();
            tbYPosition.Text = _selectedObject.Position.Y.ToString();
            tbHeatCapacity.Text = _selectedObject.SpecificHeatCapacity.ToString();
        }

        public void SetObject(EngineObject obj) {
            ClearFields();
            _selectedObject = obj;
            if (obj == null) return;
        }

        // function to set are fields enabled or disabled
        private void SetFieldsEnabled(bool enabled) {
            tbName.IsEnabled                 = enabled;
            tbTemperature.IsEnabled          = enabled;
            tbThermalConductivity.IsEnabled  = enabled;
            tbXPosition.IsEnabled            = enabled;
            tbYPosition.IsEnabled            = enabled;
            tbHeight.IsEnabled               = enabled;
            tbWidth.IsEnabled                = enabled;
            tbMass.IsEnabled                 = enabled;
            tbHeatCapacity.IsEnabled         = enabled;
        }

        private void tbName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if(!basicInputCheck(tbName, e))                     return;
            if (_selectedObject.Name != tbName.Text && !Engine.Engine.IsNameAvailable(tbName.Text)) {
                ShowErrorMessageBox("Name is not available");
                tbName.Background = Brushes.Red;
                return;
            }
            _selectedObject.Name = tbName.Text;
            tbName.Background = Brushes.White;
        }

        private void ShowErrorMessageBox(string text) {
            MessageBox.Show(text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void tbTemperature_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if(!basicInputCheck(tbTemperature, e))              return;
            tbTemperature.Background = Brushes.White;
            if (double.TryParse(tbTemperature.Text, out double temperature)) _selectedObject.SimulationTemperature = temperature;
            else tbTemperature.Background = Brushes.Red;
        }

        private void tbThermalConductivity_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if(!basicInputCheck(tbThermalConductivity, e))      return;
            tbThermalConductivity.Background = Brushes.White;
            if(double.TryParse(tbThermalConductivity.Text, out double thermCond)) _selectedObject.ThermalConductivity = thermCond;
            else tbThermalConductivity.Background = Brushes.Red;
        }

        private bool basicInputCheck(TextBox tb, System.Windows.Input.KeyEventArgs e) {
            if(_selectedObject == null)      return false;
            if (e.Key != Key.Enter)          return false;
            else                             return true;
        }

        private void ClearFields() {
            tbName.Text                      = "";
            tbTemperature.Text               = "";
            tbThermalConductivity.Text       = "";
            tbXPosition.Text                 = "";
            tbYPosition.Text                 = "";
            tbHeight.Text                    = "";
            tbWidth.Text                     = "";
            tbMass.Text                      = "";
            tbHeatCapacity.Text              = "";

            tbName.Background                = Brushes.White;
            tbTemperature.Background         = Brushes.White;
            tbThermalConductivity.Background = Brushes.White;
            tbXPosition.Background           = Brushes.White;
            tbYPosition.Background           = Brushes.White;
            tbHeight.Background              = Brushes.White;
            tbWidth.Background               = Brushes.White;
            tbMass.Background                = Brushes.White;
            tbHeatCapacity.Background        = Brushes.White;
        }

        private void tbMass_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if(!basicInputCheck(tbMass, e))                     return;
            tbMass.Background = Brushes.White;
            if (double.TryParse(tbMass.Text, out double mass)) _selectedObject.Mass = mass;
            else tbMass.Background = Brushes.Red;
        }

        private void tbHeatCapacity_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if(!basicInputCheck(tbHeatCapacity, e))             return;
            tbHeatCapacity.Background = Brushes.White;
            if (double.TryParse(tbHeatCapacity.Text, out double heatCapacity)) _selectedObject.SpecificHeatCapacity = heatCapacity;
            else tbHeatCapacity.Background = Brushes.Red;
        }

        private void tbXPosition_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            try {
                Point pos = new Point(double.Parse(tbXPosition.Text), _selectedObject.Position.Y);
                _selectedObject.Position = pos;
            } catch (Exception ex) {

            }
        }

        private void tbYPosition_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            try {
                Point pointA = new Point(_selectedObject.Position.X, double.Parse(tbYPosition.Text));
                _selectedObject.Position = pointA;
            } catch (Exception ex) {

            }
        }
    }
}
