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
using UserControl = System.Windows.Controls.UserControl;

namespace TempoEngine.UIControls {
    /// <summary>
    /// Interaction logic for EngineTabProperties.xaml
    /// </summary>
    public partial class EngineTabProperties : UserControl {
        private EngineObject? _selectedObject;
        public Action PropertiesChanged;
        public EngineTabProperties() : base() {
            InitializeComponent();
            // On Loaded event, we need to update the properties tab
            Loaded += (sender, args) => {
                Update();
            };
        }

        public void Update() {
            if(_selectedObject == null) {
                SetFieldsEnabled(false);
                return;
            }

            SetFieldsEnabled(true);

            SetObjectParameters();
        }

        private void SetObjectParameters() {
            if (_selectedObject == null) return;
            tbName.Text = _selectedObject.Name;
            tbTemperature.Text = _selectedObject.Temperature.ToString();
            tbThermalConductivity.Text = _selectedObject.ThermalConductivity.ToString();
        }

        public void SetObject(EngineObject obj) {
            ClearFields();
            _selectedObject = obj;
        }

        // function to set are fields enabled or disabled
        private void SetFieldsEnabled(bool enabled) {
            tbName.IsEnabled = enabled;
            tbTemperature.IsEnabled = enabled;
            tbThermalConductivity.IsEnabled = enabled;
        }

        private void tbName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (_selectedObject == null)                        return;
            if (e.Key != Key.Enter)                             return;
            if (!Engine.Engine.isNameAvailable(tbName.Text))    return;
            _selectedObject.Name = tbName.Text;
        }

        private void tbTemperature_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if(_selectedObject == null)                         return;
            if(e.Key != Key.Enter)                              return;
            if(double.TryParse(tbTemperature.Text, out double temperature)) _selectedObject.Temperature = temperature;
            else  tbTemperature.Background = Brushes.Red;
        }

        private void tbThermalConductivity_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if(_selectedObject == null)                         return;
            if(e.Key != Key.Enter)                              return;
            if(double.TryParse(tbThermalConductivity.Text, out double thermalConductivity)) _selectedObject.ThermalConductivity = thermalConductivity;
            else tbThermalConductivity.Background = Brushes.Red;
        }

        private void ClearFields() {
            tbName.Text = "";
            tbTemperature.Text = "";
            tbThermalConductivity.Text = "";

            tbName.Background = Brushes.White;
            tbTemperature.Background = Brushes.White;
            tbThermalConductivity.Background = Brushes.White;
        }
    }
}
