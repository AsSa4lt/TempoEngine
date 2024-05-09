﻿using System;
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
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e) {
            // Set the default values for the textboxes
            tbName.Text = _newTriangle.Name;

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
