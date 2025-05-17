using System.Collections.Generic;
using UnityEngine;

public interface IPoolable {
  public void OnSpawn();
  public void OnDelete();
}

public class ObjectPool : MonoSingleton<ObjectPool> {
  private readonly Dictionary<string, List<GameObject>> pool = new();

  public GameObject Spwan(string prefabsPath, Transform parent = null) {
    GameObject obj;
    if (pool.ContainsKey(prefabsPath) && pool[prefabsPath].Count > 0) {
      obj = pool[prefabsPath][0];
      pool[prefabsPath].RemoveAt(0);
    } else {
      obj = Object.Instantiate(Resources.Load<GameObject>(prefabsPath));
      obj.name = StringUtil.GetNameFromPath(prefabsPath);
    }
    obj.transform.SetParent(parent);
    if (obj.TryGetComponent<IPoolable>(out var poolable)) {
      poolable.OnSpawn();
    }
    obj.SetActive(true);
    return obj;
  }

  public void Delete(string prefabsPath, GameObject obj) {
    if (pool.ContainsKey(prefabsPath)) {
      pool[prefabsPath].Add(obj);
    } else {
      pool[prefabsPath] = new List<GameObject> { obj };
    }
    obj.transform.SetParent(transform);
    if (obj.TryGetComponent<IPoolable>(out var poolable)) {
      poolable.OnDelete();
    }
    obj.SetActive(false);
  }

  public void Clear() {
    foreach (var objs in pool.Values) {
      foreach (var obj in objs) {
        Destroy(obj);
      }
    }
    pool.Clear();
  }
}
