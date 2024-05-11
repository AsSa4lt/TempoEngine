using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public class GrainTriangle : EngineObject {
        private Point pointA;
        private Point pointB;
        private Point pointC;

        /**
         * Constructs a GrainTriangle with specified vertices and name.
         * \param name The name of the grain triangle.
         * \param p_a Vertex A of the triangle.
         * \param p_b Vertex B of the triangle.
         * \param p_c Vertex C of the triangle.
         */
        public GrainTriangle(string name, Point p_a, Point p_b, Point p_c) : base(name) {
            pointA = p_a;
            pointB = p_b;
            pointC = p_c;
        }

        /**
         * Gets or sets the position of vertex A.
         * Triggers a property changed event when set.
         * \see OnPropertyChanged
         */
        public Point PointA {
            get => pointA;
            set {
                pointA = value;
                OnPropertyChanged(nameof(PointA));
            }
        }

        /**
         * Gets or sets the position of vertex B.
         * Triggers a property changed event when set.
         */
        public Point PointB {
            get => pointB;
            set {
                pointB = value;
                OnPropertyChanged(nameof(PointB));
            }
        }

        /**
         * Gets or sets the position of vertex C.
         * Triggers a property changed event when set.
         */
        public Point PointC {
            get => pointC;
            set {
                pointC = value;
                OnPropertyChanged(nameof(PointC));
            }
        }

        /**
         * Generates the polygons that visually represent the triangle.
         * This method overrides the abstract method defined in \ref EngineObject.
         * \return List of polygons constituting the triangle's visual representation.
         */
        public override List<Polygon> GetPolygons() {
            List<Polygon> polygons = new List<Polygon>();
            Polygon polygon = new Polygon();
            polygon.Points.Add(pointA);
            polygon.Points.Add(pointB);
            polygon.Points.Add(pointC);

            if (!IsSelected)
                polygon.Fill = EngineManager.GetColorFromTemperature(_simulationTemperature);
            else {
                polygon.Stroke = System.Windows.Media.Brushes.Black;
                polygon.StrokeThickness = 3;
                polygon.Fill = EngineManager.GetColorFromTemperature(_simulationTemperature).Clone();
                polygon.Fill.Opacity = 0.5;
            }

            polygons.Add(polygon);
            return polygons;
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
            return isPointVisible(pointA, canvasManager) || isPointVisible(pointB, canvasManager) || isPointVisible(pointC, canvasManager);
        }

        /**
         * Calculates the bounding box that encompasses the triangle.
         * \param[out] topLeft The top-left corner of the bounding box.
         * \param[out] bottomRight The bottom-right corner of the bounding box.
         */
        public override void GetObjectVisibleArea(out Vector2 topLeft, out Vector2 bottomRight) {
            topLeft = new Vector2((float)Math.Min(pointA.X, Math.Min(pointB.X, pointC.X)), (float)Math.Min(pointA.Y, Math.Min(pointB.Y, pointC.Y)));
            bottomRight = new Vector2((float)Math.Max(pointA.X, Math.Max(pointB.X, pointC.X)), (float)Math.Max(pointA.Y, Math.Max(pointB.Y, pointC.Y)));

            float xDistance = bottomRight.X - topLeft.X;
            topLeft.X -= xDistance * 2;
            bottomRight.X += xDistance * 2;

            float yDistance = bottomRight.Y - topLeft.Y;
            topLeft.Y -= yDistance * 2;
            bottomRight.Y += yDistance * 2;
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
        public override string GetObjectType() {
            return "GrainTriangle";
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
                Type = GetObjectType(),
                Name,
                Mass = _mass,
                PointA = pointA,
                PointB = pointB,
                PointC = pointC,
                SimulationTemperature = _simulationTemperature,
                CurrentTemperature = _currentTemperature,
                ThermalConductivity = _thermalConductivity
            }, settings);
        }
    }
}
