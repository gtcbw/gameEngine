using Engine.Contracts;
using Graphics.Contracts;

namespace Engine.Framework
{
    public sealed class CullingDeactivator : IRenderingElement
    {
        private readonly ICullingController _cullingController;
        private readonly IRenderingElement _renderingElement;

        public CullingDeactivator(ICullingController cullingController, 
            IRenderingElement renderingElement)
        {
            _cullingController = cullingController;
            _renderingElement = renderingElement;
        }

        void IRenderingElement.Render()
        {
            _cullingController.Off();
            _renderingElement.Render();
            _cullingController.On();
        }
    }
}
