using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TempoEngine.Engine;


namespace TempoEngine.UIControls {
    public class EngineObjectsList : System.Windows.Controls.ListView {
        private EngineObject? _currentSelectedEngineObject;
        public Action<EngineObject>? OnSelectedObjectChanged;


        public EngineObjectsList() : base() {
            this.MouseDoubleClick += OnItemDoubleClick;
            this.SelectionChanged += OnSelectionChanged;
        }
        public void Update(List<EngineObject> objects) {
            Clear();
            foreach (var obj in objects) {
                Items.Add(obj);
            }
        }

        private void OnItemDoubleClick(object sender, MouseButtonEventArgs e) {
            var item = this.SelectedItem as EngineObject;
            if (item != null) {
                _currentSelectedEngineObject?.Deselect();
                _currentSelectedEngineObject = item;
                _currentSelectedEngineObject.Select();
                OnSelectedObjectChanged?.Invoke(_currentSelectedEngineObject);
                this.Items.Refresh();
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            /*if (SelectedItem != null) { 
                var selectedObject = SelectedItem as EngineObject ?? throw new Exception("Selected object is not an EngineObject");
                _currentSelectedEngineObject?.Deselect();
                _currentSelectedEngineObject = selectedObject;
                _currentSelectedEngineObject.Select();
                OnSelectedObjectChanged?.Invoke(_currentSelectedEngineObject);
            }*/
        }
        public void Clear() {
            Items.Clear();
        }

    }
}
