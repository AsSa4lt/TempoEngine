using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TempoEngine.Engine;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;


namespace TempoEngine.UIControls {
    public class EngineObjectsList : System.Windows.Controls.ListView {
        private EngineObject? _currentSelectedEngineObject;
        public Action<EngineObject>? OnSelectedObjectChanged;
        public Action<EngineObject>? OnZoomToObject;
        public Action? OnDeleteObject;
        private bool isEnabled = true;
        public EngineObjectsList() : base() {
            MouseDoubleClick += ZoomToObject;
            SelectionChanged += OnSelectionChanged;
        }
        public void Update(List<EngineObject> objects) {
            Clear();
            foreach (var obj in objects) {
                Items.Add(obj);
            }
        }

        public void Enable(bool IsEnabled) {
           isEnabled = IsEnabled;
        }

        private void ZoomToObject(object sender, MouseButtonEventArgs e) {
            var item = SelectedItem as EngineObject;
            if (item != null) {
                _currentSelectedEngineObject?.Deselect();
                _currentSelectedEngineObject = item;
                _currentSelectedEngineObject.Select();
                OnZoomToObject?.Invoke(_currentSelectedEngineObject);
                Items.Refresh();
            }
        }

        // On delete key press, delete the selected object
        protected override void OnKeyDown(KeyEventArgs e) {
            if (e.Key == Key.Delete && isEnabled) {
                var item = SelectedItem as EngineObject;
                if (item != null) {
                    Engine.Engine.RemoveObject(item);
                    OnDeleteObject?.Invoke();
                }
            } else if (e.Key == Key.Down || e.Key == Key.W) {
                // set as selected the next object if exists
                if (SelectedIndex < Items.Count - 1) {
                    SelectedIndex++;
                }
                _currentSelectedEngineObject = Engine.Engine.GetObjectByIndex(SelectedIndex);
            }else if (e.Key == Key.Up || e.Key == Key.S) {
                // set as selected the previous object if exists
                if (SelectedIndex > 0) {
                    SelectedIndex--;
                }
                _currentSelectedEngineObject = Engine.Engine.GetObjectByIndex(SelectedIndex);
            }
        }


        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            var selectedObject = SelectedItem as EngineObject;
            if (selectedObject != null && (_currentSelectedEngineObject == null || selectedObject.Name != _currentSelectedEngineObject.Name)) { 
                _currentSelectedEngineObject?.Deselect();
                _currentSelectedEngineObject = selectedObject;
                _currentSelectedEngineObject.Select();
                OnSelectedObjectChanged?.Invoke(_currentSelectedEngineObject);
                SelectedIndex = Items.IndexOf(_currentSelectedEngineObject);
            }
        }

        
        public void Clear() {
            Items.Clear();
        }

    }
}
