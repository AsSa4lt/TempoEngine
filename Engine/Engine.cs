using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoEngine.UIControls;
using TempoEngine.Util;
using Point = System.Windows.Point;

namespace TempoEngine.Engine{
    static class Engine{
        private static readonly ILog log = LogManager.GetLogger(typeof(Engine));

        private static List<EngineObject>? _objects = [];
        // lock object for _objects
        private static object?          _engineLock;
        private static MainWindow?      _mainWindow;
        private static TempoThread?     _engineThread;
        private static long             _lastUpdateTime = 0;

        // updates per second 
        private static readonly int _simulationRefreshRate = 100;
        
        // time of the simulation in microseconds
        private static long _simulationTime = 0; 

        public enum EngineMode {
            Stopped,
            Running,
            Paused
        }

        public static EngineMode Mode { get; private set; } = EngineMode.Stopped;

        public static void Init(MainWindow window){
            _objects = [];
            _engineLock = new object();
            _mainWindow = window;

            log.Info("Engine initialized");

            // add 3 objects to the engine
            GrainTriangle obj1 = new GrainTriangle("Triangle1", new Point(0,0), new Point(0,1), new Point(1,0));
            obj1.Temperature = 200;
            _objects.Add(obj1);
            GrainTriangle obj2 = new GrainTriangle("Triangle2", new Point(1,0), new Point(0,1), new Point(1,1));
            obj1.Temperature = 50;
            _objects.Add(obj2);
            GrainTriangle obj3 = new GrainTriangle("Triangle3", new Point(2,2), new Point(4,4), new Point(2,0));
            obj1.Temperature = 0;
            _objects.Add(obj3);


        }

        public static void Start() {
            if(_engineLock == null)         throw new InvalidOperationException("Engine lock is not initialized");
            lock (_engineLock) {
                log.Info("Engine started");
                Mode = EngineMode.Running;
                _engineThread = new TempoThread("EngineThread", Run);
            }
        }

        private static void prepareObjects() {
            if(_engineLock == null)         throw new InvalidOperationException("Engine lock is not initialized");
            if(_objects == null)            throw new InvalidOperationException("Engine not initialized");

            lock (_engineLock) {
                foreach (var obj in _objects) {
                    obj.SetStartTemperature();
                }
            }
        }

        public static void Run() {
            if(_engineLock == null)         throw new InvalidOperationException("Engine lock is not initialized");

            while (true) {

                // start timer
                long startTime = DateTime.Now.Ticks;

                lock(_engineLock) {
                    if (Mode != EngineMode.Running) break;
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
                if (timeToSleep < 0) { Console.WriteLine("Simulation is running slow"); }
            }
        }


        public static long GetSimulationTime() {
            if (_engineLock == null)        throw new InvalidOperationException("Engine lock is not initialized");
            lock (_engineLock) {
                return _simulationTime;
            }
        }

        public static void Stop() {
            if (_engineLock == null)        throw new InvalidOperationException("Engine lock is not initialized");
            lock (_engineLock) {
                log.Info("Engine stopped");
                Mode = EngineMode.Stopped;
            }
        }

        public static void Pause() {
            if (_engineLock == null)        throw new InvalidOperationException("Engine lock is not initialized");
            lock (_engineLock) {
                log.Info("Engine paused");
                Mode = EngineMode.Paused;
            }
        }

        public static bool IsRunning() {
            if (_engineLock == null)        throw new InvalidOperationException("Engine lock is not initialized");
            lock (_engineLock) {
                return Mode == EngineMode.Running;
            }
        }

        /// TODO: Implement the logic for this function
        /// Check if the position is available for the object, if not return false
        public static bool IsPositionAvailable(EngineObject obj) {
            return true;
        }

        public static List<EngineObject> GetVisibleObjects(CanvasManager manager) {
            List<EngineObject> visibleObjects = new();
            if(_engineLock == null)         throw new InvalidOperationException("Engine lock is not initialized");
            if(_objects == null)            throw new InvalidOperationException("Engine not initialized");

            lock (_engineLock) {
                foreach (var obj in _objects) {
                    if (obj.IsVisible(manager)) {
                        visibleObjects.Add(obj);
                    }
                }
            }
            return visibleObjects;
        }

        // All objects are named and the name is unique
        public static bool IsNameAvailable(string name) {
            if (_engineLock == null)        throw new InvalidOperationException("Engine lock is not initialized");
            if (_objects == null)           throw new InvalidOperationException("Engine not initialized");

            lock (_engineLock) {
                foreach (var obj in _objects) {
                    if (obj.Name == name) {
                        return false;
                    }
                }
            }
            return true;
        }

        public static void AddObject(EngineObject obj) {
            if (_engineLock == null)        throw new InvalidOperationException("Engine lock is not initialized");
            if (_objects == null)           throw new InvalidOperationException("Engine not initialized");
            if (!IsNameAvailable(obj.Name)) throw new InvalidOperationException("Object name is not available");
            lock (_engineLock) {
                _objects.Add(obj);
            }
        }

        public static void RemoveObject(EngineObject obj) {
            if (_engineLock == null)        throw new InvalidOperationException("Engine lock is not initialized");
            if (_objects == null)           throw new InvalidOperationException("Engine not initialized");

            lock (_engineLock) {
                _objects.Remove(obj);
            }
        }

        public static List<EngineObject> GetObjects() {
            if (_engineLock == null)        throw new InvalidOperationException("Engine lock is not initialized");
            if (_objects == null)           throw new InvalidOperationException("Engine not initialized");

            lock (_engineLock) {
                return _objects;
            }
        }
        
        public static void ClearObjects() {
            if (_engineLock == null)        throw new InvalidOperationException("Engine lock is not initialized");
            if (_objects == null)           throw new InvalidOperationException("Engine not initialized");

            lock (_engineLock) {
                _objects.Clear();
            }
            log.Info("All objects cleared");
        }

    }
}
