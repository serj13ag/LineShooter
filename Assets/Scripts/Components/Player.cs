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
        private static readonly int AttackSpeedMultiplierAnimatorFloat = Animator.StringToHash("AttackSpeedMultiplier");
        private static readonly int AttackAnimatorTrigger = Animator.StringToHash("Attack");

        [SerializeField] private Transform _modelTransform;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _minAttackAnimationSpeed;

        private IInputService _inputService;
        private ITimeService _timeService;
        private IEnemyService _enemyService;
        private IGameFactory _gameFactory;

        private float _moveSpeed;
        private int _damage;
        private float _shootRange;
        private float _shootCooldownSeconds;
        private float _projectileSpeed;

        private float _timeTillShoot;
        private Vector3 _shootDirection;

        private Direction _currentFacingDirection;

        public HealthBlock HealthBlock { get; private set; }

        public event EventHandler<EventArgs> OnDied;

        private void Awake()
        {
            _inputService = ServiceLocator.Instance.Get<IInputService>();
            _timeService = ServiceLocator.Instance.Get<ITimeService>();
            _enemyService = ServiceLocator.Instance.Get<IEnemyService>();
            _gameFactory = ServiceLocator.Instance.Get<IGameFactory>();
        }

        private void OnEnable()
        {
            _timeService.Subscribe(this);
        }

        private void OnDisable()
        {
            _timeService.Unsubscribe(this);
        }

        public void Init(int maxHealth, int damage, float playerMoveSpeed, float shootRange, float shootCooldownSeconds,
            float projectileSpeed)
        {
            _moveSpeed = playerMoveSpeed;
            _damage = damage;
            _shootRange = shootRange;
            _shootCooldownSeconds = shootCooldownSeconds;
            _projectileSpeed = projectileSpeed;

            HealthBlock = new HealthBlock(maxHealth);
            HealthBlock.OnHealthChanged += OnHealthChanged;

            _currentFacingDirection = Direction.Left;

            _animator.SetFloat(AttackSpeedMultiplierAnimatorFloat,
                GetAttackSpeedAnimatorMultiplier(shootCooldownSeconds));
        }

        public void TimeTick(float deltaTime)
        {
            ProcessInput(deltaTime);
            ProcessShooting(deltaTime);
        }

        public void Shoot()
        {
            _gameFactory.SpawnProjectile(transform.position, _shootDirection, _projectileSpeed, _damage);
        }

        private void OnHealthChanged(object sender, EventArgs e)
        {
            if (HealthBlock.Health == 0)
            {
                OnDied?.Invoke(this, EventArgs.Empty);
            }
        }

        private void ProcessInput(float deltaTime)
        {
            var inputAxis = _inputService.Axis;

            if (inputAxis.magnitude == 0)
            {
                return;
            }

            var newPositionX = transform.position.x;
            var newPositionY = transform.position.y;

            if (inputAxis.x != 0)
            {
                newPositionX += inputAxis.x * _moveSpeed * deltaTime;

                RotateModel(inputAxis.x > 0 ? Direction.Right : Direction.Left);
            }

            if (inputAxis.y != 0)
            {
                newPositionY += inputAxis.y * _moveSpeed * deltaTime;
            }

            newPositionX = Mathf.Clamp(newPositionX, -Constants.PlayerMoveHorizontalBorder,
                Constants.PlayerMoveHorizontalBorder);
            newPositionY = Mathf.Clamp(newPositionY, Constants.PlayerMoveBottomBorder, Constants.PlayerMoveTopBorder);

            transform.position = new Vector3(newPositionX, newPositionY, 0f);
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
            _animator.SetTrigger(AttackAnimatorTrigger);
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

        private float GetAttackSpeedAnimatorMultiplier(float shootCooldownSeconds)
        {
            var multiplier = 1f / shootCooldownSeconds;
            return Mathf.Max(multiplier, _minAttackAnimationSpeed);
        }
    }
}