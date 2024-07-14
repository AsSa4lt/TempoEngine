using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace TempoEngine.UIControls{
    /**
     * \class GridDrawer
     * \brief Represents the grid drawer.
     * 
     * The GridDrawer class provides methods for drawing the grid.
     */
    public class GridDrawer {

        private readonly Canvas _canvas;  /// Canvas

        /**
         * \brief Initializes a new instance of the GridDrawer class.
         * \param canvas Canvas.
         */
        public GridDrawer(Canvas canvas) {
            _canvas = canvas;
        }

        /**
         * \brief Draws the grid.
         * Draw vertical and horizontal grid lines
         * Draw labels
         * \param manager Canvas manager.
         */
        public void DrawGrid(CanvasManager manager) {
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

            if(x < 0) x += width / deltaX * step;
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
            if(x < 0) x += width / deltaX * step;

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

            // Starting from the top of the Canvas, which corresponds to the highest value of Y in the coordinate system
            double yScreen = 0;
            int currentYIndex = manager.CurrentTopYIndex;

            // draw horizontal lines from top to bottom
            while (yScreen < height) {
                Line line = new() {
                    Stroke = System.Windows.Media.Brushes.LightGray,
                    X1 = 0,
                    X2 = width,
                    Y1 = yScreen,
                    Y2 = yScreen
                };
                _canvas.Children.Add(line);

                // Move to the next line position on the screen
                yScreen += height / (manager.CurrentTopYIndex - manager.CurrentBottomYIndex) * step;

                // Decrement the Y index for the next line
                currentYIndex -= step;
            }

            // Reset the screen Y position and Y index for labels
            yScreen = 0;
            currentYIndex = manager.CurrentTopYIndex;

            // draw labels for horizontal lines from top to bottom (bigger number on top)
            while (yScreen < height) {
                TextBlock textBlock = new() {
                    Text = currentYIndex.ToString(),
                    Foreground = System.Windows.Media.Brushes.Black
                };
                Canvas.SetLeft(textBlock, 0); // Align labels to the left of the Canvas
                Canvas.SetTop(textBlock, yScreen); // Position the label at the current screen Y coordinate
                _canvas.Children.Add(textBlock);

                // Move to the next label position on the screen
                yScreen += height / (manager.CurrentTopYIndex - manager.CurrentBottomYIndex) * step;

                // Decrement the Y index for the next label
                currentYIndex -= step;
            }

        }
    }
}
