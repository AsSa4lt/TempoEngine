using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoEngine.UIControls;

namespace TempoEngine.Engine.Managers{
    /** 
     * \class ObjectsManager
     * \brief Manages the objects in the simulation.
     * 
     * The ObjectsManager class provides methods for managing the objects in the simulation.
     */
    public class ObjectsManager{
        private List<EngineObject> _objects = [];

        public static void PrepareObjects() {

        }

        public  List<EngineObject> GetVisibleObjects(CanvasManager manager) {
            List<EngineObject> visibleObjects = [];
            
            foreach (var obj in _objects) {
                if (obj.IsVisible(manager)) {
                    visibleObjects.Add(obj);
                }
            }
            return visibleObjects;
        }
    }
}
