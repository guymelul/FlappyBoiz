using FlappyBoiz.Scripts.Gameplay.Flapper;
using FlappyBoiz.Scripts.Gameplay.Managers;
using FlappyBoiz.Scripts.Gameplay.Obstacle;
using FlappyBoiz.Scripts.Gameplay.Presenters;
using FlappyBoiz.Scripts.Gameplay.ScriptableObjects;
using FlappyBoiz.Scripts.Gameplay.UI.Screens;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Gameplay.Scopes
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private FlapperSettings flapperSettings;

        [SerializeField] private ObstacleSpawnerSettings obstacleSpawnerSettings;

        protected override void Configure(IContainerBuilder builder)
        {
            // Instances
            builder.RegisterInstance(flapperSettings);
            builder.RegisterInstance(obstacleSpawnerSettings);

            // Managers
            builder.RegisterEntryPoint<GameplayManager>(Lifetime.Scoped).AsSelf();
            builder.RegisterComponentInHierarchy<GameplayInputManager>();
            builder.RegisterEntryPoint<GameSessionManager>(Lifetime.Scoped).AsSelf();

            // Presenter/Logic
            builder.RegisterEntryPoint<FlapperLogic>(Lifetime.Scoped).AsSelf();
            builder.RegisterEntryPoint<ObstacleSpawnerPresenter>(Lifetime.Scoped).AsSelf();

            // UI Controllers
            builder.RegisterEntryPoint<GameplayUIController>(Lifetime.Scoped).AsSelf();
            builder.RegisterEntryPoint<ReadyPresenter>(Lifetime.Scoped).AsSelf();
            builder.RegisterEntryPoint<PauseDialogPresenter>(Lifetime.Scoped).AsSelf();
            builder.RegisterEntryPoint<GameOverPresenter>(Lifetime.Scoped).AsSelf();
            builder.RegisterEntryPoint<HUDPresenter>(Lifetime.Scoped).AsSelf();

            // View
            builder.RegisterComponentInHierarchy<FlapperView>();
            builder.RegisterComponentInHierarchy<ObstacleSpawnerView>();

            // UI Screens
            builder.RegisterComponentInHierarchy<ReadyScreen>();
            builder.RegisterComponentInHierarchy<HUDScreen>();
            builder.RegisterComponentInHierarchy<PauseDialog>();
            builder.RegisterComponentInHierarchy<GameOverScreen>();

            builder.RegisterEntryPoint<LobbyMenuController>(Lifetime.Scoped).AsSelf();
            builder.RegisterEntryPoint<MainMenuPresenter>(Lifetime.Scoped).AsSelf();

            builder.RegisterComponentInHierarchy<MainMenuScreen>();
            builder.RegisterComponentInHierarchy<MenuLoadingScreen>();

            builder.RegisterEntryPoint<LobbyManager>(Lifetime.Scoped).AsSelf();

#if UNITY_EDITOR
            builder.RegisterBuildCallback(c => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));
#endif
        }
    }
}