using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using TempoEngine.Util;

namespace TempoEngine.UIControls {
    public class EngineCanvas : Canvas {

        private readonly CanvasManager _canvasManager;
        private readonly GridDrawer _gridDrawer;

        private readonly bool drawGrid = true;

        public EngineCanvas() : base() {
            _canvasManager = new CanvasManager();
            _gridDrawer = new GridDrawer(this);
            this.Focusable = true;

            Loaded += (sender, args) => {
                _canvasManager.AdjustForAspectRatio(ActualWidth, ActualHeight);
                Update();
            };
        }

        // on resize event, we need to recalculate indexes
        protected override void OnRenderSizeChanged(System.Windows.SizeChangedInfo sizeInfo) {
            base.OnRenderSizeChanged(sizeInfo);
            _canvasManager.AdjustForAspectRatio(ActualWidth, ActualHeight);
            Update();
        }

        // handle mouse scroll event
        protected override void OnMouseWheel(System.Windows.Input.MouseWheelEventArgs e) {
            base.OnMouseWheel(e);
            if (e.Delta > 0)
                _canvasManager.ZoomIn(e.Delta);
            else
                _canvasManager.ZoomOut(e.Delta);

            _canvasManager.AdjustForAspectRatio(ActualWidth, ActualHeight);
            Update();
        }

        // handle mouse click event
        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e) {
            base.OnMouseDown(e);
            Focus();

            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed) {
                //LeftClick();
            } else if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed) {
                //RightClick();
            }
        }


        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e) {
            base.OnKeyDown(e);
            _canvasManager.Move(e.Key);
            Update();
        }



        protected void Update() {
           // clear canvas
           Children.Clear();
                
            // draw grid
            if(drawGrid)
                _gridDrawer.DrawGrid(_canvasManager);
        }

    }
}
