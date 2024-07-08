using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempoEngine.Engine {
    public class Material {


        public string Name { get; set; }

        // if this material can be deleted
        public bool isBaseMaterial { get; set; }

        public double SpecificHeatCapacity { get; set; }

        public double Density { get; set; }

        public double Emmisivity { get; set; }
    }
}
