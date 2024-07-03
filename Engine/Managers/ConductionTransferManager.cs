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

        public static void TranferHeatBetweenTwoObjects(EngineObject obj1, EngineObject obj2) {
            List<GrainSquare> obj1squares = obj1.GetSquares();
            List<GrainSquare> obj2squares = obj2.GetSquares();
            for (int i = 0; i < obj1squares.Count; i++) {
                for (int j = 0; j < obj2squares.Count; j++) {
                    // Heat transfer formula is calculated by the formula Q = k * A * deltaT * t / d
                    double area = Engine.GridStep;
                    if (!(obj1squares[i].AreTouching(obj2squares[j])))
                        continue;
                    double temperatureDifference = obj1squares[i].CurrentTemperature - obj2squares[j].CurrentTemperature;

                    double coeficient = MaterialManager.GetCoeficientFromMaterial(obj1squares[i], obj2squares[j]);
                    double timeTransfer = Engine.EngineIntervalUpdate;
                    // FIXME: I need to calculate thickness of the object??????????????
                    double heatTransfer = coeficient * area * temperatureDifference * timeTransfer * Engine.GridStep * Engine.GridStep;
                    obj1squares[i].AddEnergyDelta(-heatTransfer);
                    obj2squares[j].AddEnergyDelta(heatTransfer);
                }
            }
        }
    }
}
