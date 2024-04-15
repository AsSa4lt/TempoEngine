using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using TempoEngine.UIControls;

namespace TempoEngine.Engine {
    public abstract class EngineObject(string name) {
        protected Vector2 _position = new Vector2(0, 0);
        protected Vector2 _rotation = new(0, 0);
        protected double _temperature = 200;
        protected double _thermalConductivity = 0.2;
        public readonly string Name = name;

        abstract public Polygon GetPolygon();
        abstract public bool isVisible(CanvasManager canvasManager);
        
        public void SetTemperature(double temperature) {
            _temperature = temperature;
        }
    }
}
