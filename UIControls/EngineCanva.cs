using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TempoEngine.UIControls {
    internal class EngineCanva : Canvas {
        public static int MinXIndex = -1000;
        public static int MaxXIndex = 1000;
        public static int MinYIndex = -1000;
        public static int MaxYIndex = 1000;

        private int _currentLeftXIndex;
        private int _currentRightXIndex;
        private int _currentTopYIndex;
        private int _currentBottomYIndex;

        public EngineCanva() : base() {
            // call base constructor
            _currentLeftXIndex = -100;
            _currentRightXIndex = 100;
            _currentTopYIndex = 100;
            _currentBottomYIndex = -100;
        }

        // handle mouse scroll event
        protected override void OnMouseWheel(System.Windows.Input.MouseWheelEventArgs e) {
            base.OnMouseWheel(e);
            if (e.Delta > 0) {
                ZoomIn(e.Delta);
            } else {
                ZoomOut(e.Delta);
            }
        }

        // handle mouse click event
        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e) {
            base.OnMouseDown(e);
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed) {
                //LeftClick();
            } else if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed) {
                //RightClick();
            }
        }

        private void ZoomIn(int delta) {

        }

        private void ZoomOut(int delta) {

        }

        protected void Update() {

        }

    }
}
