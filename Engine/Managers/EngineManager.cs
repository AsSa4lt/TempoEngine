using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;

namespace TempoEngine.Engine.Managers{

    public static class EngineManager {
        public static Brush GetColorFromTemperature(double temperature) {
            if (temperature < 0) throw new ArgumentException("Temperature cannot be less than 0");

            // Define color at minimum temperature (0 degrees) as violet (RGB: 143, 0, 255)
            Color startColor = Color.FromRgb(143, 0, 255);

            // Define color at 200 degrees as red (RGB: 255, 0, 0)
            Color endColor = Color.FromRgb(255, 0, 0);

            // Clamp the temperature to 200 degrees if it's higher
            temperature = Math.Min(temperature, 200);

            // Calculate the interpolation factor (0 for 0 degrees, 1 for 200 degrees)
            double factor = temperature / 200;

            // Interpolate between start and end colors based on temperature
            byte red = (byte)((endColor.R - startColor.R) * factor + startColor.R);
            byte green = (byte)((endColor.G - startColor.G) * factor + startColor.G);
            byte blue = (byte)((endColor.B - startColor.B) * factor + startColor.B);

            // Create and return the brush with the interpolated color
            return new SolidColorBrush(Color.FromRgb(red, green, blue));
        }

        public static void TranferHeatBetweenTwoObjects(EngineObject obj1, EngineObject obj2) {
            List<GrainTriangle> obj1Triangles = obj1.GetTriangles();
            List<GrainTriangle> obj2Triangles = obj2.GetTriangles();
            for(int i = 0; i < obj1Triangles.Count; i++) {
                for(int j = 0; j < obj2Triangles.Count; j++) {
                    double touchLength = obj1Triangles[i].GetLengthTouch(obj2Triangles[j]);
                    double coeficient = MaterialManager.GetCoeficientFromMaterial(obj1Triangles[i], obj2Triangles[j]);
                    double temperatureDifference = obj1Triangles[i].Temperature - obj2Triangles[j].Temperature;
                    double timeTransfer = Engine.EngineIntervalUpdate;
                    double heatTransfer = coeficient * touchLength * temperatureDifference * timeTransfer;
                    obj1Triangles[i].Temperature -= heatTransfer / obj1Triangles[i].SpecificHeatCapacity;
                    obj2Triangles[j].Temperature += heatTransfer / obj2Triangles[j].SpecificHeatCapacity;
                }
            }
        }

    }
}
