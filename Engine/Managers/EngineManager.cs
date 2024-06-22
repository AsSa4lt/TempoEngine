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
                    double temperatureDifference = obj1Triangles[i].CurrentTemperature - obj2Triangles[j].CurrentTemperature;
                    double timeTransfer = Engine.EngineIntervalUpdate;
                    double heatTransfer = coeficient * touchLength * temperatureDifference * timeTransfer;
                    obj1Triangles[i].CurrentTemperature -= heatTransfer / obj1Triangles[i].SpecificHeatCapacity;
                    obj1Triangles[i].CurrentTemperature = Math.Max(0, obj1Triangles[i].CurrentTemperature);
                    obj2Triangles[j].CurrentTemperature += heatTransfer / obj2Triangles[j].SpecificHeatCapacity;
                    obj2Triangles[j].CurrentTemperature = Math.Max(0, obj2Triangles[j].CurrentTemperature);
                }
            }
        }

        public static readonly double StefanBoltzmannConst = 5.67 * Math.Pow(10, -8);
        public static void TranferRadation(EngineObject obj1, List<EngineObject> objects) {
            List<GrainTriangle> objTriangles = obj1.GetTriangles();

            for(int i = 0; i < objTriangles.Count; i++) {
                GrainTriangle triangle = objTriangles[i];
                // calculated by Stefan-Boltzmann law of radiation and multiplied by the engine update interval
                double energyRadiationLoss = StefanBoltzmannConst * Math.E * triangle.GetPerimeter() * Math.Pow(triangle.CurrentTemperature, 4) * Engine.EngineIntervalUpdate;
                for(int j = 0; j < objects.Count; j++) {
                    List<GrainTriangle> otherTriangles = objects[j].GetTriangles();
                    for(int k = 0; k < otherTriangles.Count; k++) {
                        // calculate by radiation absobtion formula
                        double energyRadiationAbsobtion = StefanBoltzmannConst * Math.E * (Math.Pow(otherTriangles[k].CurrentTemperature, 4) - Math.Pow(triangle.CurrentTemperature, 4)) * Engine.EngineIntervalUpdate;
                        otherTriangles[k].CurrentTemperature += energyRadiationAbsobtion / otherTriangles[k].SpecificHeatCapacity;
                        otherTriangles[k].CurrentTemperature = Math.Max(0, otherTriangles[k].CurrentTemperature);
                    }
                }
                triangle.CurrentTemperature -= energyRadiationLoss / triangle.SpecificHeatCapacity;
                triangle.CurrentTemperature = Math.Max(0, triangle.CurrentTemperature);
            }
        }

    }
}
