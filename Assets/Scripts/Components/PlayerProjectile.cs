using Infrastructure;
using Interfaces;
using Services;
using UnityEngine;

namespace Components
{
    public class PlayerProjectile : MonoBehaviour, ITimeTickable
    {
        private const float HitDistanceTolerance = 0.1f;

        private ITimeService _timeService;

        private Enemy _targetEnemy;
        private float _speed;
        private int _damage;

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

        public void Init(Enemy targetEnemy, float speed, int damage)
        {
            _targetEnemy = targetEnemy;
            _speed = speed;
            _damage = damage;
        }

        public void TimeTick(float deltaTime)
        {
            if (_targetEnemy == null)
            {
                DestroyProjectile();
                return;
            }

            var closeToEnemy = Vector3.Distance(transform.position, _targetEnemy.transform.position) < HitDistanceTolerance;
            if (closeToEnemy)
            {
                _targetEnemy.HealthBlock.TakeDamage(_damage);
                DestroyProjectile();
                return;
            }

            MoveTowardsEnemy(deltaTime);
        }

        private void MoveTowardsEnemy(float deltaTime)
        {
            var directionToTarget = _targetEnemy.transform.position - transform.position;
            directionToTarget.Normalize();

            transform.position += directionToTarget * (_speed * deltaTime);
        }

        private void DestroyProjectile()
        {
            Destroy(gameObject);
        }
    }
}