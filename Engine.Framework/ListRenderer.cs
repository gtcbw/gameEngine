using Engine.Contracts;
using System.Collections.Generic;

namespace Engine.Framework
{
    public sealed class ListRenderer : IRenderingElement
    {
        private readonly IEnumerable<IRenderingElement> renderingElements;

        public ListRenderer(IEnumerable<IRenderingElement> renderingElements)
        {
            this.renderingElements = renderingElements;
        }

        void IRenderingElement.Render()
        {
            foreach(IRenderingElement element in renderingElements)
            {
                element.Render();
            }
        }
    }
}
