using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoEngine.Util;

namespace TempoEngine.UIControls{
    public class ZoomManager{
        public int CurrentLeftXIndex { get; private set; }
        public int CurrentRightXIndex { get; private set; }
        public int CurrentTopYIndex { get; private set; }
        public int CurrentBottomYIndex { get; private set; }

        public ZoomManager() {
            ResetZoom();
        }

        public void ResetZoom() {
            CurrentLeftXIndex = -100;
            CurrentRightXIndex = 100;
            CurrentTopYIndex = 100;
            CurrentBottomYIndex = -100;
        }

        public void ZoomIn(int delta) {
            int xSize = CurrentRightXIndex - CurrentLeftXIndex;
            int ySize = CurrentTopYIndex - CurrentBottomYIndex;

            if (xSize == CanvasParameters.MinRightXIndex - CanvasParameters.MaxLeftXIndex || ySize == CanvasParameters.MinYTopIndex - CanvasParameters.MaxYBottomIndex) return;
            int xZoomDelta = -(xSize * delta / 100 - xSize) / 5;
            int yZoomDelta = -(ySize * delta / 100 - ySize) / 5;

            if (xZoomDelta >= 0 || yZoomDelta >= 0) return;

            CurrentRightXIndex = Math.Max(CurrentRightXIndex + xZoomDelta, CanvasParameters.MinRightXIndex);
            CurrentLeftXIndex = Math.Min(CurrentLeftXIndex - xZoomDelta, CanvasParameters.MaxLeftXIndex);
            CurrentTopYIndex = Math.Max(CurrentTopYIndex + yZoomDelta, CanvasParameters.MinYTopIndex);
            CurrentBottomYIndex = Math.Min(CurrentBottomYIndex - yZoomDelta, CanvasParameters.MaxYBottomIndex);
        }

        public void ZoomOut(int delta) {
            // calculate new indexes depending on delta, but not more than max indexes
            int xSize = CurrentRightXIndex - CurrentLeftXIndex;
            int ySize = CurrentTopYIndex - CurrentBottomYIndex;

            if (xSize == CanvasParameters.MaxRightXIndex - CanvasParameters.MinLeftXIndex || ySize == CanvasParameters.MaxYTopIndex - CanvasParameters.MinYBottomIndex)
                return;

            int xZoomDelta = -(xSize * delta / 100 + xSize) / 2;
            int yZoomDelta = -(ySize * delta / 100 + ySize) / 2;

            if (xZoomDelta <= 0 || yZoomDelta <= 0)
                return;
            CurrentRightXIndex = Math.Min(CurrentRightXIndex + xZoomDelta, CanvasParameters.MaxRightXIndex);
            CurrentLeftXIndex = Math.Max(CurrentLeftXIndex - xZoomDelta, CanvasParameters.MinLeftXIndex);
            CurrentTopYIndex = Math.Min(CurrentTopYIndex + yZoomDelta, CanvasParameters.MaxYTopIndex);
            CurrentBottomYIndex = Math.Max(CurrentBottomYIndex - yZoomDelta, CanvasParameters.MinYBottomIndex);
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
