using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolTestMono : MonoBehaviour, IPoolable {
  public int num = 1;

  public void OnSpawn() {
    num += 1;
  }

  public void OnDelete() {
    num += 2;
  }
}
