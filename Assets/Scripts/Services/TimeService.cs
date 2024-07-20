using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Services
{
    public interface ITimeService : IService
    {
        void Subscribe(ITimeTickable timeTickable);
        void Unsubscribe(ITimeTickable timeTickable);

        void SetGameSpeed(float gameSpeed);

        void UpdateTick(float deltaTime);
    }

    public class TimeService : ITimeService
    {
        private readonly List<ITimeTickable> _timeTickables = new List<ITimeTickable>();
        private float _gameSpeed = 1f;

        public void Subscribe(ITimeTickable timeTickable)
        {
            _timeTickables.Add(timeTickable);
        }

        public void Unsubscribe(ITimeTickable timeTickable)
        {
            _timeTickables.Remove(timeTickable);
        }

        public void SetGameSpeed(float gameSpeed)
        {
            gameSpeed = Mathf.Max(gameSpeed, 0f);

            _gameSpeed = gameSpeed;
        }

        public void UpdateTick(float deltaTime)
        {
            foreach (var timeTickable in _timeTickables.ToArray())
            {
                timeTickable.TimeTick(deltaTime * _gameSpeed);
            }
        }
    }
}