using Core.PathBuilding._2dLocation.Structs;
using UnityEngine;

namespace Core.PathBuilding._2dLocation.Logic
{
    /// <summary>
    /// Класс с набором статических методов на проверку пересечения отрезков и прямых в 2д- координатах
    /// </summary>
    public class LineCrossingChecker
    { 
        
        private static Vector2 Cross(double a1, double b1, double c1, double a2, double b2, double c2)
        {
            
            var x = (b1 * c2 - b2 * c1) / (a1 * b2 - a2 * b1);
            var y = (a2 * c1 - a1 * c2) / (a1 * b2 - a2 * b1);

            return new Vector2((float)x, (float)y);
        }

        /// <summary>
        /// Проверка 2ух прямых на пересечение
        /// </summary>
        /// <param name="pABDot1">точка 1 на прямой 1</param>
        /// <param name="pABDot2">точка 2 на прямой 1</param>
        /// <param name="pCDDot1">точка 1 на прямой 2</param>
        /// <param name="pCDDot2">точка 2 на прямой 2</param>
        /// <param name="pCross">возвращает координаты точки пересечения</param>
        /// <returns>Имеют ли прямые точку пересечения?</returns>
            
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
                //Debug.Log("Прямые перпендикулярны");
                return true;
            }
            

            return true;

        }

        /// <summary>
        /// Проверяет, лежит ли данная точка внутри отрезка
        /// </summary>
        /// <param name="point">Точка, которую надо проверить</param>
        /// <param name="segment">Отрезок, который надо проверить</param>
        /// <returns>Точка лежит на отрезке?</returns>
        public static bool CrossPointLiesOnSegment(Vector2 point, Segment segment)
        {
            var xMin = segment.Point1.x < segment.Point2.x ? segment.Point1.x : segment.Point2.x;
            var xMax = segment.Point1.x < segment.Point2.x ? segment.Point2.x : segment.Point1.x;

            if (point.x > xMax || point.x < xMin)
                return false;
            
            var yMin = segment.Point1.y < segment.Point2.y ? segment.Point1.y : segment.Point2.y;
            var yMax = segment.Point1.y < segment.Point2.y ? segment.Point2.y : segment.Point1.y;

            return point.y >= yMin && point.y <= yMax;

        }
        
        
        /// <summary>
        /// Проверяет 2 отрезка на наличие пересечений и возвращает точку пересечения 
        /// </summary>
        /// <param name="segmentOne">Отрезок номер 1</param>
        /// <param name="segmentTwo">Отрезок номер 2</param>
        /// <param name="crossPoint">Точка пересечения отрезков</param>
        /// <returns>Отрезки пересекаются на плоскости?</returns>
        public static bool GetIntersectionPointIfExists(
            Segment segmentOne, Segment segmentTwo,  out Vector2 crossPoint)
        {
            var lineCrossing =  CheckLinesCrossing(segmentOne.Point1, segmentOne.Point2, segmentTwo.Point1, segmentTwo.Point2, out crossPoint);

            if (!lineCrossing)
                return false;

            if (!CrossPointLiesOnSegment(crossPoint, segmentOne) || !CrossPointLiesOnSegment(crossPoint, segmentTwo))
            {
                return false;
            }

            return true;
        }
    }
}