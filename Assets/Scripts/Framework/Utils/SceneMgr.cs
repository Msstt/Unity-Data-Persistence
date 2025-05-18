using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneMgr : Singleton<SceneMgr> {
  public void LoadScene(string sceneName, UnityAction onLoaded = null) {
    SceneManager.LoadScene(sceneName);
    onLoaded?.Invoke();
  }

  public void LoadSceneAsync(string sceneName, UnityAction onLoaded = null, UnityAction<float> onProgress = null) {
    Updater.Instance.StartCoroutine(LoadSceneAsyncCoroutine(sceneName, onLoaded, onProgress));
  }

  IEnumerator LoadSceneAsyncCoroutine(string sceneName, UnityAction onLoaded, UnityAction<float> onProgress) {
    AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
    while (!asyncOperation.isDone) {
      onProgress?.Invoke(asyncOperation.progress);
      yield return null;
    }
    onLoaded?.Invoke();
  }
}
