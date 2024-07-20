using Enums;
using Infrastructure;
using Interfaces;
using Services;
using UnityEngine;

namespace Components
{
    public class PlayerProjectile : MonoBehaviour, ITimeTickable
    {
        [SerializeField] private float _rotationSpeed;

        private ITimeService _timeService;

        private Vector3 _direction;
        private Direction _rotationDirection;
        private float _speed;

        public int Damage { get; private set; }

        private void Awake()
        {
            _timeService = ServiceLocator.Instance.Get<ITimeService>();
        }

        private void OnEnable()
        {
            _timeService.Subscribe(this);
        }

        private void OnDisable()
        {
            _timeService.Unsubscribe(this);
        }

        public void Init(Vector3 direction, Direction rotationDirection, float speed, int damage)
        {
            _direction = direction;
            _rotationDirection = rotationDirection;
            _speed = speed;

            Damage = damage;
        }

        public void TimeTick(float deltaTime)
        {
            Move(deltaTime);
            Rotate(deltaTime);

            var inDestroyRange = Mathf.Abs(transform.position.x) > Constants.ProjectileDestroyRange
                                 || Mathf.Abs(transform.position.y) > Constants.ProjectileDestroyRange;
            if (inDestroyRange)
            {
                DestroyProjectile();
            }
        }

        private void Move(float deltaTime)
        {
            transform.position += _direction * (_speed * deltaTime);
        }

        private void Rotate(float deltaTime)
        {
            var rotationAmount = _rotationSpeed * deltaTime;
            transform.Rotate(0f, 0f, _rotationDirection == Direction.Left ? rotationAmount : -rotationAmount);
        }

        public void OnCollideWithEnemy()
        {
            DestroyProjectile();
        }

        private void DestroyProjectile()
        {
            Destroy(gameObject);
        }
    }
}