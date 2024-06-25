using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoEngine.Engine;

namespace TempoEngine.Util {
    public static class FileManager {
        private static readonly ILog log = LogManager.GetLogger(typeof(FileManager));

        public static void SaveToFile(string path) {
            List<EngineObject> engineObjects = Engine.Engine.GetObjects();
            List<string> jsonObjects = new List<string>();

            log.Info("Info: Starting saving.");

            try {
                foreach (var obj in engineObjects) {
                    string json = obj.GetJsonRepresentation();
                    jsonObjects.Add(json);
                }
            }catch(Exception e) {
                log.Error("Error: " + e.Message);
            }

            string jsonOutput = JsonConvert.SerializeObject(jsonObjects, Formatting.Indented);
            File.WriteAllText(path, jsonOutput);
        }

        public static void LoadFromFile(string path) {
            Engine.Engine.ClearObjects();
            log.Info("Info: Starting reading.");

            string jsonInput = File.ReadAllText(path);
            List<string> jsonObjects = JsonConvert.DeserializeObject<List<string>>(jsonInput);

            List<EngineObject> engineObjects = new List<EngineObject>();
            try {
                foreach (var json in jsonObjects) {
                    var jObject = JsonConvert.DeserializeObject<dynamic>(json);
                    EngineObject engineObject = null;

                    // Assuming you have a type identifier in your JSON
                    string type = jObject["Type"].Value;
                    if (type == "GrainTriangle") {
                        engineObject = GrainSquare.FromJson(json);
                    }
                    // Add more types as necessary
                    if (engineObject != null) {
                        engineObjects.Add(engineObject);
                        Engine.Engine.AddObject(engineObject);
                    }
                }
            }catch(Exception e) {
                log.Error("Error: " + e.Message);
            }
        }
    }
}
