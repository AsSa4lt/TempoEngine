using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempoEngine.Engine.Managers {
    internal class MaterialManager {

        // now we assume that material is constant and it's aluminum
        public static double GetCoeficientFromMaterial(GrainTriangle obj1, GrainTriangle obj2) {
            return 237;
        }

        public static double GetSpecificHeatCapacity(GrainTriangle obj) {
            return 700;
        }

        public static double GetDensity(GrainTriangle obj) {
            return 2700;
        }

        public static double GetEmmisivity(GrainTriangle obj) {
            return 0.08;
        }
    }
}
