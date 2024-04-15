using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
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
            return polygon;
        }
    }
}
