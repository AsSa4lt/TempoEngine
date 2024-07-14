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
using TempoEngine.Engine.Managers;
using Brushes = System.Windows.Media.Brushes;
using MessageBox = System.Windows.MessageBox;
using Point = System.Windows.Point;
using TextBox = System.Windows.Controls.TextBox;
using UserControl = System.Windows.Controls.UserControl;
namespace TempoEngine.UIControls {
    /**
     * \class EngineTabProperties
     * \brief Represents the properties tab for the engine object.
     * 
     * The EngineTabProperties class provides methods for managing the properties tab for the engine object.
     */
    public partial class EngineTabProperties : UserControl {
        private EngineObject? _selectedObject;  // Selected object from object list
        private bool isEnabled = true;          // Is enabled, defined by selected mode and if we have any object selected

        /**
         * Constructor
         */
        public EngineTabProperties() : base() {
            InitializeComponent();
            // On Loaded event, we need to update the properties tab
            Loaded += (sender, args) => {
                Update();
            };
        }

        /**
         * \brief Update fields based on selected objects
         */
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

        /**
         * \brief Enable or disable the properties tab
         * \param IsEnabled Is enabled
         */
        public void Enable(bool IsEnabled) {
            isEnabled = IsEnabled;
            if(!isEnabled) {
                SetFieldsEnabled(false);
            }
        }

        /**
         * \brief Set object parameters
         */
        private void SetObjectParameters() {
            if (_selectedObject == null) return;
            tbName.Text = _selectedObject.Name;
            // show only two decimal places for temperature
            tbTemperature.Text = Math.Round(_selectedObject.CurrentTemperature, 2).ToString();
            tbXPosition.Text = _selectedObject.Position.X.ToString();
            tbYPosition.Text = _selectedObject.Position.Y.ToString();
            tbHeight.Text = _selectedObject.Size.Y.ToString();
            tbWidth.Text = _selectedObject.Size.X.ToString();

            List<Material> materials = MaterialManager.GetMaterials();
            for (int i = 0; i < materials.Count; i++) {
                cbMaterial.Items.Add(materials[i]);
                if (_selectedObject.Material == materials[i]) {
                    cbMaterial.SelectedIndex = i;
                }
            }
        }

        /**
         * \brief Set object
         * \param obj Engine object
         */
        public void SetObject(EngineObject obj) {
            ClearFields();
            _selectedObject = obj;
            if (obj == null) return;
        }

        /**
         * \brief Set fields enabled
         * Based on engine mode and if do we have any object selected enable or disable fields
         * \param enabled Enabled
         */
        private void SetFieldsEnabled(bool enabled) {
            tbName.IsEnabled                 = enabled;
            tbTemperature.IsEnabled          = enabled;
            tbXPosition.IsEnabled            = enabled;
            tbYPosition.IsEnabled            = enabled;
            tbHeight.IsEnabled               = enabled;
            tbWidth.IsEnabled                = enabled;
            cbMaterial.IsEnabled             = enabled;
        }

        /**
         * \brief Name key down
         * Set Name to an object and check it
         * \param sender Sender
         * \param e Key event arguments
         */
        private void tbName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if(!basicInputCheck(tbName, e))                     return;
            if (_selectedObject.Name != tbName.Text && !Engine.Engine.EngineObjectsManager.IsNameAvailable(tbName.Text)) {
                ShowErrorMessageBox("Name is not available");
                tbName.Background = Brushes.Red;
                return;
            }
            _selectedObject.Name = tbName.Text;
            tbName.Background = Brushes.White;
        }

        /**
         * \brief Show error message box
         * \param text Text
         */
        private void ShowErrorMessageBox(string text) {
            MessageBox.Show(text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /**
         * \brief Temperature key down
         * Set temperature to an object and check it
         * \param sender Sender
         * \param e Key event arguments
         */
        private void tbTemperature_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if(!basicInputCheck(tbTemperature, e))              return;
            tbTemperature.Background = Brushes.White;
            if (double.TryParse(tbTemperature.Text, out double temperature)) _selectedObject.SimulationTemperature = temperature;
            else tbTemperature.Background = Brushes.Red;
        }

        /**
         * \brief X position key down
         * Set X position to an object and check it
         * \param sender Sender
         * \param e Key event arguments
         */
        private bool basicInputCheck(TextBox tb, System.Windows.Input.KeyEventArgs e) {
            if(_selectedObject == null)      return false;
            if (e.Key != Key.Enter)          return false;
            else                             return true;
        }

        /**
         * \brief X position key down
         * Set X position to an object and check it
         * \param sender Sender
         * \param e Key event arguments
         */
        private void ClearFields() {
            tbName.Text                      = "";
            tbTemperature.Text               = "";
            tbXPosition.Text                 = "";
            tbYPosition.Text                 = "";
            tbHeight.Text                    = "";
            tbWidth.Text                     = "";

            cbMaterial.Items.Clear();

            tbName.Background                = Brushes.White;
            tbTemperature.Background         = Brushes.White;
            tbXPosition.Background           = Brushes.White;
            tbYPosition.Background           = Brushes.White;
            tbHeight.Background              = Brushes.White;
            tbWidth.Background               = Brushes.White;
            cbMaterial.Background            = Brushes.White;
        }

        /**
         * \brief X position key down
         * Set X position to an object and check it
         * \param sender Sender
         * \param e Key event arguments
         */
        private void tbXPosition_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            try {
                Point pos = new Point(double.Parse(tbXPosition.Text), _selectedObject.Position.Y);
                _selectedObject.Position = pos;
            } catch (Exception ex) {

            }
        }

        /**
         * \brief Y position key down
         * Set Y position to an object and check it
         * \param sender Sender
         * \param e Key event arguments
         */
        private void tbYPosition_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            try {
                Point pointA = new Point(_selectedObject.Position.X, double.Parse(tbYPosition.Text));
                _selectedObject.Position = pointA;
            } catch (Exception ex) {

            }
        }

        /**
         * \brief Width key down
         * Set width to an object and check it
         * \param sender Sender
         * \param e Key event arguments
         */
        private void cbMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if(_selectedObject == null) return;
            if(cbMaterial.Items.Count == 0) return;
            if(cbMaterial.SelectedItem == null) cbMaterial.SelectedIndex = 0;
            _selectedObject.Material = (Material)cbMaterial.SelectedItem;
        }

        /**
         * \brief Width key down
         * Set width to an object and check it
         * \param sender Sender
         * \param e Key event arguments
         */
        private void tbHeight_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            // check if it's a grain square
            // if grain square, then set to 1
            if(_selectedObject == null) return;
            if(_selectedObject.GetObjectType() == ObjectType.GrainSquare) {
                tbHeight.Text = "1";
            }

            try {
                Point size = new Point(_selectedObject.Size.X, double.Parse(tbHeight.Text));
                _selectedObject.Size = size;
            } catch (Exception ex) {

            }
        }

        /**
         * \brief Width key down
         * Set width to an object and check it
         * \param sender Sender
         * \param e Key event arguments
         */
        private void Grid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            // check if it's a grain square
            // if grain square, then set to 1
            if (_selectedObject == null) return;
            if (_selectedObject.GetObjectType() == ObjectType.GrainSquare) {
                tbHeight.Text = "1";
            }

            try {
                Point size = new Point(_selectedObject.Size.X, double.Parse(tbHeight.Text));
                _selectedObject.Size = size;
            } catch (Exception ex) {

            }
        }
    }
}
