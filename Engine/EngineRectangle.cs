using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Xml.Linq;
using TempoEngine.Engine.Managers;
using TempoEngine.UIControls;
using Point = System.Windows.Point;

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
        private List<GrainSquare> _externalSquares;
        /**
         * \brief Initializes a new instance of the EngineRectangle class.
         * Create list of squares that are part of the rectangle and set the temperature of every square to the same value.
         * Create list of external squares
         * \param name The name of the engine object.
         * \param width The width of the rectangle.
         * \param height The height of the rectangle.
         */
        public EngineRectangle(string name, int width, int height) : base(name) {
            _size = new(width, height);
            SetSquaresForShape();
            SetTemperatureForAllSquares();
        }

        /**
         * \brief Create squares for the shape
         */
        private void SetSquaresForShape() {
            _externalSquares = [];
            _grainSquares = [];
            for (int i = 0; i < Size.X; i++) {
                for (int j = 0; j < Size.Y; j++) {
                    GrainSquare square = new($"{Name} square {i} {j}", new System.Windows.Point(i, j));
                    _grainSquares.Add(square);
                    if (i == 0 || j == 0 || i == Size.X - 1 || j == Size.Y - 1) {
                        _externalSquares.Add(square);
                    }
                }
            }
        }

        /**
         * \brief Gets the external squares.
         * \returns The external squares.
         */
        public override List<GrainSquare> GetExternalSquares() {
          return _externalSquares;
        }


        /**
         * \brief Creates and object from JSON representation.
         * \param json The JSON representation of the object.
         * \returns The object created from JSON representation.
         */
        public static EngineRectangle FromJson(string json) {
            var settings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            };

            var jObject = JsonConvert.DeserializeObject<dynamic>(json, settings);

            string type = jObject.Type;

            if(type != "Rectangle") {
                throw new ArgumentException("JSON is not of type Rectangle");
            }
            string name = jObject.Name;
            double simulationTemperature = (double)jObject.SimulationTemperature;
            Point position = Util.Parsers.ParsePoint(jObject.Position.ToString());
            Point Position = Util.Parsers.ParsePoint(jObject.Position.ToString());
            Point Size = Util.Parsers.ParsePoint(jObject.Size.ToString());
            Material Material = MaterialManager.GetMaterialByName((string)jObject.MaterialName);

            return new EngineRectangle(name, (int)(Position.X + Size.X), (int)(Position.Y + Size.Y)) {
                _simulationTemperature = simulationTemperature,
                _position = Position,
                _size = Size,
            };
        }

        /**
         * \brief Gets the JSON representation of the object.
         * \returns The JSON representation of the object.
         */
        public override string GetJsonRepresentation() {
            var settings = new JsonSerializerSettings {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(new {
                Type = GetObjectTypeString(),
                Name,
                Position = _position,
                Size = _size,
                SimulationTemperature = _simulationTemperature,
                Material = _material.Name
            }, settings);
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

            if (propertyName == "Material") {
                foreach (var square in _grainSquares) {
                    square.Material = _material;
                }
            }

            if(propertyName == "Size") {
                SetSquaresForShape();
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
            return _grainSquares;
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
