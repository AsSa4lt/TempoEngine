using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempoEngine.Engine.Managers {
    internal class MaterialManager {

        public static List<Material> Materials = [];

        public static void Init() {
            populateWithBaseMaterials();
        }

        private static void populateWithBaseMaterials() {
            Material m1 = new Material();
            m1.Name = "Aluminium";
            m1.isBaseMaterial = true;
            m1.SpecificHeatCapacity = 900;
            m1.Density = 2700;
            m1.Emmisivity = 0.03;
            Materials.Add(m1);
        }

        public static Material GetBaseMaterial() {
            if(Materials.Count == 0) throw new Exception("Materials are not initialized");
            return Materials[0];
        }

        public static Material GetMaterialByName(string name) {
            return Materials.Find(x => x.Name == name);
        }

        public static List<Material> GetMaterials() {
           return Materials;
        }

        // now we assume that material is constant and it's aluminum
        public static double GetCoeficientFromMaterial(GrainSquare obj1, GrainSquare obj2) {
            return 2.05;
        }

        public static double GetSpecificHeatCapacity(GrainSquare obj) {
            return 900;
        }

        public static double GetDensity(GrainSquare obj) {
            return 2700;
        }


        // calculated as e'=(e1*e2)/(e1+e2 - e1*e2)
        // 03 is the default value for the emissivity of the object for aluminium  
        public static double GetEmmisivityBetweenTwoObjects(GrainSquare obj1, GrainSquare obj2) {
            return 0.3;
        }

    }
}
