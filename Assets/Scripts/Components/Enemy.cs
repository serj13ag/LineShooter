using Interfaces;
using Services;
using UnityEngine;

namespace Components
{
    public class Enemy : MonoBehaviour, ITimeTickable
    {
        private ITimeService _timeService;

        private float _enemySpeed;

        public void Init(ITimeService timeService, int enemyMaxHealth, float enemySpeed)
        {
            _enemySpeed = enemySpeed;
            _timeService = timeService;

            _timeService.Subscribe(this); // TODO unsub
        }

        public void TimeTick(float deltaTime)
        {
            var newPositionY = transform.position.y - _enemySpeed * deltaTime;

            transform.position = new Vector3(transform.position.x, newPositionY, transform.position.z);
        }
    }
}