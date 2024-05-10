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
using Point = System.Windows.Point;

namespace TempoEngine.UIControls {
    /// <summary>
    /// Interaction logic for AddTriangleWIndow.xaml
    /// </summary>
    public partial class AddTriangleWIndow : Window {
        GrainTriangle _newTriangle;
        public AddTriangleWIndow() {
            InitializeComponent();
            _newTriangle = new GrainTriangle("New Triangle", new Point(0, 0), new Point(0, 0), new Point(0, 0));
            int i = 0;
            while (!Engine.Engine.IsNameAvailable($"New triangle {1}")) i++;
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
        }

        private void AddButton_Click(object sender, RoutedEventArgs e) {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            // Close the window
            Close();
        }

        private void Button_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if(e.Key != Key.Enter) return;
        }
    }
}
