using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempoEngine.Util{
    static class CanvasParameters{
        // X > 0 parameters
        public static int MinLeftXIndex = -1000;
        public static int MaxLeftXIndex = -10;
        
        // X < 0 parameters
        public static int MaxRightXIndex = 1000;
        public static int MinRightXIndex = 10;

        // Y > 0 parameters
        public static int MaxYTopIndex = 1000;
        public static int MinYTopIndex = 10;

        // Y < 0 parameters
        public static int MinYBottomIndex = -1000;
        public static int MaxYBottomIndex = -10;

    }
}
