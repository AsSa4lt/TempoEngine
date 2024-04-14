using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempoEngine.Util {
    internal class TempoThread {
        private readonly Thread _thread;
        public readonly string Name;
        public TempoThread(string name, ThreadStart start) {
            Name = name;
            _thread = new Thread(start) {
                Name = name
            };
        }

        public void Start() {
            _thread.Start();
        }

        public void Join() {
            _thread.Join();
        }

        public void Abort() {
            //_thread.Abort();
        }
    }
}
