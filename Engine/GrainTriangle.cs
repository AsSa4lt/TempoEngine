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

        public override Polygon GetPolygon() {
            // Create a triangle polygon
            Polygon polygon = new Polygon();
            polygon.Points.Add(pointA);
            polygon.Points.Add(pointB);
            polygon.Points.Add(pointC);

            if(!IsSelected)
                polygon.Fill = EngineManager.GetColorFromTemperature(_temperature);
            else {
                // If the object is selected i want to add a visible border to the polygon
                polygon.Stroke = System.Windows.Media.Brushes.Black;
                polygon.StrokeThickness = 3;
                // Also change the color of the polygon to a lighter shade
                polygon.Fill = EngineManager.GetColorFromTemperature(_temperature).Clone();
                polygon.Fill.Opacity = 0.5;
            }

            return polygon;
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
    }
}
