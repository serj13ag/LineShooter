using System;
using System.Collections;
using Infrastructure;
using UnityEngine.SceneManagement;

namespace Services
{
    public interface ISceneLoader : IService
    {
        void LoadScene(string name, Action onLoaded = null, bool forceReload = false);
    }

    public class SceneLoader : ISceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void LoadScene(string name, Action onLoaded = null, bool forceReload = false)
        {
            if (!forceReload && SceneManager.GetActiveScene().name == name)
            {
                onLoaded?.Invoke();
                return;
            }

            _coroutineRunner.StartCoroutine(LoadSceneRoutine(name, onLoaded));
        }

        private IEnumerator LoadSceneRoutine(string name, Action onLoaded)
        {
            var loadSceneAsyncOperation = SceneManager.LoadSceneAsync(name);

            while (!loadSceneAsyncOperation.isDone)
            {
                yield return null;
            }

            onLoaded?.Invoke();
        }
    }
}