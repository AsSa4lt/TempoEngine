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

        // Engine.md (1)
        // Temperature is stored in Kelvin, based on wawe spectrum of light
        // Violet - 0 degrees, Red is everything 200 degrees and above

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

    }
}
