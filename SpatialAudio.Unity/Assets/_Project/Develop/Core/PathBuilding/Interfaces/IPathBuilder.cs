using Core.PathBuilding.Model;

namespace Core.PathBuilding.Interfaces
{
    /// <summary>
    /// Интерфейс класса, который строит граф возможного пути звука от источников к слушателю
    /// </summary>
    public interface IPathBuilder
    {
        /// <summary>
        /// Заного создать граф пути и рассчитать все расстояния
        /// </summary>
        /// <returns></returns>
        SoundPathGraph CreateGraph();

        /// <summary>
        /// Обновить состояние графа при перемещении слушателя
        /// </summary>
        void UpdateListener();

        /// <summary>
        /// Обновить состояние графа при перемещении источника звука
        /// </summary>
        void UpdateSource();
    }
}