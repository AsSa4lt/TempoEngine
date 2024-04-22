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
        public EngineTabProperties() : base() {
            InitializeComponent();
            // On Loaded event, we need to update the properties tab
            Loaded += (sender, args) => {
                Update();
            };
        }

        public void Update() {
            if(_selectedObject == null) {

            }
        }

        // function to set are fields enabled or disabled
        private void SetFieldsEnabled(bool enabled) {
            
        }
    }
}
