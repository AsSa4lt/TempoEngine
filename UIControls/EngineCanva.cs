using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace TempoEngine.UIControls {
    internal class EngineCanva : Canvas {
        public static int MinXIndex = -1000;
        public static int MaxXIndex = 1000;
        public static int MinYIndex = -1000;
        public static int MaxYIndex = 1000;
        public static int ZoomFactor = 100;

        private int _currentLeftXIndex;
        private int _currentRightXIndex;
        private int _currentTopYIndex;
        private int _currentBottomYIndex;

        private bool drawGrid = true;

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

            Update();
        }

        private void ZoomOut(int delta) {
            // calculate new indexes depending on delta, but not more than max indexes
            int xSize = _currentRightXIndex - _currentLeftXIndex;
            int ySize = _currentTopYIndex - _currentBottomYIndex;

            if(xSize == MaxXIndex - MinXIndex || ySize == MaxYIndex - MinYIndex)
                return;

            int xZoomDelta = -(xSize * delta / 100 + xSize) /2;
            int yZoomDelta = -(ySize * delta / 100 + ySize) /2;
            _currentRightXIndex = Math.Min(_currentRightXIndex + xZoomDelta, MaxXIndex);
            _currentLeftXIndex = Math.Max(_currentLeftXIndex - xZoomDelta, MinXIndex);
            _currentTopYIndex = Math.Min(_currentTopYIndex + yZoomDelta, MaxYIndex);
            _currentBottomYIndex = Math.Max(_currentBottomYIndex - yZoomDelta, MinYIndex);
        }

        private void ZoomIn(int delta) {
/**/

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



        protected void Update() {
           // clear canvas
           Children.Clear();
                
            // draw grid
            if(drawGrid)
                DrawGrid();
        }

        private void DrawGrid() {
            // grid will be drawn as vertical and horizontal lines
            // and labels on the left and bottom sides of the screen

            // calculate step for grid lines
            int step = 1;
            
            int deltaX = _currentRightXIndex - _currentLeftXIndex;
            if (deltaX > 1000)
                step = 100;
            else if (deltaX > 100)
                step = 10;
            else if (deltaX > 10)
                step = 1;


            // draw vertical lines
            // get canvas size
            double width = ActualWidth;
            double height = ActualHeight;
            // find nearest step for left x index
            int leftX = _currentLeftXIndex - _currentLeftXIndex % step;
            // convert left x index to screen coordinates
            double x = (leftX - _currentLeftXIndex) * width / deltaX;
            // draw lines
            while (x < width) {
                Line line = new Line();
                line.Stroke = System.Windows.Media.Brushes.LightGray;
                line.X1 = x;
                line.X2 = x;
                line.Y1 = 0;
                line.Y2 = height;
                Children.Add(line);
                x += width / deltaX * step;
            }


            

        }

    }
}
