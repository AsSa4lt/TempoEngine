using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace TempoEngine.Engine {
    public abstract class EngineObject(string name) {
        private Vector2 _position = new Vector2(0, 0);
        private Vector2 _rotation = new(0, 0);
        private double _temperature = 20;
        private double _thermalConductivity = 0.2;
        public readonly string Name = name;

        abstract public Polygon GetPolygon();
    }
}
