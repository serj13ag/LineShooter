using System;

namespace Services
{
    public interface IRandomService : IService
    {
        int Range(int max);
        float Range(float min, float max);
    }

    public class RandomService : IRandomService
    {
        private readonly Random _random;

        public RandomService()
        {
            _random = new Random();
        }

        public int Range(int max)
        {
            return _random.Next(0, max);
        }

        public float Range(float min, float max)
        {
            double range = max - min;
            var sample = _random.NextDouble();
            var scaled = sample * range + min;
            return (float)scaled;
        }
    }
}