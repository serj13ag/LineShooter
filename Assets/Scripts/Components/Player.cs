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

        private IInputService _inputService;
        private ITimeService _timeService;
        private IEnemyService _enemyService;

        private float _moveSpeed;
        private int _damage;
        private float _shootRange;
        private float _shootCooldownSeconds;

        private float _timeTillShoot;

        private Direction _currentFacingDirection;

        public HealthBlock HealthBlock { get; private set; }

        public event EventHandler<EventArgs> OnDied;

        private void Awake()
        {
            _inputService = ServiceLocator.Instance.Get<IInputService>();
            _timeService = ServiceLocator.Instance.Get<ITimeService>();
            _enemyService = ServiceLocator.Instance.Get<IEnemyService>();
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

            HealthBlock = new HealthBlock(maxHealth);
            HealthBlock.OnHealthChanged += OnHealthChanged;

            _currentFacingDirection = Direction.Left;
        }

        public void TimeTick(float deltaTime)
        {
            ProcessInput(deltaTime);
            ProcessShooting(deltaTime);
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
                    ShootAtEnemy(enemy);
                }

                Debug.DrawLine(transform.position, enemy.transform.position, Color.green); // TODO targeting
            }

            if (_timeTillShoot > 0)
            {
                _timeTillShoot -= deltaTime;
            }
        }

        private void ShootAtEnemy(Enemy nearestEnemy)
        {
            nearestEnemy.HealthBlock.TakeDamage(_damage);
            _timeTillShoot = _shootCooldownSeconds;
            Debug.LogWarning("Shoot!"); // TODO spawn projectile
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
    }
}