using Engine.Contracts;
using Graphics.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Landscape.Rendering
{
    public sealed class MeshUnitCollection : IRenderingElement, IMeshUnitCollection
    {
        private readonly IVertexBufferUnitRenderer _bufferedMeshUnitRenderer;
        private readonly Dictionary<int, VertexBufferUnit> _units = new Dictionary<int, VertexBufferUnit>();

        public MeshUnitCollection(IVertexBufferUnitRenderer bufferedMeshUnitRenderer)
        {
            _bufferedMeshUnitRenderer = bufferedMeshUnitRenderer;
        }

        void IRenderingElement.Render()
        {
            foreach (int id in _units.Keys)
            {
                _bufferedMeshUnitRenderer.RenderMesh(_units[id]);
            }
        }

        void IMeshUnitCollection.AddMeshUnit(int id, VertexBufferUnit unit)
        {
            if (!_units.Keys.Contains(id))
                _units.Add(id, unit);
        }

        void IMeshUnitCollection.RemoveMeshUnit(int id)
        {
            if (_units.Keys.Contains(id))
                _units.Remove(id);
        }
    }
}
