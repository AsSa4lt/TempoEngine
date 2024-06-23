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


        // calculated as e'=(e1*e2)/(e1+e2 - e1*e2)
        // 03 is the default value for the emissivity of the object for aluminium  
        public static double GetEmmisivityBetweenTwoObjects(GrainTriangle obj1, GrainTriangle obj2) {
            return 0.3;
        }

    }
}
