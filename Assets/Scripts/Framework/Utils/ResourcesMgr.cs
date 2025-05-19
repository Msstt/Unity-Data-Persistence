using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ResourcesMgr : Singleton<ResourcesMgr> {
  public T Load<T>(string path) where T : Object {
    T resource = Resources.Load<T>(path);
    if (resource is GameObject) {
      resource = Object.Instantiate(resource);
      resource.name = StringUtils.GetNameFromPath(path);
    }
    return resource;
  }

  public void LoadAsync<T>(string path, UnityAction<T> onLoaded) where T : Object {
    Updater.Instance.StartCoroutine(LoadAsyncCoroutine<T>(path, onLoaded));
  }

  private IEnumerator LoadAsyncCoroutine<T>(string path, UnityAction<T> onLoaded) where T : Object {
    ResourceRequest resourceRequest = Resources.LoadAsync<T>(path);
    yield return resourceRequest;
    if (resourceRequest.asset is GameObject) {
      var resource = Object.Instantiate(resourceRequest.asset);
      resource.name = StringUtils.GetNameFromPath(path);
      onLoaded?.Invoke(resource as T);
    } else {
      onLoaded?.Invoke(resourceRequest.asset as T);
    }
  }
}
