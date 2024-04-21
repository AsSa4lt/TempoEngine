using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TempoEngine.Util;

namespace TempoEngine.UIControls{
    public class CanvasManager{
        public int CurrentLeftXIndex { get; private set; }
        public int CurrentRightXIndex { get; private set; }
        public int CurrentTopYIndex { get; private set; }
        public int CurrentBottomYIndex { get; private set; }

        public CanvasManager() {
            ResetZoom();
        }

        public void ResetZoom() {
            CurrentLeftXIndex = -100;
            CurrentRightXIndex = 100;
            CurrentTopYIndex = 100;
            CurrentBottomYIndex = -100;
        }

        private int getStep() {
            int deltaX = CurrentRightXIndex - CurrentLeftXIndex;
            int step = 1;
            if (deltaX > 1000) step = 100;
            else if (deltaX > 500) step = 50;
            else if (deltaX > 200) step = 20;
            else if (deltaX > 100) step = 10;
            else if (deltaX > 50) step = 5;
            else if (deltaX > 20) step = 2;
            else if (deltaX > 10) step = 1;
            return step;
        }

        public void Move(System.Windows.Input.Key key) {
            int width = CurrentRightXIndex - CurrentLeftXIndex;
            int height = CurrentTopYIndex - CurrentBottomYIndex;
            int step = getStep();

            switch (key) {
                case System.Windows.Input.Key.A:
                    CurrentLeftXIndex -= step;
                    CurrentRightXIndex -= step;
                    break;
                case System.Windows.Input.Key.D:
                    CurrentLeftXIndex += step;
                    CurrentRightXIndex += step;
                    break;
                case System.Windows.Input.Key.W:
                    CurrentTopYIndex += step;
                    CurrentBottomYIndex += step;
                    break;
                case System.Windows.Input.Key.S:
                    CurrentTopYIndex -= step;
                    CurrentBottomYIndex -= step;
                    break;
            }

            // check if we are out of bounds
            if (CurrentLeftXIndex < CanvasData.MinLeftXIndex) {
                CurrentLeftXIndex = CanvasData.MinLeftXIndex;
                CurrentRightXIndex = CurrentLeftXIndex + width;
            }
            if (CurrentRightXIndex > CanvasData.MaxRightXIndex) {
                CurrentRightXIndex = CanvasData.MaxRightXIndex;
                CurrentLeftXIndex = CurrentRightXIndex - width;
            }
            if (CurrentTopYIndex > CanvasData.MaxYTopIndex) {
                CurrentTopYIndex = CanvasData.MaxYTopIndex;
                CurrentBottomYIndex = CurrentTopYIndex - height;
            }
            if (CurrentBottomYIndex < CanvasData.MinYBottomIndex) {
                CurrentBottomYIndex = CanvasData.MinYBottomIndex;
                CurrentTopYIndex = CurrentBottomYIndex + height;
            }
        }

        public void ZoomToArea(Vector2 topLeft, Vector2 bottomRight) {
            CurrentLeftXIndex = (int)topLeft.X;
            CurrentRightXIndex = (int)bottomRight.X;
            CurrentTopYIndex = (int)bottomRight.Y;
            CurrentBottomYIndex = (int)topLeft.Y;
            // check if we are out of bounds
            if (CurrentLeftXIndex < CanvasData.MinLeftXIndex) {
                CurrentLeftXIndex = CanvasData.MinLeftXIndex;
            }
            if (CurrentRightXIndex > CanvasData.MaxRightXIndex) {
                CurrentRightXIndex = CanvasData.MaxRightXIndex;
            }
            if (CurrentTopYIndex > CanvasData.MaxYTopIndex) {
                CurrentTopYIndex = CanvasData.MaxYTopIndex;
            }
            if (CurrentBottomYIndex < CanvasData.MinYBottomIndex) {
                CurrentBottomYIndex = CanvasData.MinYBottomIndex;
            }
            // check for distance between indexes, if it is smaller than 10, we need to adjust indexes
            if (CurrentRightXIndex - CurrentLeftXIndex < 10) {
                int delta = 10 - (CurrentRightXIndex - CurrentLeftXIndex);
                CurrentRightXIndex += delta / 2;
                CurrentLeftXIndex -= delta / 2;
            }
            if (CurrentTopYIndex - CurrentBottomYIndex < 10) {
                int delta = 10 - (CurrentTopYIndex - CurrentBottomYIndex);
                CurrentTopYIndex += delta / 2;
                CurrentBottomYIndex -= delta / 2;
            }
        }

