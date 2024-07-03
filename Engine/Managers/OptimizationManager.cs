using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempoEngine.Engine.Managers {
    public static class OptimizationManager {
        public static void Optimize(List<EngineObject> objects) {
            OptimizeTouching(objects);
        }

        /**
         * Optimize touching objects, by setting adjuscent squares for every square of an object
         * \param objects list of objects
         */
        private static void OptimizeTouching(List<EngineObject> objects) { 
            
        }
    }
}
