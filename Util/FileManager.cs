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
        public static void SaveToFile(string path) {
            List<EngineObject> engineObjects = Engine.Engine.GetObjects();
            List<string> jsonObjects = new List<string>();

            foreach (var obj in engineObjects) {
                string json = obj.GetJsonRepresentation();
                jsonObjects.Add(json);
            }

            string jsonOutput = JsonConvert.SerializeObject(jsonObjects, Formatting.Indented);
            File.WriteAllText(path, jsonOutput);
        }

        public static void LoadFromFile(string path) {
            Engine.Engine.ClearObjects();
            string jsonInput = File.ReadAllText(path);
            List<string> jsonObjects = JsonConvert.DeserializeObject<List<string>>(jsonInput);

            List<EngineObject> engineObjects = new List<EngineObject>();

            foreach (var json in jsonObjects) {
                var jObject = JsonConvert.DeserializeObject<dynamic>(json);
                EngineObject engineObject = null;

                // Assuming you have a type identifier in your JSON
                string type = jObject["Type"].Value;
                if (type == "GrainTriangle") {
                    engineObject = GrainTriangle.FromJson(json);
                }
                // Add more types as necessary
                if (engineObject != null) {
                    engineObjects.Add(engineObject);
                    Engine.Engine.AddObject(engineObject);
                }
            }
        }
    }
}
