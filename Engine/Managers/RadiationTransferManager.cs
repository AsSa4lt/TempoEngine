using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


namespace TempoEngine.Engine.Managers{

    public static class RadiationTransferManager {

        public static readonly double StefanBoltzmannConst = 5.67 * Math.Pow(10, -8) * Math.Pow(10, -2);
        /**
         * Transfer radiation heat between two objects
         * Q = σ*A*e`*(T1^4 - T2^4)*t. 
         * \param obj1 first object
         * \param obj2 second object
         */
        private static void TransferRadiationBetweenTwoObjects(EngineObject obj1, EngineObject obj2) {
            List<GrainSquare> obj1squares = obj1.GetSquares();
            List<GrainSquare> obj2squares = obj2.GetSquares();
            for (int i = 0; i < obj1squares.Count; i++) {
                for (int j = 0; j < obj2squares.Count; j++) {
                    
                    
                }
            }
        }
        private static void TransferRadiationHeatLooseToAir(EngineObject obj) {
            List<GrainSquare> objsquares = obj.GetSquares();

            for(int i = 0; i < objsquares.Count; i++) {
                GrainSquare square = objsquares[i];
                // calculated by Stefan-Boltzmann law of radiation and multiplied by the engine update interval
                double energyRadiationLoss = StefanBoltzmannConst * square.GetNormalizedSideArea() * (Math.Pow(square.CurrentTemperature, 4) - Math.Pow(Engine.AirTemperature, 4)) * Engine.EngineIntervalUpdate;
                square.CurrentTemperature -= energyRadiationLoss / square.Material.SpecificHeatCapacity / square.GetMass();
                square.CurrentTemperature = Math.Max(0, square.CurrentTemperature);
            }
        }

        /**
         * 
         * Transfer radiation heat for all objects
         * First, transfer radiation heat loss to air
         * Second, transfer radiation heat between objects
         * \param objects list of objects
         */
        public static void TransferRadiationHeatForObjects(List<EngineObject> objects){
            foreach(var obj in objects){
                TransferRadiationHeatLooseToAir(obj);
            }
        }

    }
}
