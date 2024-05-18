using System;
using System.Drawing;
using UnityEngine;

namespace SpatialAudio.Code
{
    public class LineCrossingChecker
    { 
        
        private static Vector2 Cross(double a1, double b1, double c1, double a2, double b2, double c2)
        {
            
            var x = (b1 * c2 - b2 * c1) / (a1 * b2 - a2 * b1);
            var y = (a2 * c1 - a1 * c2) / (a1 * b2 - a2 * b1);

            return new Vector2((float)x, (float)y);
        }

            
        public static bool CheckLinesCrossing(Vector2 pABDot1, Vector2 pABDot2, 
            Vector2 pCDDot1, Vector2 pCDDot2, out Vector2 pCross)
        {
            pCross = new Vector2();
            double a1 = pABDot2.y - pABDot1.y;
            double b1 = pABDot1.x - pABDot2.x;
            double c1 = -pABDot1.x * pABDot2.y + pABDot1.y * pABDot2.x;

            double a2 = pCDDot2.y - pCDDot1.y;
            double b2 = pCDDot1.x - pCDDot2.x;
            double c2 = -pCDDot1.x * pCDDot2.y + pCDDot1.y * pCDDot2.x;
            
            // Прямые параллельны
            if ((a1 * b2 - a2 * b1) == 0)
            {
                
                //Debug.Log("Прямые параллельны");
                if (a1 * b2 == b1 * a2 && a1 * c2 == a2 * c1 && b1 * c2 == c1 * b2)
                {
                   // Debug.Log("Прямые совпадают");
                }

                
                return false;
            }

            //  --------------  Прямые пересекаются ----------------------

            // Точка пересечения прямых
            pCross = Cross(a1, b1, c1, a2, b2, c2);
            
            // Прямые перпендикулярны
            if ((a1 * a2 + b1 * b2) == 0)
            {
                Debug.Log("Прямые перпендикулярны");
                return true;
            }
            

            return true;

        }

        public static bool PointOnSegment(Vector2 point, Segment segment)
        {
            var xMin = segment.Point1.x < segment.Point2.x ? segment.Point1.x : segment.Point2.x;
            var xMax = segment.Point1.x < segment.Point2.x ? segment.Point2.x : segment.Point1.x;

            if (point.x > xMax || point.x < xMin)
                return false;
            
            var yMin = segment.Point1.y < segment.Point2.y ? segment.Point1.y : segment.Point2.y;
            var yMax = segment.Point1.y < segment.Point2.y ? segment.Point2.y : segment.Point1.y;

            return point.y >= yMin && point.y <= yMax;

        }
        
        
        
        public static bool GetIntersectionPoint(
            Segment segmentOne, Segment segmentTwo,  out Vector2 crossPoint)
        {
            var lineCrossing =  CheckLinesCrossing(segmentOne.Point1, segmentOne.Point2, segmentTwo.Point1, segmentTwo.Point2, out crossPoint);

            if (!lineCrossing)
                return false;

            if (!PointOnSegment(crossPoint, segmentOne) || !PointOnSegment(crossPoint, segmentTwo))
            {
                return false;
            }

            return true;
        }
    }
}