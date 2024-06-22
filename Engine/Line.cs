using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = System.Windows.Point;

namespace TempoEngine.Engine {

    public struct Line {
        public Point Start, End;

        public Line(Point start, Point end) {
            Start = start;
            End = end;
        }

        // Calculate the length of the line segment
        public double Length() {
            return Math.Sqrt((End.X - Start.X) * (End.X - Start.X) + (End.Y - Start.Y) * (End.Y - Start.Y));
        }

        // Determine if two line segments are collinear
        public static bool AreCollinear(Line l1, Line l2) {
            // Check if the two lines are collinear by cross product being zero
            return (l1.End.X - l1.Start.X) * (l2.End.Y - l2.Start.Y) ==
                   (l2.End.X - l2.Start.X) * (l1.End.Y - l1.Start.Y);
        }

        // Calculate the overlap length between two collinear line segments
        public static double CalculateOverlap(Line l1, Line l2) {
            if (!AreCollinear(l1, l2))
                return 0;

            // Sort points to make comparison easier
            Point l1p1 = l1.Start.X < l1.End.X ? l1.Start : l1.End;
            Point l1p2 = l1.Start.X < l1.End.X ? l1.End : l1.Start;
            Point l2p1 = l2.Start.X < l2.End.X ? l2.Start : l2.End;
            Point l2p2 = l2.Start.X < l2.End.X ? l2.End : l2.Start;

            // Check for overlap
            if (l1p2.X < l2p1.X || l2p2.X < l1p1.X)
                return 0;  // No overlap

            // Calculate overlap
            double start = Math.Max(l1p1.X, l2p1.X);
            double end = Math.Min(l1p2.X, l2p2.X);
            return end - start;
        }
    }

}
