using System;
using System.Collections.Generic;
using Services;

namespace Infrastructure
{
    public class ServiceLocator
    {
        private readonly Dictionary<Type, IService> _services = new Dictionary<Type, IService>();

        public static ServiceLocator Instance { get; private set; }

        public ServiceLocator()
        {
            Instance = this;
        }

        public void Register<TService>(TService service) where TService : IService
        {
            _services.Add(typeof(TService), service);
        }

        public TService Get<TService>() where TService : IService
        {
            return (TService)_services[typeof(TService)];
        }

        public void Remove<TService>() where TService : IService
        {
            var serviceToRemove = _services[typeof(TService)];

            if (serviceToRemove is IDisposable disposableService)
            {
                disposableService.Dispose();
            }

            _services.Remove(typeof(TService));
        }
    }
}