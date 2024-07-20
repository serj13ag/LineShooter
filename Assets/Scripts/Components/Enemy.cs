using Infrastructure;
using Interfaces;
using Services;
using UnityEngine;

namespace Components
{
    public class Enemy : MonoBehaviour, ITimeTickable
    {
        private ITimeService _timeService;

        private float _enemySpeed;

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

        public void Init(int enemyMaxHealth, float enemySpeed)
        {
            _enemySpeed = enemySpeed;
        }

        public void TimeTick(float deltaTime)
        {
            var newPositionY = transform.position.y - _enemySpeed * deltaTime;

            transform.position = new Vector3(transform.position.x, newPositionY, transform.position.z);
        }
    }
}