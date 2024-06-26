using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using TempoEngine.Engine.Managers;
using TempoEngine.UIControls;
using Point = System.Windows.Point;

namespace TempoEngine.Engine{
    /**
     * \class GrainTriangle
     * \brief Represents a triangular grain object within the simulation engine.
     *
     * The GrainTriangle class extends \ref EngineObject and encapsulates the properties
     * and behavior of a triangle-shaped grain in the simulation, including thermal properties,
     * position, and selection state. It includes methods for rendering, visibility checks, and serialization.
     * 
     * \see EngineObject
     * \see CanvasManager
     */
    public class GrainSquare : EngineObject {
        private double _energyDelta = 0;
        public event PropertyChangedEventHandler? PositionChanged;
        private List<GrainSquare> _adjacentSquares = [];

        private static readonly ILog log = LogManager.GetLogger(typeof(GrainSquare));

        /**
         * Constructs a GrainTriangle with specified vertices and name.
         * \param name The name of the grain triangle.
         * \param p_a Vertex A of the triangle.
         * \param p_b Vertex B of the triangle.
         * \param p_c Vertex C of the triangle.
         */
        public GrainSquare(string name, Point position) : base(name) {
            _position = position;
            SetCachedPoints();
        }


        // Cached points of square to not to allocate anything during the runtime
        Point _cachedPointB = new(0, 0); // right top corner
        Point _cachedPointC = new(0, 0); // left bottom corner
        Point _cachedPointD = new(0, 0); // right bottom corner
        /**
         * Generates the polygons that visually represent the triangle.
         * This method overrides the abstract method defined in \ref EngineObject.
         * \return List of polygons constituting the triangle's visual representation.
         */
        public override List<Polygon> GetPolygons() {
            List<Polygon> polygons = new List<Polygon>();
            Polygon polygon = new Polygon();
            polygon.Points.Add(_position);
            polygon.Points.Add(_cachedPointB);
            polygon.Points.Add(_cachedPointD);
            polygon.Points.Add(_cachedPointC);

            if (!IsSelected)
                polygon.Fill = ColorManager.GetColorFromTemperature(_currentTemperature);
            else {
                polygon.Stroke = System.Windows.Media.Brushes.Black;
                polygon.StrokeThickness = 3;
                polygon.Fill = ColorManager.GetColorFromTemperature(_currentTemperature).Clone();
                if(Engine.Mode != Engine.EngineMode.Running)
                    polygon.Fill.Opacity = 0.5;
            }

            polygons.Add(polygon);
            return polygons;
        }

        private void SetCachedPoints() {
            _cachedPointB = new(Position.X + 1, Position.Y);
            _cachedPointC = new(Position.X, Position.Y - 1);
            _cachedPointD = new(Position.X + 1, Position.Y - 1);
        }

        public override Point Position {
            get => _position;
            set {
                _position = value;
                SetCachedPoints();
                OnPositionChanged(nameof(Position));
                OnPropertyChanged(nameof(Position));
            }
        }

        /**
         * Checks if a given point is visible within the current canvas manager's view.
         * \param point The point to check for visibility.
         * \param canvasManager The canvas manager providing the current view context.
         * \return True if the point is visible, otherwise false.
         */
        private bool isPointVisible(Point point, CanvasManager canvasManager) {
            return point.X >= canvasManager.CurrentLeftXIndex && point.X <= canvasManager.CurrentRightXIndex &&
                   point.Y >= canvasManager.CurrentBottomYIndex && point.Y <= canvasManager.CurrentTopYIndex;
        }

        /**
         * Determines whether any of the triangle's vertices are visible in the current view.
         * \param canvasManager The canvas manager providing the current view context.
         * \return True if any vertex is visible, otherwise false.
         */
        public override bool IsVisible(CanvasManager canvasManager) {
            return isPointVisible(Position, canvasManager);
        }

        /**
         * Calculates the bounding box that encompasses the triangle.
         * \param[out] topLeft The top-left corner of the bounding box.
         * \param[out] bottomRight The bottom-right corner of the bounding box.
         */
        public override void GetObjectVisibleArea(out Vector2 topLeft, out Vector2 bottomRight) {
            topLeft = new Vector2((float)_position.X, (float)_position.Y);
            bottomRight = new Vector2((float)_cachedPointD.X, (float)_cachedPointD.Y);
        }


        public void AddEnergyDelta(double energyDelta) {
            _energyDelta += energyDelta;
        }

