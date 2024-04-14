using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace TempoEngine.UIControls
{
    public class GridDrawer {

        private readonly Canvas _canvas;

        public GridDrawer(Canvas canvas) {
            _canvas = canvas;
        }

        public void DrawGrid(ZoomManager manager) {
            int step = 1;

            int deltaX = manager.CurrentRightXIndex - manager.CurrentLeftXIndex;
            if (deltaX > 1000) step = 100;
            else if (deltaX > 500) step = 50;
            else if (deltaX > 200) step = 20;
            else if (deltaX > 100) step = 10;
            else if (deltaX > 50) step = 5;
            else if (deltaX > 20) step = 2;
            else if (deltaX > 10) step = 1;


            // draw vertical lines
            // get canvas size
            double width = _canvas.ActualWidth;
            double height = _canvas.ActualHeight;
            // find nearest step for left x index
            int leftX = manager.CurrentLeftXIndex - manager.CurrentLeftXIndex % step;
            // convert left x index to screen coordinates
            double x = (leftX - manager.CurrentLeftXIndex) * width / deltaX;
            // draw lines
            while (x < width) {
                Line line = new() {
                    Stroke = System.Windows.Media.Brushes.LightGray,
                    X1 = x,
                    X2 = x,
                    Y1 = 0,
                    Y2 = height
                };
                _canvas.Children.Add(line);
                x += width / deltaX * step;
            }
            // draw labels for vertical lines
            x = (leftX - manager.CurrentLeftXIndex) * width / deltaX;
            while (x < width) {
                TextBlock textBlock = new() {
                    Text = leftX.ToString(),
                    Foreground = System.Windows.Media.Brushes.Black
                };
                Canvas.SetLeft(textBlock, x);
                Canvas.SetTop(textBlock, height - 20);
                _canvas.Children.Add(textBlock);
                leftX += step;
                x += width / deltaX * step;
            }

            // draw horizontal lines
            // get canvas size
            // find nearest step for bottom y index
            int bottomY = manager.CurrentBottomYIndex - manager.CurrentBottomYIndex % step;
            // convert bottom y index to screen coordinates
            double y = (bottomY - manager.CurrentBottomYIndex) * height / (manager.CurrentTopYIndex - manager.CurrentBottomYIndex);
            // draw lines
            while (y < height) {
                Line line = new() {
                    Stroke = System.Windows.Media.Brushes.LightGray,
                    X1 = 0,
                    X2 = width,
                    Y1 = y,
                    Y2 = y
                };
                _canvas.Children.Add(line);
                y += height / (manager.CurrentTopYIndex - manager.CurrentBottomYIndex) * step;
            }

            // draw labels for horizontal lines
            y = (bottomY - manager.CurrentBottomYIndex) * height / (manager.CurrentTopYIndex - manager.CurrentBottomYIndex);
            while (y < height) {
                TextBlock textBlock = new() {
                    Text = bottomY.ToString(),
                    Foreground = System.Windows.Media.Brushes.Black
                };
                Canvas.SetLeft(textBlock, 0);
                Canvas.SetTop(textBlock, y);
                _canvas.Children.Add(textBlock);
                bottomY += step;
                y += height / (manager.CurrentTopYIndex - manager.CurrentBottomYIndex) * step;
            }
        }
    }
}
