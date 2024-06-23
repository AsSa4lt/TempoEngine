using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = System.Windows.Point;

namespace TempoEngine.Engine.Examples {
    public static class SimpleExamples {
        public static void SetThreeTriangles() {
            // add 3 objects to the engine
            GrainTriangle obj1 = new GrainTriangle("Triangle1", new Point(0, 0), new Point(0, 1), new Point(1, 0));
            obj1.SimulationTemperature = 200;
            Engine.AddObject(obj1);
            GrainTriangle obj2 = new GrainTriangle("Triangle2", new Point(1, 0), new Point(0, 1), new Point(1, 1));
            obj2.SimulationTemperature = 50;
            Engine.AddObject(obj2);
            GrainTriangle obj3 = new GrainTriangle("Triangle3", new Point(2, 2), new Point(4, 4), new Point(2, 0));
            obj1.SimulationTemperature = 0;
            Engine.AddObject(obj3);
        }

        public static void RectangleWithTempDifference(int width, int height) {
            double maxTemperature = 1000.0;

            // Calculate the center position
            double centerX = width / 2.0;
            double centerY = height / 2.0;

            // Create each triangle in the grid
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    // Calculate the centroid of the triangle
                    Point centroid = new Point(x + 0.5, y + 0.5);

                    // Calculate distance from the center
                    double distance = Math.Sqrt(Math.Pow(centroid.X - centerX, 2) + Math.Pow(centroid.Y - centerY, 2));
                    double maxDistance = Math.Sqrt(Math.Pow(centerX, 2) + Math.Pow(centerY, 2));

                    // Assign temperature inversely proportional to the distance
                    double temperature = maxTemperature * (1 - (distance / maxDistance));

                    // Define points for the triangle (assuming each cell is a right triangle)
                    Point p1 = new Point(x, y);
                    Point p2 = new Point(x, y + 1);
                    Point p3 = new Point(x + 1, y);
                    Point p4 = new Point(x + 1, y + 1);
                    // Create triangle
                    GrainTriangle triangle = new GrainTriangle($"Triangle_{x}_{y}", p1, p2, p3);
                    triangle.SimulationTemperature = temperature;
                    GrainTriangle simetricalTriangle = new GrainTriangle($"Triangle_{x}_{y}S", p2, p3, p4);
                    simetricalTriangle.SimulationTemperature = temperature;


                    // Add to the engine
                    Engine.AddObject(triangle);
                    Engine.AddObject(simetricalTriangle);
                }
            }
        }
    }
}
