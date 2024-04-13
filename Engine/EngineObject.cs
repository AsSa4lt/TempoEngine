using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace TempoEngine.Engine {
    abstract class EngineObject {
        private Vector2 _position;
        private Vector2 _rotation;
        private double _temperature;
        private double _thermalConductivity;
        public readonly string Name;
        
        public EngineObject(string name) {
            Name = name;
            _position = new Vector2(0, 0);
            _rotation = new Vector2(0, 0);
            _temperature = 20;
            _thermalConductivity = 0.2;
        }

        abstract public Polygon GetPolygon();
    }
}
