using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using TempoEngine.UIControls;

namespace TempoEngine.Engine {
    /**
     * \class EngineRectangle
     * \brief Object of the engine that represents rectangle
     * 
     * 
     * Manages itself and makes it easier to calculate transfers
     * \see EngineObject
     * \see CanvasManager
     */
    public class EngineRectangle : EngineObject {
        private List<GrainSquare> _grainSquares;
        public EngineRectangle(string name, int width, int height) : base(name) {
            _size = new(width, height);
        }

        public override List<GrainSquare> GetExternalSquares() {
            throw new NotImplementedException();
        }

        public override string GetJsonRepresentation() {
            throw new NotImplementedException();
        }


        /**
         * \brief Gets the type of the object.
         * \returns The type of the object.
         */ 
        public override ObjectType GetObjectType() {
            return ObjectType.Rectangle;
        }

        public override string GetObjectTypeString() {
            return "Rectangle";
        }

        public override void GetObjectVisibleArea(out Vector2 topLeft, out Vector2 bottomRight) {
            throw new NotImplementedException();
        }

        public override List<Polygon> GetPolygons() {
            throw new NotImplementedException();
        }

        public override List<GrainSquare> GetSquares() {
            throw new NotImplementedException();
        }

        public override bool IsIntersecting(EngineObject obj) {
            throw new NotImplementedException();
        }

        public override bool IsVisible(CanvasManager canvasManager) {
            throw new NotImplementedException();
        }

        public override void SetStartTemperature() {
            throw new NotImplementedException();
        }
    }
}
