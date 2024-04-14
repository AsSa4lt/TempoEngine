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

        private readonly ZoomManager _zoomManager;
        private readonly GridDrawer _gridDrawer;

        private readonly bool drawGrid = true;

        public EngineCanvas() : base() {
            _zoomManager = new ZoomManager();
            _gridDrawer = new GridDrawer(this);

            Loaded += (sender, args) => {
                _zoomManager.AdjustForAspectRatio(ActualWidth, ActualHeight);
                Update();
            };
        }

        // on resize event, we need to recalculate indexes
        protected override void OnRenderSizeChanged(System.Windows.SizeChangedInfo sizeInfo) {
            base.OnRenderSizeChanged(sizeInfo);
            _zoomManager.AdjustForAspectRatio(ActualWidth, ActualHeight);
            Update();
        }

        // handle mouse scroll event
        protected override void OnMouseWheel(System.Windows.Input.MouseWheelEventArgs e) {
            base.OnMouseWheel(e);
            if (e.Delta > 0) {
                _zoomManager.ZoomIn(e.Delta);
            } else {
                _zoomManager.ZoomOut(e.Delta);
            }

            Update();
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
                _gridDrawer.DrawGrid(_zoomManager);
        }

    }
}
