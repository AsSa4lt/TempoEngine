﻿using log4net;
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

        private static readonly ILog log = LogManager.GetLogger(typeof(GrainTriangle));

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
        public override string GetObjectTypeString() {
            return "GrainTriangle";
        }


        public static GrainTriangle FromJson(string json) {
            var settings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            };

            var jObject = JsonConvert.DeserializeObject<dynamic>(json, settings);

            string type = jObject.Type;
            if (type != "GrainTriangle")
                throw new InvalidOperationException("JSON is not of type GrainTriangle.");

            Point pointA = ParsePoint(jObject.PointA.ToString());
            Point pointB = ParsePoint(jObject.PointB.ToString());
            Point pointC = ParsePoint(jObject.PointC.ToString());

            string name = jObject.Name;
            double mass = (double)jObject.Mass;
            double simulationTemperature = (double)jObject.SimulationTemperature;
            double currentTemperature = (double)jObject.CurrentTemperature;
            double thermalConductivity = (double)jObject.ThermalConductivity;

            return new GrainTriangle(name, pointA, pointB, pointC) {
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
                PointA = pointA,
                PointB = pointB,
                PointC = pointC,
                SimulationTemperature = _simulationTemperature,
                CurrentTemperature = _currentTemperature,
                ThermalConductivity = _thermalConductivity
            }, settings);
        }

        public override bool IsIntersecting(EngineObject obj) {
            throw new NotImplementedException();
        }

        public override ObjectType GetObjectType() {
            return ObjectType.GrainTriangle;
        }

        public override double GetLengthTouch(EngineObject obj) {
            // we need to assert that the other object is a grain triangle
            if (obj.GetObjectType() != ObjectType.GrainTriangle) {
                // log error with logger
                log.Debug("GetLengthTouch can only be calculated between two GrainTriangles");
                throw new InvalidOperationException("GetLengthTouch can only be calculated between two GrainTriangles");
            }
            // downcast the object to a grain triangle
            GrainTriangle other = (GrainTriangle)obj;
            // check the object is not null and that the two triangles are not the same
            if (other == null || this.Name == other.Name) {
                return 0;
            }

            // Define the edges of each triangle
            Line[] edgesThis = [
                new Line(this.pointA, this.pointB),
                new Line(this.pointB, this.pointC),
                new Line(this.pointC, this.pointA)
            ];
            Line[] edgesOther = [
                new Line(other.pointA, other.pointB),
                new Line(other.pointB, other.pointC),
                new Line(other.pointC, other.pointA)
            ];

            double totalLength = 0;
            foreach (Line edgeThis in edgesThis) {
                foreach (Line edgeOther in edgesOther) {
                    totalLength += Line.CalculateOverlap(edgeThis, edgeOther);
                }
            }

            // we should divide by 2 since we are double counting the overlap
            return totalLength/2;
        }

        public override List<GrainTriangle> GetTriangles() {
            List<GrainTriangle> grainTriangles = [this];
            return grainTriangles;
        }
    }
}
