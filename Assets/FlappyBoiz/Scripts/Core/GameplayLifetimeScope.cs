using FlappyBoiz.Scripts.Data;
using FlappyBoiz.Scripts.Gameplay.Flapper;
using FlappyBoiz.Scripts.Managers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Core
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private FlapperSettings m_flapperSettings;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameplayManager>(Lifetime.Scoped).AsSelf();

            builder.RegisterEntryPoint<FlapperLogic>(Lifetime.Scoped).AsSelf();

            builder.RegisterComponentInHierarchy<FlapperView>();

            builder.RegisterInstance(m_flapperSettings);
        }
    }
}