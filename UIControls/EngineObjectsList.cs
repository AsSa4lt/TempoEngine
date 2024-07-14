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
    /**
     * \class EngineObjects
     * \brief EngineObjectsList class
     * List of all objects of the simulation
     */
    public class EngineObjectsList : System.Windows.Controls.ListView {
        private EngineObject? _currentSelectedEngineObject;  // Current selected object
        public Action<EngineObject>? OnSelectedObjectChanged;// Event for selected object changed
        public Action<EngineObject>? OnZoomToObject;         // Event for zoom to object
        public Action? OnDeleteObject;                       // Event for delete object
        private bool isEnabled = true;                       // Is enabled

        /**
         * Constructor
         */
        public EngineObjectsList() : base() {
            MouseDoubleClick += ZoomToObject;
            SelectionChanged += OnSelectionChanged;
        }

        /**
         * Update the list of objects
         * \param objects List of objects
         */
        public void Update(List<EngineObject> objects) {
            Clear();
            foreach (var obj in objects) {
                Items.Add(obj);
            }
        }

        public void Enable(bool IsEnabled) {
           isEnabled = IsEnabled;
        }

        /**
         * Zoom to object
         * \param sender Sender
         * \param e Mouse button event
         */
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

       
        /**
         * \brief OnKeyDown method for Delete key
         * On delete key press, delete the selected object
         */
        protected override void OnKeyDown(KeyEventArgs e) {
            if (e.Key == Key.Delete && isEnabled) {
                var item = SelectedItem as EngineObject;
                if (item != null) {
                    Engine.Engine.EngineObjectsManager.RemoveObject(item);
                    OnDeleteObject?.Invoke();
                }
            } else if (e.Key == Key.Down || e.Key == Key.W) {
                // set as selected the next object if exists
                if (SelectedIndex < Items.Count - 1) {
                    SelectedIndex++;
                }
                _currentSelectedEngineObject = Engine.Engine.EngineObjectsManager.GetObjectByIndex(SelectedIndex);
            }else if (e.Key == Key.Up || e.Key == Key.S) {
                // set as selected the previous object if exists
                if (SelectedIndex > 0) {
                    SelectedIndex--;
                }
                _currentSelectedEngineObject = Engine.Engine.EngineObjectsManager.GetObjectByIndex(SelectedIndex);
            }
        }


        /**
         * OnSelectionChanged method
         * \param sender Sender
         * \param e Selection changed event
         */
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

        /**
         * Clear the list
         */
        public void Clear() {
            Items.Clear();
        }

    }
}
