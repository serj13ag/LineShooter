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

        public HealthBlock HealthBlock { get; private set; }

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

            HealthBlock = new HealthBlock(maxHealth);
            HealthBlock.OnHealthChanged += OnHealthChanged;
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

        private void OnHealthChanged(object sender, EventArgs e)
        {
            if (HealthBlock.Health == 0)
            {
                OnDied?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}