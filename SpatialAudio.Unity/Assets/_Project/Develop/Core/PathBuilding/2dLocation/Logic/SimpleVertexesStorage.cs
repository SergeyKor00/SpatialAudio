using System.Collections.Generic;
using Core.PathBuilding._2dLocation.Structs;
using Core.PathBuilding.Interfaces;

namespace Core.PathBuilding._2dLocation.Logic
{
    public class SimpleVertexesStorage : IVertexStorage
    {
        private Dictionary<int, Vertex> _vertexesOnScene = new Dictionary<int, Vertex>();
       

        private int nextVertexKey;
        
      

        public int AddVertexAndReturnId(Vertex vertex)
        {
            vertex.Id = nextVertexKey;
            _vertexesOnScene.Add(vertex.Id, vertex);
            nextVertexKey++;
            return vertex.Id;
        }

        public bool VertexExists(int id)
        {
            return _vertexesOnScene.ContainsKey(id);
        }

        public Vertex GetVertexById(int id)
        {
            return _vertexesOnScene[id];
        }

        public IEnumerable<Vertex> GetAllVertexes()
        {
            return _vertexesOnScene.Values;
        }

        public void Clear()
        {
            _vertexesOnScene.Clear();
        }
    }
}