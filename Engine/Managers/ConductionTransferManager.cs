using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempoEngine.Engine.Managers {
    internal class ConductionTransferManager {

        // fourier's law of heat conduction, take a look at the documentation
        public static void TranferHeatBetweenTwoSquares(GrainSquare sq1, GrainSquare sq2) {
            double temperatureDifference = sq1.CurrentTemperature - sq2.CurrentTemperature;

            double coeficient = MaterialManager.GetCoeficientFromMaterial(sq1, sq2);
            double timeTransfer = Engine.EngineIntervalUpdate;
            double heatTransfer = coeficient  * temperatureDifference * timeTransfer;
            sq1.AddEnergyDelta(-heatTransfer);
            sq2.AddEnergyDelta(heatTransfer);
        }


        public static void TransferHeatForObject(EngineObject obj) {
            List<GrainSquare> objsquares = obj.GetSquares();
            for (int i = 0; i < objsquares.Count; i++) {
                List<GrainSquare> adjSquares = objsquares[i].GetAdjacentSquares();
                for (int j = 0; j < adjSquares.Count; j++) {
                    TranferHeatBetweenTwoSquares(objsquares[i], adjSquares[j]);
                }
            }
        }
    }
}
