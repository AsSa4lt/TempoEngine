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
            tbTemperature.Text = _selectedObject.Temperature.ToString();
            tbThermalConductivity.Text = _selectedObject.ThermalConductivity.ToString();
            tbMass.Text = _selectedObject.Mass.ToString();
            if (_isTriangleMode) {
                GrainTriangle triangle = (GrainTriangle) _selectedObject;
                pointAX.Text = triangle.PointA.X.ToString();
                pointAY.Text = triangle.PointA.Y.ToString();
                pointBX.Text = triangle.PointB.X.ToString();
                pointBY.Text = triangle.PointB.Y.ToString();
                pointCX.Text = triangle.PointC.X.ToString();
                pointCY.Text = triangle.PointC.Y.ToString();
            }
            tbHeatCapacity.Text = _selectedObject.SpecificHeatCapacity.ToString();
        }

        public void SetObject(EngineObject obj) {
            ClearFields();
            _selectedObject = obj;
            if (obj == null) return;
            _isTriangleMode = obj.GetObjectType() == ObjectType.GrainTriangle;
            setTriangleMode(_isTriangleMode);
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
            pointAX.IsEnabled                = enabled;
            pointAY.IsEnabled                = enabled;
            pointBX.IsEnabled                = enabled;
            pointBY.IsEnabled                = enabled;
            pointCX.IsEnabled                = enabled;
            pointCY.IsEnabled                = enabled;
        }

        private void setTriangleMode(bool isTriangleMode) {
            Visibility triangleControlsVisibility = isTriangleMode ? Visibility.Visible : Visibility.Hidden;
            Visibility otherControlVisibility = isTriangleMode ? Visibility.Hidden : Visibility.Visible;
            labelPoints.Visibility = triangleControlsVisibility;
            pointAX.Visibility = triangleControlsVisibility;
            pointAY.Visibility = triangleControlsVisibility;
            pointBX.Visibility = triangleControlsVisibility;
            pointBY.Visibility = triangleControlsVisibility;
            pointCX.Visibility = triangleControlsVisibility;
            pointCY.Visibility = triangleControlsVisibility;

            positionLabel.Visibility = otherControlVisibility;
            xPositionLabel.Visibility = otherControlVisibility;
            yPositionLabel.Visibility = otherControlVisibility;
            tbXPosition.Visibility = otherControlVisibility;
            tbYPosition.Visibility = otherControlVisibility;
            sizeLabel.Visibility = otherControlVisibility;
            heightLabel.Visibility = otherControlVisibility;
            widthLabel.Visibility = otherControlVisibility;
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
            if (double.TryParse(tbTemperature.Text, out double temperature)) _selectedObject.Temperature = temperature;
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
            pointAX.Background               = Brushes.White;
            pointAY.Background               = Brushes.White;
            pointBX.Background               = Brushes.White;
            pointBY.Background               = Brushes.White;
            pointCX.Background               = Brushes.White;
            pointCY.Background               = Brushes.White;
            tbHeatCapacity.Background        = Brushes.White;
        }

        private void tbMass_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if(!basicInputCheck(tbMass, e))                     return;
            tbMass.Background = Brushes.White;
            if (double.TryParse(tbMass.Text, out double mass)) _selectedObject.Mass = mass;
            else tbMass.Background = Brushes.Red;
        }

        private void pointAX_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            //// TODO: Check for available position
            // Cast to GrainTriangle, set Point
            GrainTriangle triangle = (GrainTriangle)_selectedObject;
            try {
                Point pointA = new Point(double.Parse(pointAX.Text), triangle.PointA.Y);
                triangle.PointA = pointA;
            }catch(Exception ex) {
               
            }
        }

        private void pointAY_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            //// TODO: Check for available position
            GrainTriangle triangle = (GrainTriangle)_selectedObject;
            try {
                Point pointA = new Point(triangle.PointA.X, double.Parse(pointAY.Text));
                triangle.PointA = pointA;
            }catch(Exception ex) {

            }
        }

        private void pointBX_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            //// TODO: Check for available position
            GrainTriangle triangle = (GrainTriangle)_selectedObject;
            try {
                Point pointB = new Point(double.Parse(pointBX.Text), triangle.PointB.Y);
                triangle.PointA = pointB;
            } catch (Exception ex) {

            }
        }
        private void pointBY_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            //// TODO: Check for available position
            GrainTriangle triangle = (GrainTriangle)_selectedObject;
            try {
                Point pointB = new Point(triangle.PointB.X, double.Parse(pointBY.Text));
                triangle.PointB = pointB;
            } catch(Exception ex) {

            }
        }

        private void pointCX_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            //// TODO: Check for available position
            GrainTriangle triangle = (GrainTriangle)_selectedObject;
            try {
                Point pointC = new Point(double.Parse(pointCX.Text), triangle.PointC.Y);
                triangle.PointC = pointC;
            }catch(Exception ex) {

            }
        }

        private void pointCY_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            //// TODO: Check for available position
            GrainTriangle triangle = (GrainTriangle)_selectedObject;
            try {
                Point pointC = new Point(triangle.PointC.X, double.Parse(pointCY.Text));
                triangle.PointC = pointC;
            }catch(Exception ex) {

            }
        }

        private void tbHeatCapacity_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if(!basicInputCheck(tbHeatCapacity, e))             return;
            tbHeatCapacity.Background = Brushes.White;
            if (double.TryParse(tbHeatCapacity.Text, out double heatCapacity)) _selectedObject.SpecificHeatCapacity = heatCapacity;
            else tbHeatCapacity.Background = Brushes.Red;
        }
    }
}
