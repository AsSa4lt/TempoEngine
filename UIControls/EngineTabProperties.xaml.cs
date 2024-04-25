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

            tbName.Text = _selectedObject.Name;

        }

        private void SetObjectParameters() {
            if (_selectedObject == null) return;
            tbName.Text = _selectedObject.Name;
        }

        public void SetObject(EngineObject obj) {
            _selectedObject = obj;
        }

        // function to set are fields enabled or disabled
        private void SetFieldsEnabled(bool enabled) {
            
        }

        private void tbName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (_selectedObject == null)                        return;
            if (e.Key != Key.Enter)                             return;
            if (!Engine.Engine.isNameAvailable(tbName.Text))    return;
            _selectedObject.Name = tbName.Text;
        }

        private void tbTemperature_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if(_selectedObject == null) return;
            if(e.Key != Key.Enter) return;
            if(double.TryParse(tbTemperature.Text, out double temperature)) {
                _selectedObject.Temperature = temperature;
            }
        }
    }
}
