using System.Collections.Generic;
using Interfaces;

namespace Services
{
    public interface ITimeService : IService
    {
        void Subscribe(ITimeTickable timeTickable);

        void UpdateTick(float deltaTime);
    }

    public class TimeService : ITimeService
    {
        private const float GameSpeed = 1f;

        private readonly List<ITimeTickable> _timeTickables = new List<ITimeTickable>();

        public void Subscribe(ITimeTickable timeTickable)
        {
            _timeTickables.Add(timeTickable);
        }

        public void UpdateTick(float deltaTime)
        {
            foreach (var timeTickable in _timeTickables.ToArray())
            {
                timeTickable.TimeTick(deltaTime * GameSpeed);
            }
        }
    }
}