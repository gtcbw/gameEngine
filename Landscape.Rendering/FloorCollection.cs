using Engine.Contracts;
using Graphics.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Landscape.Rendering
{
    public sealed class FloorCollection : IRenderingElement, IFloorCollection
    {
        private readonly IBufferedMeshUnitRenderer _bufferedMeshUnitRenderer;
        private readonly Dictionary<int, BufferedMeshUnit> _floorParts = new Dictionary<int, BufferedMeshUnit>();

        public FloorCollection(IBufferedMeshUnitRenderer bufferedMeshUnitRenderer)
        {
            _bufferedMeshUnitRenderer = bufferedMeshUnitRenderer;
        }

        void IRenderingElement.Render()
        {
            foreach (BufferedMeshUnit floor in _floorParts.Values)
                _bufferedMeshUnitRenderer.RenderMesh(floor);
        }

        void IFloorCollection.AddFloorPart(int id, BufferedMeshUnit floorPart)
        {
            if (!_floorParts.Keys.Contains(id))
                _floorParts.Add(id, floorPart);
        }

        void IFloorCollection.RemoveFloorPart(int id)
        {
            if (_floorParts.Keys.Contains(id))
                _floorParts.Remove(id);
        }
    }
}
