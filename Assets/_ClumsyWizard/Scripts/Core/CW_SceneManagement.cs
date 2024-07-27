using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace ClumsyWizard.Core
{
    public interface ISceneLoadEvent
    {
        public void OnSceneLoadTriggered(Action onComplete);
        public void OnSceneLoaded();
    }

    public abstract class CW_SceneManagement : CW_Persistant<CW_SceneManagement>, ISceneLoadEvent
    {
        public string CurrentLevel => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        public bool IsMenuScene => CurrentLevel.Contains("Menu");
        public abstract bool IsGameLevel { get; }
        private bool loading;

        //Level Load Cleanup
        private ISceneLoadEvent[] sceneLoadEvents;
        private int currentSceneLoadEventIndex;
        private string sceneToLoad = string.Empty;

        protected virtual void Start()
        {

        }

        public virtual void Load(string sceneName)
        {
            if (loading)
                return;

            loading = true;
            StartCoroutine(LoadScene(sceneName));
        }

        protected abstract void OnLoadTriggered();

        private IEnumerator LoadScene(string sceneName)
        {
            if (sceneName == "")
            {
                Debug.Log("Empty Scene Name");
            }
            else
            {
                sceneToLoad = sceneName;
                OnLoadTriggered();
                yield return new WaitForSecondsRealtime(1.3f);
                CleanupScene();
            }
        }

        //Scene load cleanup Callbacks
        private void CleanupScene()
        {
            sceneLoadEvents = FindObjectsOfType<MonoBehaviour>().OfType<ISceneLoadEvent>().ToArray();
            currentSceneLoadEventIndex = 0;

            sceneLoadEvents[0].OnSceneLoadTriggered(OnNextObjectCleanup);
        }
        private void OnNextObjectCleanup()
        {
            currentSceneLoadEventIndex++;
            if (currentSceneLoadEventIndex >= sceneLoadEvents.Length)
            {
                StartCoroutine(FinishLoadingScene());
            }
            else
            {
                sceneLoadEvents[currentSceneLoadEventIndex].OnSceneLoadTriggered(OnNextObjectCleanup);
            }
        }

        private IEnumerator FinishLoadingScene()
        {
            yield return null;

            AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneToLoad);
            operation.allowSceneActivation = false;

            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingProgress(progress);

            while (!operation.isDone)
            {
                if (operation.progress >= 0.9f)
                {
                    yield return new WaitForEndOfFrame();

                    operation.allowSceneActivation = true;
                    OnFinishLoadingScene();

                    sceneLoadEvents = FindObjectsOfType<MonoBehaviour>().OfType<ISceneLoadEvent>().ToArray();
                    foreach (ISceneLoadEvent sceneLoadEvent in sceneLoadEvents)
                    {
                        sceneLoadEvent.OnSceneLoaded();
                    }

                    loading = false;
                }

                yield return null;
            }
        }
        protected abstract void LoadingProgress(float progress);
        protected abstract void OnFinishLoadingScene();

        public void OnSceneLoadTriggered(Action onComplete)
        {
            throw new NotImplementedException();
        }

        public void OnSceneLoaded()
        {
            throw new NotImplementedException();
        }
    }
}