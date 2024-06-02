using Core.PathBuilding.Interfaces;
using UnityEngine;

namespace Core.PathBuilding._2dLocation.Structs
{
    /// <summary>
    /// Точка, которая является краем линии препятствия
    /// </summary>
    public class Vertex : ISoundAnchor
    {
        public int Id;
        public Vector2 Position { get; private set; }
        public Line LeftLine, RightLine;

        public Vertex(Vector2 position)
        {
            Position = position;
        }
    }
}