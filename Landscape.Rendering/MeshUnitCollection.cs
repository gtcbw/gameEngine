using Engine.Contracts;
using Graphics.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Landscape.Rendering
{
    public sealed class MeshUnitCollection : IRenderingElement, IMeshUnitCollection
    {
        private readonly IBufferedMeshUnitRenderer _bufferedMeshUnitRenderer;
        private readonly Dictionary<int, BufferedMeshUnit> _units = new Dictionary<int, BufferedMeshUnit>();

        public MeshUnitCollection(IBufferedMeshUnitRenderer bufferedMeshUnitRenderer)
        {
            _bufferedMeshUnitRenderer = bufferedMeshUnitRenderer;
        }

        void IRenderingElement.Render()
        {
            foreach (BufferedMeshUnit floor in _units.Values)
                _bufferedMeshUnitRenderer.RenderMesh(floor);
        }

        void IMeshUnitCollection.AddMeshUnit(int id, BufferedMeshUnit unit)
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
