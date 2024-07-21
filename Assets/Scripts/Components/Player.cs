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
        [SerializeField] private Transform _weaponTransform;
        [SerializeField] private PlayerAnimator _playerAnimator;

        private IInputService _inputService;
        private ITimeService _timeService;
        private IEnemyService _enemyService;
        private IGameFactory _gameFactory;
        private IGameplayLevelEndTracker _gameplayLevelEndTracker;

        private float _moveSpeed;
        private int _damage;
        private float _shootRange;
        private float _shootCooldownSeconds;
        private float _projectileSpeed;

        private float _timeTillShoot;
        private Vector3 _shootDirection;

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
            _damage = damage;
            _shootRange = shootRange;
            _shootCooldownSeconds = shootCooldownSeconds;
            _projectileSpeed = projectileSpeed;

            _playerAnimator.Init(shootCooldownSeconds);

            _playerMover = new PlayerMover(this, _moveSpeed, _inputService);

            HealthBlock = new HealthBlock(maxHealth);
            HealthBlock.OnHealthChanged += OnHealthChanged;

            _currentFacingDirection = Direction.Left;
        }

        public void TimeTick(float deltaTime)
        {
            _playerMover.TimeTick(deltaTime);
            ProcessShooting(deltaTime);
        }

        public void Shoot()
        {
            _gameFactory.SpawnProjectile(_weaponTransform.position, _weaponTransform.rotation, _currentFacingDirection,
                _shootDirection, _projectileSpeed, _damage);
        }

        public void OnReceiveInputX(float inputValue)
        {
            RotateModel(inputValue > 0 ? Direction.Right : Direction.Left);
        }

        private void OnHealthChanged(object sender, EventArgs e)
        {
            if (HealthBlock.Health == 0)
            {
                _gameplayLevelEndTracker.PlayerDied();
            }
        }

        private void ProcessShooting(float deltaTime)
        {
            if (_enemyService.TryGetNearestEnemy(transform.position, _shootRange, out var enemy))
            {
                if (_timeTillShoot <= 0)
                {
                    StartShoot(enemy.transform.position);
                }
            }

            if (_timeTillShoot > 0)
            {
                _timeTillShoot -= deltaTime;
            }
        }

        private void StartShoot(Vector3 targetPosition)
        {
            _shootDirection = (targetPosition - transform.position).normalized;
            _playerAnimator.PlayAttack();
            _timeTillShoot = _shootCooldownSeconds;
        }

        private void RotateModel(Direction direction)
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