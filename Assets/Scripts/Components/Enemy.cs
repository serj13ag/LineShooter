using System;
using Infrastructure;
using Interfaces;
using Services;
using UnityEngine;

namespace Components
{
    public class Enemy : MonoBehaviour, ITimeTickable
    {
        private ITimeService _timeService;

        private float _speed;

        private int _health;

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

        private void OnDisable()
        {
            _timeService.Unsubscribe(this);
        }

        public void Init(int maxHealth, float speed)
        {
            _speed = speed;

            _health = maxHealth;
        }

        public void TimeTick(float deltaTime)
        {
            var newPositionY = transform.position.y - _speed * deltaTime;

            transform.position = new Vector3(transform.position.x, newPositionY, transform.position.z);

            var crossedFinishLine = transform.position.y < Constants.PlayerMoveTopBorder;
            if (crossedFinishLine)
            {
                OnCrossedFinishLine?.Invoke(this, EventArgs.Empty);
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

            // TODO update UI flash red

            if (_health == 0)
            {
                OnDied?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}