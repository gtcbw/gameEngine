using Engine.Contracts;
using Engine.Contracts.Input;
using Engine.Contracts.PlayerMotion;
using World.Model;

namespace Engine.Framework
{
    public sealed class RayTrigger
    {
        private readonly RayWithWorldTester _rayWithWorldTester;
        private readonly IPlayerViewRayProvider _playerViewRayProvider;
        private readonly IMouseButtonEventProvider _mouseButtonEventProvider;
        private readonly IParticleContainer _particleContainer;

        public RayTrigger(RayWithWorldTester rayWithWorldTester,
            IPlayerViewRayProvider playerViewRayProvider,
            IMouseButtonEventProvider mouseButtonEventProvider,
            IParticleContainer particleContainer)
        {
            _rayWithWorldTester = rayWithWorldTester;
            _playerViewRayProvider = playerViewRayProvider;
            _mouseButtonEventProvider = mouseButtonEventProvider;
            _particleContainer = particleContainer;
        }

        public void DoStuff()
        {
            if (!_mouseButtonEventProvider.GetMouseEvents().LeftButtonPressed)
                return;

            Position position = _rayWithWorldTester.Test(_playerViewRayProvider.GetPlayerViewRay());

            if (position != null)
            {
                _particleContainer.AddParticleExplosion(position);
            }
        }
    }
}
