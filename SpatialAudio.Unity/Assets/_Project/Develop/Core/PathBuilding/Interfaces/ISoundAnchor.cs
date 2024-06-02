using UnityEngine;

namespace Core.PathBuilding.Interfaces
{
    /// <summary>
    /// Интурфес сущности, которая учасвует в процессе рассчета звука
    /// </summary>
    public interface ISoundAnchor
    {
        /// <summary>
        /// Позиция сущности в текущий момент времени
        /// </summary>
        public Vector2 Position { get; }
    }
}