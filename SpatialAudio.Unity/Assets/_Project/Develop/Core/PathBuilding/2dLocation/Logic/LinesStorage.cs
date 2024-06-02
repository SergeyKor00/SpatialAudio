using System.Collections.Generic;
using Core.PathBuilding._2dLocation.Structs;

namespace Core.PathBuilding._2dLocation.Logic
{
    public class LinesStorage
    {
        private Dictionary<int, Line> _vertexesOnScene = new Dictionary<int, Line>();
       

        private int nextVertexKey;
        
      

        public int AddLineAndReturnId(Line vertex)
        {
            vertex.Id = nextVertexKey;
            _vertexesOnScene.Add(vertex.Id, vertex);
            nextVertexKey++;
            return vertex.Id;
        }

        public bool LineExists(int id)
        {
            return _vertexesOnScene.ContainsKey(id);
        }

        public Line GetLineById(int id)
        {
            return _vertexesOnScene[id];
        }

        public IEnumerable<Line> GetAllLines()
        {
            return _vertexesOnScene.Values;
        }

        public void Clear()
        {
            _vertexesOnScene.Clear();
        }
    }
}