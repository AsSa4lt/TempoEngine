﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using TempoEngine.Engine.Managers;
using TempoEngine.UIControls;
using Point = System.Windows.Point;

namespace TempoEngine.Engine {
    public enum ObjectType {
        GrainSquare
    }

    /**
     * \class EngineObject
     * \brief Abstract base class for all engine objects in the TempoEngine.
     *
     * EngineObject serves as the foundational class for objects in the simulation engine, providing
     * common properties like position, and temperature, and abstract methods that must be
     * implemented by derived classes to fit specific needs of the engine.
     */
    public abstract class EngineObject : INotifyPropertyChanged {
        /// Event triggered when a property changes.
        public event PropertyChangedEventHandler? PropertyChanged;

        /// Current position of the object in the engine's space.
        protected Point _position = new(0, 0);

        /// Simulation temperature intended for thermodynamic calculations.
        protected double _simulationTemperature = 200;

        /// Current temperature of the object.
        protected double _currentTemperature = 200;

        /// Mass of the object.
        protected double _mass = 1;

        /// Name of the object.
        private string _name;

        /// It's time consuming to calculate the area of an object, so we cache it.
        protected double _cachedSideArea = -1; // -1 means not yet calculated

        /// Cached volume of the object.
        protected double _cachedVolume = -1; // -1 means not yet calculated

        // Cached mass of the object.
        protected double _cachedMass = -1; // -1 means not yet calculated

        /// Selection state of the object.
        private bool _isSelected = false;

        /// Material of the object.
        protected Material _material;

        /**
         * Constructor for creating a new EngineObject.
         * \param name The name of the engine object.
         */
        public EngineObject(string name) {
            Name = name;
            Material = MaterialManager.GetBaseMaterial();
        }

        /**
         * Marks the object as selected.
         */
        public void Select() {
            IsSelected = true;
        }

        /**
         * Marks the object as deselected.
         */
        public void Deselect() {
            IsSelected = false;
        }

        /**
         * Returns the name of the object as its string representation.
         * \return The name of the object.
         */
        public override string ToString() {
            return Name;
        }

        /// Gets or sets the position of the object.
        public virtual Point Position {
            get => _position;
            set {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        /// Gets or sets the material of the object.
        public Material Material {
            get => _material;
            set {
                _material = value;
                OnPropertyChanged(nameof(Material));
            }
        }

        /// Gets or sets the current temperature.
        public double CurrentTemperature {
            get => _currentTemperature;
            set {
                if (_currentTemperature != value) {
                    _currentTemperature = value;
                    OnPropertyChanged(nameof(CurrentTemperature));
                }
            }
        }

        /// Gets or sets the simulation temperature.
        public double SimulationTemperature {
            get => _simulationTemperature;
            set {
                if (_simulationTemperature != value) {
                    _simulationTemperature = value;
                    _currentTemperature = value;
                    OnPropertyChanged(nameof(SimulationTemperature));
                }
            }
        }

        /// Gets or sets the mass of the object.
        public double Mass {
            get => _mass;
            set {
                if (_mass != value) {
                    _mass = value;
                    OnPropertyChanged(nameof(Mass)); // Possibly should be OnPropertyChanged(nameof(Mass));
                }
            }
        }

        /// Gets or sets the name of the object.
        public string Name {
            get => _name;
            set {
                if (_name != value) {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        /// Gets or sets whether the object is selected.
        public bool IsSelected {
            get => _isSelected;
            set {
                if (_isSelected != value) {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        /// Called to notify observers of property changes.
        protected void OnPropertyChanged(string propertyName) {
            if (Engine.Mode != Engine.EngineMode.Running) {
                _cachedSideArea = -1;
                _cachedVolume = -1;
                _cachedMass = -1;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// Returns polygons representing the object's shape. Must be implemented by subclasses.
        abstract public List<Polygon> GetPolygons();

        /// Returns the object's squares. Must be implemented by subclasses.
        abstract public List<GrainSquare> GetSquares();

        /// Determines if the object is visible on the given canvas. Must be implemented by subclasses.
        abstract public bool IsVisible(CanvasManager canvasManager);

        /// Gets the visible area of the object. Must be implemented by subclasses.
        abstract public void GetObjectVisibleArea(out Vector2 topLeft, out Vector2 bottomRight);

        /// Sets the starting temperature for the simulation. Must be implemented by subclasses.
        abstract public void SetStartTemperature();

        /// Gets the type of the object as a string. Must be implemented by subclasses.
        abstract public string GetObjectTypeString();

        /// Gets the type of the object as an ObjectType enum. Must be implemented by subclasses.
        abstract public ObjectType GetObjectType();
        /// Gets a JSON string representing the object's state. Must be implemented by subclasses.
        abstract public string GetJsonRepresentation();

        /// Determines if the object is intersecting with another object. Must be implemented by subclasses.
        abstract public bool IsIntersecting(EngineObject obj);

        /// Cache all the object's properties. Must be implemented by subclasses.
        abstract public void CacheProperties();

        /// Gets all external GrainSquare’s of an object, that can tranfer heat with other external GrainSquare’s of other objects
        abstract public List<GrainSquare> GetExternalSquares();
    }
}
