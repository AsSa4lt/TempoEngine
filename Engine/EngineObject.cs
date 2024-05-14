﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using TempoEngine.UIControls;

namespace TempoEngine.Engine {
    public enum ObjectType {
        GrainTriangle
    }

    /**
     * \class EngineObject
     * \brief Abstract base class for all engine objects in the TempoEngine.
     *
     * EngineObject serves as the foundational class for objects in the simulation engine, providing
     * common properties like position, rotation, and temperature, and abstract methods that must be
     * implemented by derived classes to fit specific needs of the engine.
     */
    public abstract class EngineObject : INotifyPropertyChanged {
        /// Event triggered when a property changes.
        public event PropertyChangedEventHandler? PropertyChanged;

        /// Current position of the object in the engine's space.
        protected Vector2 _position = new(0, 0);

        /// Current rotation of the object.
        protected Vector2 _rotation = new(0, 0);

        /// Simulation temperature intended for thermodynamic calculations.
        protected double _simulationTemperature = 200;

        /// Current temperature of the object.
        protected double _currentTemperature = 200;

        /// Thermal conductivity of the object.
        protected double _thermalConductivity = 0.2;

        /// Mass of the object.
        protected double _mass = 1;

        /// Name of the object.
        private string _name;

        /// Selection state of the object.
        private bool _isSelected = false;

        /**
         * Constructor for creating a new EngineObject.
         * \param name The name of the engine object.
         */
        public EngineObject(string name) {
            Name = name;
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
        public Vector2 Position {
            get => _position;
            set {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        /// Gets or sets the rotation of the object.
        public Vector2 Rotation {
            get => _rotation;
            set {
                _rotation = value;
                OnPropertyChanged(nameof(Rotation));
            }
        }

        /// Gets or sets the simulation temperature.
        public double Temperature {
            get => _simulationTemperature;
            set {
                if (_simulationTemperature != value) {
                    _simulationTemperature = value;
                    OnPropertyChanged(nameof(Temperature));
                }
            }
        }

        /// Gets or sets the mass of the object.
        public double Mass {
            get => _mass;
            set {
                if (_mass != value) {
                    _mass = value;
                    OnPropertyChanged(nameof(Temperature)); // Possibly should be OnPropertyChanged(nameof(Mass));
                }
            }
        }

        /// Gets or sets the thermal conductivity.
        public double ThermalConductivity {
            get => _thermalConductivity;
            set {
                if (_thermalConductivity != value) {
                    _thermalConductivity = value;
                    OnPropertyChanged(nameof(ThermalConductivity));
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// Returns polygons representing the object's shape. Must be implemented by subclasses.
        abstract public List<Polygon> GetPolygons();

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
    }
}
