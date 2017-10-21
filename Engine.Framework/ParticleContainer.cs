using Engine.Contracts;
using Engine.Contracts.PlayerMotion;
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

            public double StartTime { set; get; }
        }

        private List<ParticleDefinition> _particles = new List<ParticleDefinition>();
        private readonly IGameTimeProvider _gameTimeProvider;
        private readonly ITranslator _worldTranslator;
        private readonly IMatrixManager _matrixManager;
        private readonly ITextureChanger _textureChanger;
        private readonly ITexture _texture;
        private readonly IPolygonRenderer _polygonRenderer;
        private readonly IEnumerable<Polygon> _polygons;
        private readonly IPlayerViewDirectionProvider _playerViewDirectionProvider;
        private readonly IWorldRotator _worldRotator;

        public ParticleContainer(IGameTimeProvider gameTimeProvider,
            ITranslator worldTranslator,
            IMatrixManager matrixManager,
            ITextureChanger textureChanger,
            ITexture texture,
            IPolygonRenderer polygonRenderer,
            IEnumerable<Polygon> polygons,
            IPlayerViewDirectionProvider playerViewDirectionProvider,
            IWorldRotator worldRotator)
        {
            _gameTimeProvider = gameTimeProvider;
            _worldTranslator = worldTranslator;
            _matrixManager = matrixManager;
            _textureChanger = textureChanger;
            _texture = texture;
            _polygonRenderer = polygonRenderer;
            _polygons = polygons;
            _playerViewDirectionProvider = playerViewDirectionProvider;
            _worldRotator = worldRotator;
        }

        void IParticleContainer.AddParticleExplosion(Position position)
        {
            _particles.Add(new ParticleDefinition { Position = position, StartTime = _gameTimeProvider.GetTotalTime() });
        }

        void IRenderingElement.Render()
        {
            foreach(ParticleDefinition particleDefinition in _particles)
            {
                _matrixManager.Store();
                _worldTranslator.Translate(particleDefinition.Position.X, particleDefinition.Position.Y, particleDefinition.Position.Z);
                _worldRotator.RotateY(270 - _playerViewDirectionProvider.GetViewDirection().DegreeXZ);
                _textureChanger.SetTexture(_texture.TextureId);
                _polygonRenderer.RenderPolygons(_polygons);
                _matrixManager.Reset();
            }
        }
    }
}
