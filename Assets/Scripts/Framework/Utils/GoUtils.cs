using UnityEngine;

public static class GoUtils {
  public static T EnsureComponent<T>(this GameObject go) where T : Component {
    if (!go.TryGetComponent<T>(out var component)) {
      component = go.AddComponent<T>();
    }
    return component;
  }
}
