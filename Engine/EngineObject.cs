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
    public abstract class EngineObject : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;


        protected Vector2 _position = new(0, 0);
        protected Vector2 _rotation = new(0, 0);
        protected double _temperature = 200;
        protected double _thermalConductivity = 0.2;
        private string _name;
        private bool _isSelected = false;

        public EngineObject(string name) {
            Name = name;
        }

        public void Select() {
            IsSelected = true;
        }

        public void Deselect() {
            IsSelected = false;
        }


        // tostring returns the name of the object
        public override string ToString() {
            return Name;
        }

        public Vector2 Position {
            get => _position;
            set {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        public Vector2 Rotation {
            get => _rotation;
            set {
                _rotation = value;
                OnPropertyChanged(nameof(Rotation));
            }
        }

        public double Temperature {
            get => _temperature;
            set {
                if (_temperature != value) {
                    _temperature = value;
                    OnPropertyChanged(nameof(Temperature));
                }
            }
        }

        public double ThermalConductivity {
            get => _thermalConductivity;
            set {
                if (_thermalConductivity != value) {
                    _thermalConductivity = value;
                    OnPropertyChanged(nameof(ThermalConductivity));
                }
            }
        }

        public string Name {
            get => _name;
            set {
                if (_name != value) {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public bool IsSelected {
            get => _isSelected;
            set {
                if (_isSelected != value) {
                    _isSelected = value;
                    //OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        abstract public List<Polygon> GetPolygons();
        abstract public bool IsVisible(CanvasManager canvasManager);

        abstract public void GetObjectVisibleArea(out Vector2 topLeft, out Vector2 bottomRight);
    }
}
