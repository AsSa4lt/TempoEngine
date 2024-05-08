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

namespace TempoEngine.UIControls {
    /// <summary>
    /// Interaction logic for AddTriangleWIndow.xaml
    /// </summary>
    public partial class AddTriangleWIndow : Window {
        public AddTriangleWIndow() {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e) {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            // Close the window
            Close();
        }
    }
}
