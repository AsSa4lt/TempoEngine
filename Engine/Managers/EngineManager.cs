﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


namespace TempoEngine.Engine.Managers{

    public static class EngineManager {




        public static readonly double StefanBoltzmannConst = 5.67 * Math.Pow(10, -8);


        // For two objects, transfer radiation heat between them calculated as
        // Q = σ*A*e`*(T1^4 - T2^4)*t. Area is simplified now as the sum of the two areas, it's not true
        public static void TransferRadiationBetweenTwoObjects(EngineObject obj1, EngineObject obj2) {
            List<GrainSquare> obj1Triangles = obj1.GetSquares();
            List<GrainSquare> obj2Triangles = obj2.GetSquares();
            for (int i = 0; i < obj1Triangles.Count; i++) {
                for (int j = 0; j < obj2Triangles.Count; j++) {
                    // Heat transfer formula is calculated by th
                    double area = (obj1Triangles[i].GetNormalizedSideArea() + obj2Triangles[j].GetNormalizedSideArea());
                    double coeficient = MaterialManager.GetEmmisivityBetweenTwoObjects(obj1Triangles[i], obj2Triangles[j]);
                    double tempDif = Math.Pow(obj1Triangles[i].CurrentTemperature,4) - Math.Pow(obj2Triangles[j].CurrentTemperature, 4);
                    double timeTransfer = Engine.EngineIntervalUpdate;
                    double heatTransfer = StefanBoltzmannConst * area * coeficient * tempDif * timeTransfer;
                    double massObj1 = obj1Triangles[i].GetMass();
                    double massObj2 = obj2Triangles[j].GetMass();
                    obj1Triangles[i].CurrentTemperature -= heatTransfer / MaterialManager.GetSpecificHeatCapacity(obj1Triangles[i]) / massObj1;
                    obj1Triangles[i].CurrentTemperature = Math.Max(0, obj1Triangles[i].CurrentTemperature);
                    obj2Triangles[j].CurrentTemperature += heatTransfer / MaterialManager.GetSpecificHeatCapacity(obj2Triangles[j]) / massObj2;
                    obj2Triangles[j].CurrentTemperature = Math.Max(0, obj2Triangles[j].CurrentTemperature);
                }
            }
        }
        public static void TransferRadiationHeatLooseToAir(EngineObject obj) {
            List<GrainSquare> objTriangles = obj.GetSquares();

            for(int i = 0; i < objTriangles.Count; i++) {
                GrainSquare square = objTriangles[i];
                // calculated by Stefan-Boltzmann law of radiation and multiplied by the engine update interval
                double energyRadiationLoss = StefanBoltzmannConst * square.GetNormalizedSideArea() * (Math.Pow(square.CurrentTemperature, 4) - Math.Pow(Engine.AirTemperature, 4)) * Engine.EngineIntervalUpdate;
                square.CurrentTemperature -= energyRadiationLoss / square.Material.SpecificHeatCapacity / square.GetMass();
                square.CurrentTemperature = Math.Max(0, square.CurrentTemperature);
            }
        }

    }
}
