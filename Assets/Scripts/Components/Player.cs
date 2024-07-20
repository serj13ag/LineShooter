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

        public event EventHandler<EventArgs> OnDied;

        private IInputService _inputService;
        private ITimeService _timeService;
        private IEnemyService _enemyService;

        private float _moveSpeed;
        private float _shootRange;
        private int _health;

        private Direction _currentFacingDirection;

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
            _shootRange = shootRange;
            _health = maxHealth;

            _currentFacingDirection = Direction.Left;
        }

        public void TimeTick(float deltaTime)
        {
            ProcessInput(deltaTime);

            if (_enemyService.TryGetNearestEnemy(transform.position, _shootRange, out var enemy))
            {
                Debug.DrawLine(transform.position, enemy.transform.position, Color.green); // TODO targeting
            }
        }

        public void TakeDamage(int damage)
        {
            if (damage > _health)
            {
                damage = _health;
            }

            if (damage <= 0)
            {
                return;
            }

            _health -= damage;

            Debug.LogWarning(_health); // TODO update UI

            if (_health == 0)
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