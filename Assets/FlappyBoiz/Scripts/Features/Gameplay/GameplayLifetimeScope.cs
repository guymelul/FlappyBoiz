using FlappyBoiz.Scripts.Core.Controllers;
using FlappyBoiz.Scripts.Features.Gameplay.Controllers;
using FlappyBoiz.Scripts.Features.Gameplay.Flapper;
using FlappyBoiz.Scripts.Features.Gameplay.Managers;
using FlappyBoiz.Scripts.Features.Gameplay.Obstacle;
using FlappyBoiz.Scripts.Features.Gameplay.ScriptableObjects;
using FlappyBoiz.Scripts.Features.Gameplay.UI.Screens;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Features.Gameplay
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private FlapperSettings flapperSettings;

        [SerializeField]
        private ObstacleSpawnerSettings obstacleSpawnerSettings;

        protected override void Configure(IContainerBuilder builder)
        {
            // Instances
            builder.RegisterInstance(flapperSettings);
            builder.RegisterInstance(obstacleSpawnerSettings);

            // Presenter/Logic
            builder.RegisterEntryPoint<FlapperLogic>(Lifetime.Scoped).AsSelf();
            builder.RegisterEntryPoint<ObstacleSpawnerLogic>(Lifetime.Scoped).AsSelf();

            // Controllers
            builder.RegisterController<GameplayController>();
            builder.RegisterController<GamePhaseController>();
            builder.RegisterController<ReadyPhaseController>();
            builder.RegisterController<GameOverPhaseController>();
            builder.RegisterController<GamePauseController>();

            // Managers
            builder.RegisterComponentInHierarchy<GameplayInputManager>();
            builder.RegisterEntryPoint<GameSessionManager>(Lifetime.Scoped).AsSelf();

            // View
            builder.RegisterComponentInHierarchy<FlapperPresenter>();
            builder.RegisterComponentInHierarchy<ObstacleSpawnerPresenter>();

            // UI Screens
            builder.RegisterComponentInHierarchy<ReadyScreen>();
            builder.RegisterComponentInHierarchy<HUDScreen>();
            builder.RegisterComponentInHierarchy<PauseDialog>();
            builder.RegisterComponentInHierarchy<GameOverScreen>();
        }
    }
}