        public void ZoomIn(int delta) {
            int xSize = CurrentRightXIndex - CurrentLeftXIndex;
            int ySize = CurrentTopYIndex - CurrentBottomYIndex;

            if (xSize == CanvasData.MinRightXIndex - CanvasData.MaxLeftXIndex || ySize == CanvasData.MinYTopIndex - CanvasData.MaxYBottomIndex) return;
            int xZoomDelta = -(xSize * delta / 100 - xSize) / 5;
            int yZoomDelta = -(ySize * delta / 100 - ySize) / 5;

            if (xZoomDelta >= 0 || yZoomDelta >= 0) return;

            CurrentRightXIndex = Math.Max(CurrentRightXIndex + xZoomDelta, CanvasData.MinRightXIndex);
            CurrentLeftXIndex = Math.Min(CurrentLeftXIndex - xZoomDelta, CanvasData.MaxLeftXIndex);
            CurrentTopYIndex = Math.Max(CurrentTopYIndex + yZoomDelta, CanvasData.MinYTopIndex);
            CurrentBottomYIndex = Math.Min(CurrentBottomYIndex - yZoomDelta, CanvasData.MaxYBottomIndex);
        }

        public void ZoomOut(int delta) {
            // calculate new indexes depending on delta, but not more than max indexes
            int xSize = CurrentRightXIndex - CurrentLeftXIndex;
            int ySize = CurrentTopYIndex - CurrentBottomYIndex;

            if (xSize == CanvasData.MaxRightXIndex - CanvasData.MinLeftXIndex || ySize == CanvasData.MaxYTopIndex - CanvasData.MinYBottomIndex)
                return;

            int xZoomDelta = -(xSize * delta / 100 + xSize) / 2;
            int yZoomDelta = -(ySize * delta / 100 + ySize) / 2;

            if (xZoomDelta <= 0 || yZoomDelta <= 0){
                // then make it 10 and adjust indexes
                xZoomDelta = 3;
                // adjust y zoom delta to keep aspect ratio
                yZoomDelta = (int)(xZoomDelta * (CurrentTopYIndex - CurrentBottomYIndex) / (CurrentRightXIndex - CurrentLeftXIndex));
            }
            CurrentRightXIndex = Math.Min(CurrentRightXIndex + xZoomDelta, CanvasData.MaxRightXIndex);
            CurrentLeftXIndex = Math.Max(CurrentLeftXIndex - xZoomDelta, CanvasData.MinLeftXIndex);
            CurrentTopYIndex = Math.Min(CurrentTopYIndex + yZoomDelta, CanvasData.MaxYTopIndex);
            CurrentBottomYIndex = Math.Max(CurrentBottomYIndex - yZoomDelta, CanvasData.MinYBottomIndex);
        }

        public void AdjustForAspectRatio(double width, double height) {
            // Adjust indexes based on aspect ratio
            // if canvas is not square, e need to adjust indexes for Y axis
            double ratio = width / height;

            // get needed distance for Y axis
            int yDistance = (int)((CurrentRightXIndex - CurrentLeftXIndex) / ratio);
            // get current distance for Y axis
            int currentYDistance = CurrentTopYIndex - CurrentBottomYIndex;
            // calculate new indexes for Y axis
            int yDelta = (yDistance - currentYDistance) / 2;
            CurrentTopYIndex += yDelta;
            CurrentBottomYIndex -= yDelta;
        }
    }
}
