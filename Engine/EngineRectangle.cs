﻿using Newtonsoft.Json;
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
                    square.Position = new Point(_position.X + i, _position.Y + j);
                    square.CurrentTemperature = _simulationTemperature;
                    square.SimulationTemperature = _simulationTemperature;
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

            if (propertyName == "Material") {
                foreach (var square in _grainSquares) {
                    square.Material = _material;
                }
            }

            if(propertyName == "Size")      SetSquaresForShape();

            if(propertyName == "SimulationTemperature")   SetTemperatureForAllSquares();

            if (propertyName == "Position") SetSquaresForShape();

            // call base method
            base.OnPropertyChanged(propertyName);
        }

        /**
         * \brief Sets the temperature for all squares.
         */
        private void SetTemperatureForAllSquares() {
            foreach (var square in _grainSquares) {
                square.SimulationTemperature = _simulationTemperature;
                square.CurrentTemperature = _simulationTemperature;
            }
            CurrentTemperature = _simulationTemperature;
            SimulationTemperature = _simulationTemperature;
        }

        /**
         * \brief Gets the object type string.
         * \returns The object type string.
         */
        public override string GetObjectTypeString() {
            return "Rectangle";
        }

        /**
         * \brief Gets the object visible area as the left top and right bottom squares positions
         * \param topLeft The top left corner of the visible area.
         * \param bottomRight The bottom right corner of the visible area.
         */
        public override void GetObjectVisibleArea(out Vector2 topLeft, out Vector2 bottomRight) {
            topLeft = new Vector2((float)_position.X, (float)_position.Y);
            bottomRight = new Vector2((float)(_position.X + _size.X), (float)(_position.Y + _size.Y));
        }

        /**
         * \brief Gets the polygons representing the object's shape.
         * \param canvasManager The canvas manager.
         * \returns The polygons(visible) representing the object's shape.
         */
        public override List<Polygon> GetPolygons(CanvasManager canvasManager) {
            List<Polygon> polygons = new List<Polygon>();
            foreach (GrainSquare sq in _grainSquares) {
                //awful code, but it was made for inheritance and each grain square has only one polygon
                if(sq.IsVisible(canvasManager))
                   polygons.Add(sq.GetPolygons(canvasManager)[0]);
            }

            return polygons;
        }

        public override List<GrainSquare> GetSquares() {
            return _grainSquares;
        }

        public override bool IsIntersecting(EngineObject obj) {
            return false;
        }
        
        /**
         * \brief Determines if the object is visible on the given canvas.
         * \param canvasManager The canvas manager.
         * \returns True if the object is visible on the given canvas, false otherwise.
         */
        public override bool IsVisible(CanvasManager canvasManager) {
            // we need to check coordinates of the canvas manager and check if there is any square in the visible area
            Vector2 topLeft, bottomRight;
            GetObjectVisibleArea(out topLeft, out bottomRight);
            // check for intersecting with canvas manager
            if (topLeft.X > canvasManager.CurrentRightXIndex || bottomRight.X < canvasManager.CurrentLeftXIndex ||
                topLeft.Y > canvasManager.CurrentTopYIndex || bottomRight.Y < canvasManager.CurrentBottomYIndex) {
                return false;
            }
            return true;
        }

        /**
         * Sets the initial temperature of the grain to the simulation temperature.
         */
        public override void SetStartTemperature() {
            _currentTemperature = _simulationTemperature;
            OnPropertyChanged(nameof(CurrentTemperature));
        }
    }

}