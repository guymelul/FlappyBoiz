using FlappyBoiz.Scripts.Gameplay.Flapper;
using FlappyBoiz.Scripts.Gameplay.Managers;
using FlappyBoiz.Scripts.Gameplay.Obstacle;
using FlappyBoiz.Scripts.Gameplay.UI;
using FlappyBoiz.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

namespace FlappyBoiz.Scripts.Scopes
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private FlapperSettings flapperSettings;

        [SerializeField]
        private ObstacleSpawnerSettings obstacleSpawnerSettings;

        [SerializeField]
        private InputActionAsset inputActionAsset;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(flapperSettings);
            builder.RegisterInstance(obstacleSpawnerSettings);
            builder.RegisterInstance(inputActionAsset);

            builder.RegisterEntryPoint<GameplayManager>(Lifetime.Scoped).AsSelf();

            builder.RegisterEntryPoint<FlapperLogic>(Lifetime.Scoped).AsSelf();
            builder.RegisterEntryPoint<ObstacleSpawnerPresenter>(Lifetime.Scoped).AsSelf();
            builder.RegisterEntryPoint<GameplayUIPresenter>(Lifetime.Scoped).AsSelf();

            builder.RegisterComponentOnNewGameObject<GameInputManager>(
                Lifetime.Scoped, "GameInputManager"
            ).AsSelf();

            builder.RegisterComponentInHierarchy<FlapperView>();
            builder.RegisterComponentInHierarchy<ObstacleSpawnerView>();
            builder.RegisterComponentInHierarchy<GameplayUIView>();
            builder.RegisterComponentInHierarchy<UIDocument>();
        }
    }
}