using log4net;
using System.Diagnostics;
using System.Numerics;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TempoEngine.Engine;
using Brushes = System.Windows.Media.Brushes;
using Point = System.Windows.Point;


namespace TempoEngine.UIControls {

    /**
     * \class EngineCanvas
     * \brief EngineCanvas class
     * Canvas for drawing the simulation
     */
    public class EngineCanvas : Canvas {

        private readonly CanvasManager _canvasManager;   // Canvas manager
        private readonly GridDrawer _gridDrawer;         // Grid drawer
        private static readonly ILog log = LogManager.GetLogger(typeof(EngineCanvas)); // Logger
        private readonly bool drawGrid = true;           // Draw grid

        /**
         * 
         * Constructor
         * On load event, adjust the canvas for aspect ratio and update
         */
        public EngineCanvas() : base() {
            _canvasManager = new CanvasManager();
            _gridDrawer = new GridDrawer(this);
            Focusable = true;
            Background = Brushes.Transparent;

            Loaded += (sender, args) => {
                _canvasManager.AdjustForAspectRatio(ActualWidth, ActualHeight);
                Update();
            };
        }


        /**
         * OnRenderSizeChanged method
         * on resize event, we need to recalculate indexes
         * \param sizeInfo Size changed info
         */
        protected override void OnRenderSizeChanged(System.Windows.SizeChangedInfo sizeInfo) {
            base.OnRenderSizeChanged(sizeInfo);
            Update();
        }


        /**
         * OnMouseWheel method
         * on event, zoom in or out and update objects on canvas
         * \param e Mouse wheel event
         */
        protected override void OnMouseWheel(System.Windows.Input.MouseWheelEventArgs e) {
            base.OnMouseWheel(e);
            if (e.Delta > 0)
                _canvasManager.ZoomIn(e.Delta);
            else
                _canvasManager.ZoomOut(e.Delta);

            Update();
        }

        /**
         * OnMouseDown method
         * on mouse down event, focus the canvas and check for left or right click
         * \param e Mouse button event
         */
        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e) {
            base.OnMouseDown(e);
            Focus();
        }

        /**
         * OnKeyDown method
         * on key down event, move the canvas and update
         * \param e Key event
         */
        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e) {
            base.OnKeyDown(e);
            _canvasManager.Move(e.Key);
            Update();
        }

        /**
         * SetClipGeometry method
         * Set the clip geometry
         */
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

        public void Update() {
            // clear canvas
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Children.Clear();
            Background = Brushes.Transparent;

            _canvasManager.AdjustForAspectRatio(ActualWidth, ActualHeight);

            // log time
            stopwatch.Stop();
            log.Info("Time to clear canvas: " + stopwatch.ElapsedMilliseconds + " ms");
            stopwatch.Restart();

            List<EngineObject> objects = Engine.Engine.EngineObjectsManager.GetVisibleObjects(_canvasManager);
            stopwatch.Stop();
            log.Info("Time to get visible objects: " + stopwatch.ElapsedMilliseconds + " ms");
            stopwatch.Restart();

            
            // draw grid
            if (drawGrid)
                _gridDrawer.DrawGrid(_canvasManager);
            stopwatch.Stop();
            log.Info("Time to draw grid: " + stopwatch.ElapsedMilliseconds + " ms");
            stopwatch.Restart();

            // get polygons
            foreach (var obj in objects) {
                List<Polygon> polygons = obj.GetPolygons(_canvasManager);
                // convert polygon points to screen coordinates
                foreach (Polygon polygon in polygons) {
                    for (int i = 0; i < polygon.Points.Count; i++) {
                        polygon.Points[i] = ConvertToScreenCoordinates(polygon.Points[i]);
                    }
                    // add polygon to canvas
                    Children.Add(polygon);
                }
            }
            stopwatch.Stop();
            log.Info("Time to draw polygons: " + stopwatch.ElapsedMilliseconds + " ms");
            stopwatch.Restart();
                
            SetClipGeometry();
            log.Info("Time to set clip geometry: " + stopwatch.ElapsedMilliseconds + " ms");
        }

    }
}
