using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempoEngine.Engine.Managers {
    internal class ConductionTransferManager {

        // fourier's law of heat conduction, take a look at the documentation
        public static void TranferHeatBetweenTwoSquares(GrainSquare sq1, GrainSquare sq2) {
            if (!(sq1.AreTouching(sq2)))
                return;
            double temperatureDifference = sq1.CurrentTemperature - sq2.CurrentTemperature;

            double coeficient = MaterialManager.GetCoeficientFromMaterial(sq1, sq2);
            double timeTransfer = Engine.EngineIntervalUpdate;
            double heatTransfer = coeficient  * temperatureDifference * timeTransfer;
            sq1.AddEnergyDelta(-heatTransfer);
            sq2.AddEnergyDelta(heatTransfer);
        }


        public static void TransferHeatForObject(EngineObject obj) {
            List<GrainSquare> objTriangles = obj.GetSquares();
            for (int i = 0; i < objTriangles.Count; i++) {
                List<GrainSquare> adjSquares = objTriangles[i].GetAdjacentSquares();
                for (int j = 0; j < adjSquares.Count; j++) {
                    TranferHeatBetweenTwoSquares(objTriangles[i], adjSquares[j]);
                }
            }
        }

        public static void TranferHeatBetweenTwoObjects(EngineObject obj1, EngineObject obj2) {
            List<GrainSquare> obj1Triangles = obj1.GetSquares();
            List<GrainSquare> obj2Triangles = obj2.GetSquares();
            for (int i = 0; i < obj1Triangles.Count; i++) {
                for (int j = 0; j < obj2Triangles.Count; j++) {
                    // Heat transfer formula is calculated by the formula Q = k * A * deltaT * t / d
                    double area = Engine.GridStep;
                    if (!(obj1Triangles[i].AreTouching(obj2Triangles[j])))
                        continue;
                    double temperatureDifference = obj1Triangles[i].CurrentTemperature - obj2Triangles[j].CurrentTemperature;

                    double coeficient = MaterialManager.GetCoeficientFromMaterial(obj1Triangles[i], obj2Triangles[j]);
                    double timeTransfer = Engine.EngineIntervalUpdate;
                    // FIXME: I need to calculate thickness of the object??????????????
                    double heatTransfer = coeficient * area * temperatureDifference * timeTransfer * EngineObject.Width * EngineObject.Width;
                    obj1Triangles[i].AddEnergyDelta(-heatTransfer);
                    obj2Triangles[j].AddEnergyDelta(heatTransfer);
                }
            }
        }
    }
}
