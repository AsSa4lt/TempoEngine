using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoEngine.Util;

namespace TempoEngine.Engine{
    static class Engine{
        private static List<EngineObject>? _objects = [];
        // lock object for _objects
        private static object? _engineLock;
        private static MainWindow? _mainWindow;
        private static bool _isRunning = false;
        private static TempoThread? _engineThread;
        private static int _refreshRate;

        public static void Init(MainWindow window){
            _objects = [];
            _engineLock = new object();
            _mainWindow = window;
        }

        public static void Start() {
            if(_engineLock == null) throw new InvalidOperationException("Engine lock is not initialized");
            lock (_engineLock) {
                _isRunning = true;
                _engineThread = new TempoThread("EngineThread", Run);
                _refreshRate = SystemInfo.GetRefreshRate();
            }
        }

        public static void Run() {
            if(_engineLock == null) throw new InvalidOperationException("Engine lock is not initialized");

            while (true) {
                lock(_engineLock) {
                    if (!_isRunning) break;
                }
                // update logic

                
            }
        }

        public static void Stop() {
            if (_engineLock == null) throw new InvalidOperationException("Engine lock is not initialized");
            lock (_engineLock) {
                _isRunning = false;
            }
        }

        public static List<EngineObject> GetObjects() {
            if (_engineLock == null) throw new InvalidOperationException("Engine lock is not initialized");
            if (_objects == null) throw new InvalidOperationException("Engine not initialized");

            lock (_engineLock) {
                return _objects;
            }
        }   

    }
}
