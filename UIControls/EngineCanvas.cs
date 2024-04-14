using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using TempoEngine.Util;

namespace TempoEngine.UIControls {
    internal class EngineCanvas : Canvas {

        private int _currentLeftXIndex;
        private int _currentRightXIndex;
        private int _currentTopYIndex;
        private int _currentBottomYIndex;

        private bool drawGrid = true;

        public EngineCanvas() : base() {
            _currentLeftXIndex = -100;
            _currentRightXIndex = 100;

            _currentTopYIndex = 100;
            _currentBottomYIndex = -100;

            Loaded += (sender, args) => {
                SetCurrentIndeces();
                Update();
            };
        }


        public void SetCurrentIndeces() {
            
            // if canvas is not square, e need to adjust indexes for Y axis
            double width = ActualWidth;
            double height = ActualHeight;
            double ratio = width / height;
             
            // get needed distance for Y axis
            int yDistance = (int)((_currentRightXIndex - _currentLeftXIndex) / ratio);
            // get current distance for Y axis
            int currentYDistance = _currentTopYIndex - _currentBottomYIndex;
            // calculate new indexes for Y axis
            int yDelta = (yDistance - currentYDistance) / 2;
            _currentTopYIndex += yDelta;
            _currentBottomYIndex -= yDelta;

        }

        // on resize event, we need to recalculate indexes
        protected override void OnRenderSizeChanged(System.Windows.SizeChangedInfo sizeInfo) {
            base.OnRenderSizeChanged(sizeInfo);
            SetCurrentIndeces();
            Update();
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

            if(xSize == CanvasParameters.MaxRightXIndex - CanvasParameters.MinLeftXIndex || ySize == CanvasParameters.MaxYTopIndex - CanvasParameters.MinYBottomIndex)
                return;

            int xZoomDelta = -(xSize * delta / 100 + xSize) /2;
            int yZoomDelta = -(ySize * delta / 100 + ySize) / 2;

            if (xZoomDelta <= 0 || yZoomDelta <= 0)
                return;    
            _currentRightXIndex = Math.Min(_currentRightXIndex + xZoomDelta, CanvasParameters.MaxRightXIndex);
            _currentLeftXIndex = Math.Max(_currentLeftXIndex - xZoomDelta, CanvasParameters.MinLeftXIndex);
            _currentTopYIndex = Math.Min(_currentTopYIndex + yZoomDelta, CanvasParameters.MaxYTopIndex);
            _currentBottomYIndex = Math.Max(_currentBottomYIndex - yZoomDelta, CanvasParameters.MinYBottomIndex);
        }

        private void ZoomIn(int delta) {
            int xSize = _currentRightXIndex - _currentLeftXIndex;
            int ySize = _currentTopYIndex - _currentBottomYIndex;

            if (xSize == CanvasParameters.MinRightXIndex - CanvasParameters.MaxLeftXIndex || ySize == CanvasParameters.MinYTopIndex - CanvasParameters.MaxYBottomIndex) return;
            int xZoomDelta = -(xSize * delta / 100 - xSize) / 5;
            int yZoomDelta = -(ySize * delta / 100 - ySize) / 5;

            if (xZoomDelta >= 0 || yZoomDelta >= 0) return;

            _currentRightXIndex = Math.Max(_currentRightXIndex + xZoomDelta, CanvasParameters.MinRightXIndex);
            _currentLeftXIndex = Math.Min(_currentLeftXIndex - xZoomDelta, CanvasParameters.MaxLeftXIndex);
            _currentTopYIndex = Math.Max(_currentTopYIndex + yZoomDelta, CanvasParameters.MinYTopIndex);
            _currentBottomYIndex = Math.Min(_currentBottomYIndex - yZoomDelta, CanvasParameters.MaxYBottomIndex);


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
            if (deltaX > 1000)     step = 100;
            else if (deltaX > 500) step = 50;
            else if (deltaX > 200) step = 20;
            else if (deltaX > 100) step = 10;
            else if (deltaX > 50)  step = 5;
            else if (deltaX > 20)  step = 2;
            else if (deltaX > 10)  step = 1;


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
                Line line = new Line {
                    Stroke = System.Windows.Media.Brushes.LightGray,
                    X1 = x,
                    X2 = x,
                    Y1 = 0,
                    Y2 = height
                };
                Children.Add(line);
                x += width / deltaX * step;
            }
            // draw labels for vertical lines
            x = (leftX - _currentLeftXIndex) * width / deltaX;
            while (x < width) {
                TextBlock textBlock = new TextBlock {
                    Text = leftX.ToString(),
                    Foreground = System.Windows.Media.Brushes.Black
                };
                SetLeft(textBlock, x);
                SetTop(textBlock, height - 20);
                Children.Add(textBlock);
                leftX += step;
                x += width / deltaX * step;
            }

            // draw horizontal lines
            // get canvas size
            // find nearest step for bottom y index
            int bottomY = _currentBottomYIndex - _currentBottomYIndex % step;
            // convert bottom y index to screen coordinates
            double y = (bottomY - _currentBottomYIndex) * height / (_currentTopYIndex - _currentBottomYIndex);
            // draw lines
            while (y < height) {
                Line line = new() {
                    Stroke = System.Windows.Media.Brushes.LightGray,
                    X1 = 0,
                    X2 = width,
                    Y1 = y,
                    Y2 = y
                };
                Children.Add(line);
                y += height / (_currentTopYIndex - _currentBottomYIndex) * step;
            }

            // draw labels for horizontal lines
            y = (bottomY - _currentBottomYIndex) * height / (_currentTopYIndex - _currentBottomYIndex);
            while (y < height) {
                TextBlock textBlock = new() {
                    Text = bottomY.ToString(),
                    Foreground = System.Windows.Media.Brushes.Black
                };
                SetLeft(textBlock, 0);
                SetTop(textBlock, y);
                Children.Add(textBlock);
                bottomY += step;
                y += height / (_currentTopYIndex - _currentBottomYIndex) * step;
            }

        }

    }
}
