using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TempoEngine.Engine;


namespace TempoEngine.UIControls {
    public class EngineObjectsList : System.Windows.Controls.ListView {

        public void Update(List<EngineObject> objects) {
            Clear();
            foreach (var obj in objects) {
                Items.Add(obj.Name);
            }
        }


        public void Clear() {
            Items.Clear();
        }

    }
}
