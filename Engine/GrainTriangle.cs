using System;
using System.Collections.Generic;
using System.Linq;
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

        public override Polygon GetPolygon() {
            // Create a triangle polygon
            Polygon polygon = new Polygon();
            polygon.Points.Add(pointA);
            polygon.Points.Add(pointB);
            polygon.Points.Add(pointC);

            polygon.Fill = EngineManager.GetColorFromTemperature(_temperature);

            return polygon;
        }
        
        private bool isPointVisible(Point point, CanvasManager canvasManager) {
            // implement the point visibility check
            if(point.X >= canvasManager.CurrentLeftXIndex && point.X <= canvasManager.CurrentRightXIndex 
                && point.Y >= canvasManager.CurrentBottomYIndex && point.Y <= canvasManager.CurrentTopYIndex)
                return true;
            return false;
        }

        public override bool isVisible(CanvasManager canvasManager) {
            // implement the visibility check
            if(isPointVisible(pointA, canvasManager) || isPointVisible(pointB, canvasManager) || isPointVisible(pointC, canvasManager)) {
                return true;
            }
            return false;
        }
    }
}
