using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> {
  private static T instance;
  public static T Instance {
    get {
      if (instance == null) {
        GameObject singleton = new GameObject();
        singleton.name = "[Singleton] " + typeof(T).ToString();
        singleton.AddComponent<T>();
        DontDestroyOnLoad(singleton);
      }
      return instance;
    }
  }

  protected virtual void Awake() {
    if (instance == null) {
      instance = this as T;
    } else {
      Destroy(gameObject);
    }
  }
}
