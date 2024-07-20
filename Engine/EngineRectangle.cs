using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using TempoEngine.UIControls;

namespace TempoEngine.Engine {
    /**
     * \class EngineRectangle
     * \brief Object of the engine that represents rectangle
     * 
     * 
     * Manages itself and makes it easier to calculate transfers
     * \see EngineObject
     * \see CanvasManager
     */
    public class EngineRectangle : EngineObject {
        private List<GrainSquare> _grainSquares;
        /**
         * \brief Initializes a new instance of the EngineRectangle class.
         * Create list of squares that are part of the rectangle and set the temperature of every square to the same value.
         * \param name The name of the engine object.
         * \param width The width of the rectangle.
         * \param height The height of the rectangle.
         */
        public EngineRectangle(string name, int width, int height) : base(name) {
            _size = new(width, height);
            _grainSquares = new List<GrainSquare>();
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    GrainSquare square = new($"{name} square {i} {j}", new System.Windows.Point(i, j));
                    _grainSquares.Add(square);
                }
            }
            SetTemperatureForAllSquares();
        }

        public override List<GrainSquare> GetExternalSquares() {
            throw new NotImplementedException();
        }

        public override string GetJsonRepresentation() {
            throw new NotImplementedException();
        }


        /**
         * \brief Gets the type of the object.
         * \returns The type of the object.
         */ 
        public override ObjectType GetObjectType() {
            return ObjectType.Rectangle;
        }

        /**
         * \brief OnPropertyChanged
         * Based on which property has been changed, set the parameters for the squares
         */
        protected override void OnPropertyChanged(string propertyName) {
            // set the same temperature for all squares
            foreach (var square in _grainSquares) {
                square.SimulationTemperature = _simulationTemperature;
            }

            if(propertyName == "Material") {
                foreach (var square in _grainSquares) {
                    square.Material = _material;
                }
            }

            // call base method
            base.OnPropertyChanged(propertyName);
        }

        /**
         * \brief Sets the temperature for all squares.
         */
        private void SetTemperatureForAllSquares() {
            foreach (var square in _grainSquares) {
                square.SimulationTemperature = _simulationTemperature;
            }
        }

        /**
         * \brief Gets the object type string.
         * \returns The object type string.
         */
        public override string GetObjectTypeString() {
            return "Rectangle";
        }

        public override void GetObjectVisibleArea(out Vector2 topLeft, out Vector2 bottomRight) {
            throw new NotImplementedException();
        }

        public override List<Polygon> GetPolygons() {
            throw new NotImplementedException();
        }

        public override List<GrainSquare> GetSquares() {
            throw new NotImplementedException();
        }

        public override bool IsIntersecting(EngineObject obj) {
            throw new NotImplementedException();
        }

        public override bool IsVisible(CanvasManager canvasManager) {
            throw new NotImplementedException();
        }

        public override void SetStartTemperature() {
            throw new NotImplementedException();
        }
    }

}
