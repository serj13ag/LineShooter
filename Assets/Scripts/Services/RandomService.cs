using System;

namespace Services
{
    public interface IRandomService : IService
    {
        int Range(int max);
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
    }
}