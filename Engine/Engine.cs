using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoEngine.UIControls;
using TempoEngine.Util;
using TempoEngine.Engine.Managers;
using Point = System.Windows.Point;
using TempoEngine.Engine.Examples;
using System.Diagnostics;

namespace TempoEngine.Engine{
    static class Engine{
        private static readonly ILog log = LogManager.GetLogger(typeof(Engine));
        public static readonly double GridStep = 0.001; // 1mm

        private static List<EngineObject>? _objects = [];
        // lock object for _objects
        private static object?          _engineLock;
        private static MainWindow?      _mainWindow;
        private static TempoThread?     _engineThread;
        private static long             _lastUpdateTime = 0;
        public static double            EngineIntervalUpdate = 0;
         static int frames = 0;

        // updates per second 
        private static int _simulationRefreshRate = 60;
        
        // time of the simulation in microseconds
        private static long _simulationTime = 0; 

        // should we optimize the engine by setting adjacent squares to be touching
        public static bool Optimize = true;

        public enum EngineMode {
            Stopped,
            Running,
            Paused
        }

        public static EngineMode Mode { get; private set; } = EngineMode.Stopped;

        public static readonly double AirTemperature = 293;

        public static void Init(MainWindow window){
            _objects = [];
            _engineLock = new object();
            _mainWindow = window;
            MaterialManager.Init();
            SimpleExamples.RectangleWithTempDifference(15, 15);
            _simulationRefreshRate = Util.SystemInfo.GetRefreshRate();
            log.Info("Engine initialized");
        }
        public static void Start() {
            if(_engineLock == null)         throw new InvalidOperationException("Engine lock is not initialized");
            lock (_engineLock) {
                log.Info("Engine started");
                prepareObjects();
                Mode = EngineMode.Running;
                frames = 0;
                EngineIntervalUpdate = 1/(double)Util.SystemInfo.GetRefreshRate();
                _engineThread = new TempoThread("EngineThread", Run);
                _engineThread.SetPriority(ThreadPriority.Highest);
                _engineThread.Start();
            }
        }

        private static void prepareObjects() {
            if(_engineLock == null)         throw new InvalidOperationException("Engine lock is not initialized");
            if(_objects == null)            throw new InvalidOperationException("Engine not initialized");

            lock (_engineLock) {
                foreach (var obj in _objects) {
                    obj.SetStartTemperature();
                }
                if (Optimize) {
                    OptimizationManager.Optimize(_objects);
                }
            }
        }

        public static void Run() {
            if(_engineLock == null)         throw new InvalidOperationException("Engine lock is not initialized");
            if(_objects == null)            throw new InvalidOperationException("Engine not initialized");

            Stopwatch stopwatch = new Stopwatch();

            while (true) {
                stopwatch.Restart();
                double msPerFrame = 1000.0 / (double)_simulationRefreshRate;  // milliseconds per frame

                lock (_engineLock) {
                    if (Mode != EngineMode.Running) break;
                }
                // update logic

                // simplify the logic for now
                RadiationTransferManager.TransferRadiationHeat(_objects);
                ConductionTransferManager.TransferConductionHeat(_objects);

                // apply results to the UI
                for(int i = 0; i < _objects.Count; i++) {
                    List<GrainSquare> list = _objects[i].GetSquares();
                    for(int j = 0; j < list.Count; j++) {
                        list[j].ApplyEnergyDelta();
                    }
                }

                double elapsedTimeMs = stopwatch.ElapsedTicks/10000;
                if (msPerFrame - elapsedTimeMs < 0) {
                    log.Info($"Simulation is too slow, time is {elapsedTimeMs - msPerFrame} ms");
                }

                while (msPerFrame - elapsedTimeMs > 0) {
                    elapsedTimeMs = stopwatch.ElapsedTicks/10000;
                }
                stopwatch.Stop();


                lock (_engineLock) {
                    frames++;
                    _simulationTime = (int)(frames * msPerFrame);
                }
            }
        }

        public static EngineMode GetMode() {
            if (_engineLock == null)        throw new InvalidOperationException("Engine lock is not initialized");
            lock (_engineLock) {
                return Mode;
            }
        }

        public static long GetSimulationTime() {
            if (_engineLock == null)        throw new InvalidOperationException("Engine lock is not initialized");
            lock (_engineLock) {
                return _simulationTime;
            }
        }

        public static long GetSimulationTimeUnsafe() {
            return _simulationTime;
        }

        public static void Stop() {
            if (_engineLock == null)        throw new InvalidOperationException("Engine lock is not initialized");
            lock (_engineLock) {
                log.Info("Engine stopped");
                _simulationTime = 0;
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

        public static List<EngineObject> GetVisibleObjectsUnsafe(CanvasManager manager) {
            List<EngineObject> visibleObjects = new();
            if(_engineLock == null)         throw new InvalidOperationException("Engine lock is not initialized");
            if(_objects == null)            throw new InvalidOperationException("Engine not initialized");

            foreach (var obj in _objects) {
                if (obj.IsVisible(manager)) {
                    visibleObjects.Add(obj);
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

        public static EngineObject GetObjectByIndex(int index) {
            if (_engineLock == null)        throw new InvalidOperationException("Engine lock is not initialized");
            if (_objects == null)           throw new InvalidOperationException("Engine not initialized");
            if (index < 0 || index >= _objects.Count) throw new InvalidOperationException("Index out of bounds");

            lock (_engineLock) {
                return _objects[index];
            }
        }

        public static List<EngineObject> GetObjectsUnsafe() {
            if (_engineLock == null)        throw new InvalidOperationException("Engine lock is not initialized");
            if (_objects == null)           throw new InvalidOperationException("Engine not initialized");

            lock (_engineLock) {
                return _objects;
            }
        }

        public static void ResetSimulation() {
            if (Mode == EngineMode.Running) throw new InvalidOperationException("Cannot reset simulation while running");
            if(_objects == null)            throw new InvalidOperationException("Engine not initialized");

            foreach(var obj in _objects) {
                obj.SetStartTemperature();
            }
            log.Info("Simulation reset");
            _mainWindow.UpdateAll();
        }

        public static void ClearObjects() {
            if (_engineLock == null)        throw new InvalidOperationException("Engine lock is not initialized");
            if (_objects == null)           throw new InvalidOperationException("Engine not initialized");

            lock (_engineLock) {
                _objects.Clear();
            }
            log.Info("All objects cleared");
            _mainWindow.UpdateAll();
        }

    }
}
