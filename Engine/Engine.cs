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

        // updates per second 
        private static readonly int _simulationRefreshRate = 100;
        
        // time of the simulation in microseconds
        private static long _simulationTime = 0;

        public static void Init(MainWindow window){
            _objects = [];
            _engineLock = new object();
            _mainWindow = window;

            // add 3 objects to the engine
            _objects.Add(new EngineObject("Object1"));
            _objects.Add(new EngineObject("Object2"));
            _objects.Add(new EngineObject("Object3"));
        }

        public static void Start() {
            if(_engineLock == null) throw new InvalidOperationException("Engine lock is not initialized");
            lock (_engineLock) {
                _isRunning = true;
                _engineThread = new TempoThread("EngineThread", Run);
            }
        }

        public static void Run() {
            if(_engineLock == null) throw new InvalidOperationException("Engine lock is not initialized");

            while (true) {

                // start timer
                long startTime = DateTime.Now.Ticks;

                lock(_engineLock) {
                    if (!_isRunning) break;
                }
                // update logic

                // check if elapsed time is less than the time of one update as 1/refresh rate
                long elapsedTime = DateTime.Now.Ticks - startTime;
                long timeToSleep = 1000000 / _simulationRefreshRate - elapsedTime;
                // update time of the simulation
                lock (_engineLock) {
                    _simulationTime += 1000000 / _simulationRefreshRate;
                }
                if (timeToSleep > 0) { Thread.Sleep((int)(timeToSleep / 1000)); }
            }
        }


        public static long GetSimulationTime() {
            if (_engineLock == null) throw new InvalidOperationException("Engine lock is not initialized");
            lock (_engineLock) {
                return _simulationTime;
            }
        }

        public static void Stop() {
            if (_engineLock == null) throw new InvalidOperationException("Engine lock is not initialized");
            lock (_engineLock) {
                _isRunning = false;
            }
        }

        public static bool IsRunning() {
            if (_engineLock == null) throw new InvalidOperationException("Engine lock is not initialized");
            lock (_engineLock) {
                return _isRunning;
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