        public void ApplyEnergyDelta() {
            CurrentTemperature = _currentTemperature + _energyDelta / MaterialManager.GetSpecificHeatCapacity(this) / GetMass();
            CurrentTemperature = Math.Max(0, CurrentTemperature);
            _energyDelta = 0;
        }

        /**
         * Sets the initial temperature of the grain to the simulation temperature.
         */
        public override void SetStartTemperature() {
            _currentTemperature = _simulationTemperature;
        }

        /**
         * Provides the type identifier for GrainTriangle objects.
         * \return A string identifier for the type.
         */
        public override string GetObjectTypeString() {
            return "GrainSquare";
        }


        public static GrainSquare FromJson(string json) {
            var settings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            };

            var jObject = JsonConvert.DeserializeObject<dynamic>(json, settings);

            string type = jObject.Type;
            if (type != "GrainTriangle")
                throw new InvalidOperationException("JSON is not of type GrainTriangle.");
            Point Position = ParsePoint(jObject.Position.ToString());

            string name = jObject.Name;
            double mass = (double)jObject.Mass;
            double simulationTemperature = (double)jObject.SimulationTemperature;
            double currentTemperature = (double)jObject.CurrentTemperature;
            double thermalConductivity = (double)jObject.ThermalConductivity;

            return new GrainSquare(name, Position) {
                _simulationTemperature = simulationTemperature,
                _currentTemperature = currentTemperature,
                _thermalConductivity = thermalConductivity,
                _mass = mass
            };
        }

        private static Point ParsePoint(string point) {
            var parts = point.Split(',');
            return new Point(double.Parse(parts[0]), double.Parse(parts[1]));
        }
        /**
         * Serializes the grain triangle to a JSON representation.
         * \return A JSON string representing the grain triangle.
         */
        public override string GetJsonRepresentation() {
            var settings = new JsonSerializerSettings {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(new {
                Type = GetObjectTypeString(),
                Name,
                Mass = _mass,
                Position = _position,
                SimulationTemperature = _simulationTemperature,
                CurrentTemperature = _currentTemperature,
                ThermalConductivity = _thermalConductivity
            }, settings);
        }

        public double GetPerimeter() {
            return 0.004;
        }

        /// IT'S NOT AN AREA OF A TRIANGLE
        public double GetNormalizedSideArea() {
            if(_cachedSideArea != -1)
                return _cachedSideArea;
            double normalizedPerimeter = GetPerimeter() / Math.Pow(1000, 2);
            double normalizedSideArea = normalizedPerimeter * Width;
            _cachedSideArea = normalizedSideArea;
            return _cachedSideArea;
        }

        private double GetNormalizedArea() {
            return Width * Width ;
        }

 
        public double GetNormalizedVolume() {
            if(_cachedVolume != -1)
                return _cachedVolume;
            double normalizedArea = GetNormalizedArea() * Width;
            double normalizedVolume = normalizedArea * Width;
            _cachedVolume = normalizedVolume;
            return _cachedVolume;
        }

        public double GetMass() {
            if(_cachedMass != -1)
                return _cachedMass;
            double mass = GetNormalizedVolume() * MaterialManager.GetDensity(this);
            _cachedMass = mass;
            return _cachedMass;
        }


        public override bool IsIntersecting(EngineObject obj) {
            throw new NotImplementedException();
        }

        public override ObjectType GetObjectType() {
            return ObjectType.GrainSquare;
        }

        protected void OnPositionChanged(string propertyName) {
            if (Engine.Mode != Engine.EngineMode.Running) {
                SetCachedPoints();
                PositionChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool AreTouching(GrainSquare other) {
            // check the object is not null and that the two triangles are not the same
            if (other == null || this.Name == other.Name) {
                return false;
            }
            bool xTouch = Math.Abs(this.Position.X - other.Position.X) == 1 && this.Position.Y == other.Position.Y;
            bool yTouch = Math.Abs(this.Position.Y - other.Position.Y) == 1 && this.Position.X == other.Position.X;

            return xTouch || yTouch;
        }

        public override List<GrainSquare> GetSquares() {
            List<GrainSquare> grainTriangles = [this];
            return grainTriangles;
        }

        public List<GrainSquare> GetAdjacentSquares() {
            return _adjacentSquares;
        }

        public void AddAdjacentSquare(GrainSquare square) {
            _adjacentSquares.Add(square);
        }

        public void ClearAdjacentSquares() {
            _adjacentSquares.Clear();
        }

        public override List<GrainSquare> GetExternalSquares() {
            return new List<GrainSquare> {this};
        }
    }
}
