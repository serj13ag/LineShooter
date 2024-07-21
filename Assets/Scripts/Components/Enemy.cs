using System;
using Infrastructure;
using Interfaces;
using Services;
using UnityEngine;

namespace Components
{
    public class Enemy : MonoBehaviour, ITimeTickable
    {
        [SerializeField] private SpriteFlashColorizer _spriteFlashColorizer;

        private ITimeService _timeService;

        private float _speed;

        private HealthBlock _healthBlock;

        public event EventHandler<EventArgs> OnCrossedFinishLine;
        public event EventHandler<EventArgs> OnDied;

        private void Awake()
        {
            _timeService = ServiceLocator.Instance.Get<ITimeService>();
        }

        private void OnEnable()
        {
            _timeService.Subscribe(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(Constants.PlayerProjectileTag))
            {
                return;
            }

            CollideWithPlayerProjectile(other);
        }

        public void Init(int maxHealth, float speed)
        {
            _speed = speed;

            _healthBlock = new HealthBlock(maxHealth);
            _healthBlock.OnHealthChanged += OnHealthChanged;
        }

        public void TimeTick(float deltaTime)
        {
            MoveDown(deltaTime);

            var crossedFinishLine = transform.position.y < Constants.PlayerMoveTopBorder;
            if (crossedFinishLine)
            {
                OnCrossedFinishLine?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnHealthChanged(object sender, EventArgs e)
        {
            _spriteFlashColorizer.Flash();

            if (_healthBlock.Health == 0)
            {
                OnDied?.Invoke(this, EventArgs.Empty);
            }
        }

        private void CollideWithPlayerProjectile(Collider2D other)
        {
            var playerProjectile = other.GetComponent<PlayerProjectile>();
            playerProjectile.OnCollideWithEnemy();
            _healthBlock.TakeDamage(playerProjectile.Damage);
        }

        private void MoveDown(float deltaTime)
        {
            var newPositionY = transform.position.y - _speed * deltaTime;
            transform.position = new Vector3(transform.position.x, newPositionY, transform.position.z);
        }

        private void OnDisable()
        {
            _timeService.Unsubscribe(this);
        }

        private void OnDestroy()
        {
            _healthBlock.OnHealthChanged -= OnHealthChanged;
        }
    }
}