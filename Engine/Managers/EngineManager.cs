using System;
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
            List<GrainSquare> obj1squares = obj1.GetSquares();
            List<GrainSquare> obj2squares = obj2.GetSquares();
            for (int i = 0; i < obj1squares.Count; i++) {
                for (int j = 0; j < obj2squares.Count; j++) {
                    // Heat transfer formula is calculated by th
                    double area = (obj1squares[i].GetNormalizedSideArea() + obj2squares[j].GetNormalizedSideArea());
                    double coeficient = MaterialManager.GetEmmisivityBetweenTwoObjects(obj1squares[i], obj2squares[j]);
                    double tempDif = Math.Pow(obj1squares[i].CurrentTemperature,4) - Math.Pow(obj2squares[j].CurrentTemperature, 4);
                    double timeTransfer = Engine.EngineIntervalUpdate;
                    double heatTransfer = StefanBoltzmannConst * area * coeficient * tempDif * timeTransfer;
                    double massObj1 = obj1squares[i].GetMass();
                    double massObj2 = obj2squares[j].GetMass();
                    obj1squares[i].CurrentTemperature -= heatTransfer / MaterialManager.GetSpecificHeatCapacity(obj1squares[i]) / massObj1;
                    obj1squares[i].CurrentTemperature = Math.Max(0, obj1squares[i].CurrentTemperature);
                    obj2squares[j].CurrentTemperature += heatTransfer / MaterialManager.GetSpecificHeatCapacity(obj2squares[j]) / massObj2;
                    obj2squares[j].CurrentTemperature = Math.Max(0, obj2squares[j].CurrentTemperature);
                }
            }
        }
        public static void TransferRadiationHeatLooseToAir(EngineObject obj) {
            List<GrainSquare> objsquares = obj.GetSquares();

            for(int i = 0; i < objsquares.Count; i++) {
                GrainSquare square = objsquares[i];
                // calculated by Stefan-Boltzmann law of radiation and multiplied by the engine update interval
                double energyRadiationLoss = StefanBoltzmannConst * square.GetNormalizedSideArea() * (Math.Pow(square.CurrentTemperature, 4) - Math.Pow(Engine.AirTemperature, 4)) * Engine.EngineIntervalUpdate;
                square.CurrentTemperature -= energyRadiationLoss / square.Material.SpecificHeatCapacity / square.GetMass();
                square.CurrentTemperature = Math.Max(0, square.CurrentTemperature);
            }
        }

    }
}
