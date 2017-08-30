using Engine.Contracts;
using Graphics.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World.Model;

namespace Engine.Framework
{
    public sealed class ParticleContainer : IParticleContainer, IRenderingElement
    {
        private class ParticleDefinition
        {
            public Position Position { set; get; }

            public Material Material { set; get; }

            public double StartTime { set; get; }
        }

        private List<ParticleDefinition> _particles = new List<ParticleDefinition>();
        private readonly IGameTimeProvider _gameTimeProvider;
        private readonly IWorldTranslator _worldTranslator;
        private readonly ITextureChanger _textureChanger;
        private readonly ITexture _texture;
        private readonly IPolygonRenderer _polygonRenderer;
        private readonly IEnumerable<Polygon> _polygons;

        public ParticleContainer(IGameTimeProvider gameTimeProvider,
            IWorldTranslator worldTranslator,
            ITextureChanger textureChanger,
            ITexture texture,
            IPolygonRenderer polygonRenderer,
            IEnumerable<Polygon> polygons)
        {
            _gameTimeProvider = gameTimeProvider;
            _worldTranslator = worldTranslator;
            _textureChanger = textureChanger;
            _texture = texture;
            _polygonRenderer = polygonRenderer;
            _polygons = polygons;
        }

        void IParticleContainer.AddParticleExplosion(Position position, Material material)
        {
            _particles.Add(new ParticleDefinition { Position = position, Material = material, StartTime = _gameTimeProvider.GetTotalTime() });
        }

        void IRenderingElement.Render()
        {
            foreach(ParticleDefinition particleDefinition in _particles)
            {
                _worldTranslator.Store();
                _worldTranslator.TranslateWorld(particleDefinition.Position.X, particleDefinition.Position.Y, particleDefinition.Position.Z);
                _textureChanger.SetTexture(_texture.TextureId);
                _polygonRenderer.RenderPolygons(_polygons);
                _worldTranslator.Reset();
            }
        }
    }
}
