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
    public class GrainTriangle : EngineObject {
        Point pointA;
        Point pointB;
        Point pointC;
        public GrainTriangle(string name, Point p_a, Point p_b, Point p_c) : base(name) {
            pointA = p_a;
            pointB = p_b;
            pointC = p_c;
        }

        public override List<Polygon> GetPolygons() {
            List<Polygon> polygons = new List<Polygon>();
            // Create a triangle polygon
            Polygon polygon = new Polygon();
            polygon.Points.Add(pointA);
            polygon.Points.Add(pointB);
            polygon.Points.Add(pointC);

            if(!IsSelected)
                polygon.Fill = EngineManager.GetColorFromTemperature(_simulationTemperature);
            else {
                // If the object is selected i want to add a visible border to the polygon
                polygon.Stroke = System.Windows.Media.Brushes.Black;
                polygon.StrokeThickness = 3;
                // Also change the color of the polygon to a lighter shade
                polygon.Fill = EngineManager.GetColorFromTemperature(_simulationTemperature).Clone();
                polygon.Fill.Opacity = 0.5;
            }

            polygons.Add(polygon);
            return polygons;
        }
        
        private bool isPointVisible(Point point, CanvasManager canvasManager) {
            // implement the point visibility check
            if(point.X >= canvasManager.CurrentLeftXIndex && point.X <= canvasManager.CurrentRightXIndex 
                && point.Y >= canvasManager.CurrentBottomYIndex && point.Y <= canvasManager.CurrentTopYIndex)
                return true;
            return false;
        }

        public override bool IsVisible(CanvasManager canvasManager) {
            // implement the visibility check
            if(isPointVisible(pointA, canvasManager) || isPointVisible(pointB, canvasManager) || isPointVisible(pointC, canvasManager)) {
                return true;
            }
            return false;
        }

        public override void GetObjectVisibleArea(out Vector2 topLeft, out Vector2 bottomRight) {
            // implement the visible area calculation
            topLeft = new Vector2((float)Math.Min(pointA.X, Math.Min(pointB.X, pointC.X)), (float)Math.Min(pointA.Y, Math.Min(pointB.Y, pointC.Y)));
            bottomRight = new Vector2((float)Math.Max(pointA.X, Math.Max(pointB.X, pointC.X)), (float)Math.Max(pointA.Y, Math.Max(pointB.Y, pointC.Y)));
            // extend vectors 4x times bigger than the triangle
            // get distance between left X and right X and multiply by 4
            float xDistance = bottomRight.X - topLeft.X;
            topLeft.X -= xDistance * 2;
            bottomRight.X += xDistance * 2;
            // get distance between top Y and bottom Y and multiply by 4
            float yDistance = bottomRight.Y - topLeft.Y;
            topLeft.Y -= yDistance * 2;
            bottomRight.Y += yDistance * 2;
        }

        public override void SetStartTemperature() {
            _currentTemperature = _simulationTemperature; 
        }

        public override string GetObjectType() {
            return "GrainTriangle";
        }

        public override string GetJsonRepresentation() {
            var settings = new JsonSerializerSettings {
                Formatting = Formatting.Indented, // For better readability of the output JSON
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


    }
}
