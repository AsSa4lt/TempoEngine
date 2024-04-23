﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoEngine.UIControls;
using TempoEngine.Util;
using Point = System.Windows.Point;

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
            GrainTriangle obj1 = new GrainTriangle("Triangle1", new Point(0,0), new Point(0,1), new Point(1,0));
            obj1.SetTemperature(200);
            _objects.Add(obj1);
            GrainTriangle obj2 = new GrainTriangle("Triangle2", new Point(1,0), new Point(0,1), new Point(1,1));
            obj1.SetTemperature(50);
            _objects.Add(obj2);
            GrainTriangle obj3 = new GrainTriangle("Triangle3", new Point(2,2), new Point(4,4), new Point(2,0));
            obj1.SetTemperature(0);
            _objects.Add(obj3);


        }

        public static void Start() {
            if(_engineLock == null)         throw new InvalidOperationException("Engine lock is not initialized");
            lock (_engineLock) {
                _isRunning = true;
                _engineThread = new TempoThread("EngineThread", Run);
            }
        }

        public static void Run() {
            if(_engineLock == null)         throw new InvalidOperationException("Engine lock is not initialized");

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
            if (_engineLock == null)        throw new InvalidOperationException("Engine lock is not initialized");
            lock (_engineLock) {
                return _simulationTime;
            }
        }

        public static void Stop() {
            if (_engineLock == null)        throw new InvalidOperationException("Engine lock is not initialized");
            lock (_engineLock) {
                _isRunning = false;
            }
        }

        public static bool IsRunning() {
            if (_engineLock == null)        throw new InvalidOperationException("Engine lock is not initialized");
            lock (_engineLock) {
                return _isRunning;
            }
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
        public static bool isNameAvailable(string name) {
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
            if (!isNameAvailable(obj.Name)) throw new InvalidOperationException("Object name is not available");
            lock (_engineLock) {
                _objects.Add(obj);
            }
        }

        public static List<EngineObject> GetObjects() {
            if (_engineLock == null)        throw new InvalidOperationException("Engine lock is not initialized");
            if (_objects == null)           throw new InvalidOperationException("Engine not initialized");

            lock (_engineLock) {
                return _objects;
            }
        }   

    }
}
