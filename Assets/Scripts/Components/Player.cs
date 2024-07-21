using System;
using Enums;
using Infrastructure;
using Interfaces;
using Services;
using UnityEngine;

namespace Components
{
    public class Player : MonoBehaviour, ITimeTickable
    {
        [SerializeField] private Transform _modelTransform;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PlayerShootBlock _playerShootBlock;

        private IInputService _inputService;
        private ITimeService _timeService;
        private IEnemyService _enemyService;
        private IGameFactory _gameFactory;
        private IGameplayLevelEndTracker _gameplayLevelEndTracker;

        private float _moveSpeed;

        private Direction _currentFacingDirection;

        private PlayerMover _playerMover;

        public HealthBlock HealthBlock { get; private set; }

        private void Awake()
        {
            _inputService = ServiceLocator.Instance.Get<IInputService>();
            _timeService = ServiceLocator.Instance.Get<ITimeService>();
            _enemyService = ServiceLocator.Instance.Get<IEnemyService>();
            _gameFactory = ServiceLocator.Instance.Get<IGameFactory>();
            _gameplayLevelEndTracker = ServiceLocator.Instance.Get<IGameplayLevelEndTracker>();
        }

        private void OnEnable()
        {
            _timeService.Subscribe(this);
        }

        public void Init(int maxHealth, int damage, float playerMoveSpeed, float shootRange, float shootCooldownSeconds,
            float projectileSpeed)
        {
            _moveSpeed = playerMoveSpeed;

            _playerAnimator.Init(shootCooldownSeconds);

            _playerShootBlock.Init(this, damage, shootRange, shootCooldownSeconds, projectileSpeed, _playerAnimator,
                _gameFactory, _enemyService);

            _playerMover = new PlayerMover(this, _moveSpeed, _inputService);

            HealthBlock = new HealthBlock(maxHealth);
            HealthBlock.OnHealthChanged += OnHealthChanged;

            _currentFacingDirection = Direction.Left;
        }

        public void TimeTick(float deltaTime)
        {
            _playerMover.TimeTick(deltaTime);
            _playerShootBlock.TimeTick(deltaTime);
        }

        public void Shoot()
        {
            _playerShootBlock.Shoot(_currentFacingDirection);
        }

        public void OnReceiveInputX(float inputValue)
        {
            TryRotateModel(inputValue > 0 ? Direction.Right : Direction.Left);
        }

        private void OnHealthChanged(object sender, EventArgs e)
        {
            if (HealthBlock.Health == 0)
            {
                _gameplayLevelEndTracker.PlayerDied();
            }
        }

        private void TryRotateModel(Direction direction)
        {
            if (_currentFacingDirection == direction)
            {
                return;
            }

            _currentFacingDirection = direction;

            _modelTransform.localScale = new Vector3(-_modelTransform.localScale.x, _modelTransform.localScale.y,
                _modelTransform.localScale.z);
        }

        private void OnDisable()
        {
            _timeService.Unsubscribe(this);
        }

        private void OnDestroy()
        {
            HealthBlock.OnHealthChanged -= OnHealthChanged;
        }
    }
}