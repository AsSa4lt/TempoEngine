using System.Numerics;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TempoEngine.Engine;
using Point = System.Windows.Point;


namespace TempoEngine.UIControls {
    public class EngineCanvas : Canvas {

        private readonly CanvasManager _canvasManager;
        private readonly GridDrawer _gridDrawer;

        private readonly bool drawGrid = true;

        public EngineCanvas() : base() {
            _canvasManager = new CanvasManager();
            _gridDrawer = new GridDrawer(this);
            Focusable = true;

            Loaded += (sender, args) => {
                _canvasManager.AdjustForAspectRatio(ActualWidth, ActualHeight);
                Update();
            };
        }

        // on resize event, we need to recalculate indexes
        protected override void OnRenderSizeChanged(System.Windows.SizeChangedInfo sizeInfo) {
            base.OnRenderSizeChanged(sizeInfo);
            Update();
        }

        // handle mouse scroll event
        protected override void OnMouseWheel(System.Windows.Input.MouseWheelEventArgs e) {
            base.OnMouseWheel(e);
            if (e.Delta > 0)
                _canvasManager.ZoomIn(e.Delta);
            else
                _canvasManager.ZoomOut(e.Delta);

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

        private void SetClipGeometry() {
            this.Clip = new RectangleGeometry(new System.Windows.Rect(0, 0, this.ActualWidth, this.ActualHeight));
        }

        public void ZoomToObject(EngineObject obj) {
            // get object data
            Vector2 topLeft, bottomRight;
            obj.GetObjectVisibleArea(out topLeft, out bottomRight);
            _canvasManager.ZoomToArea(topLeft, bottomRight);
            Update();
        }


        private Point ConvertToScreenCoordinates(Point point) {
            // get ActualWidth and Height
            double width = ActualWidth;
            double height = ActualHeight;
            // get manager indexes
            int leftX = _canvasManager.CurrentLeftXIndex;
            int rightX = _canvasManager.CurrentRightXIndex;
            int topY = _canvasManager.CurrentTopYIndex;
            int bottomY = _canvasManager.CurrentBottomYIndex;
            // convert point to screen coordinates
            double x = (point.X - leftX) * width / (rightX - leftX);
            double y = height - (point.Y - bottomY) * height / (topY - bottomY);
            return new Point(x, y);
        }

        protected void Update() {
            // clear canvas
            Children.Clear();
            _canvasManager.AdjustForAspectRatio(ActualWidth, ActualHeight);

            List<EngineObject> objects = Engine.Engine.GetVisibleObjects(_canvasManager);

            // draw grid
            if (drawGrid)
                _gridDrawer.DrawGrid(_canvasManager);


            // get polygons
            foreach (var obj in objects) {
                Polygon polygon = obj.GetPolygon();
                // convert polygon points to screen coordinates
                for (int i = 0; i < polygon.Points.Count; i++) {
                    polygon.Points[i] = ConvertToScreenCoordinates(polygon.Points[i]);
                }
                // add polygon to canvas
                Children.Add(polygon);
            }
                
            SetClipGeometry();
        }

    }
}
