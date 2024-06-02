using System.Collections;
using System.Collections.Generic;
using Core.PathBuilding._2dLocation.Structs;

namespace Core.PathBuilding.Interfaces
{
    public interface IVertexStorage
    {
        public int AddVertexAndReturnId(Vertex vertex);

        public bool VertexExists(int id);

        public Vertex GetVertexById(int id);

        public IEnumerable<Vertex> GetAllVertexes();

        public void Clear();
    }
}