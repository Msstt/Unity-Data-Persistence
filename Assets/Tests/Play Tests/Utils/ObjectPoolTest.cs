using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class ObjectPoolTest {
  [Test]
  public void ObjectPoolTestSimplePasses() {
    GameObject parent = new();
    ObjectPool.Instance.Spwan("Prefabs/TestPrefabs", parent.transform);
    GameObject gameObject = parent.transform.Find("TestPrefabs").gameObject;
    Assert.AreEqual(true, gameObject.activeSelf);
    ObjectPoolTestMono com = gameObject.GetComponent<ObjectPoolTestMono>();
    Assert.AreEqual(2, com.num);
    ObjectPool.Instance.Delete("Prefabs/TestPrefabs", gameObject);
    Assert.AreEqual(4, com.num);
    Assert.AreEqual(false, GameObject.Find("[Singleton] ObjectPool").transform.Find("TestPrefabs").gameObject.activeSelf);

    GameObject gameObject2 = ObjectPool.Instance.Spwan("Prefabs/TestPrefabs", parent.transform);
    Assert.AreEqual(gameObject2, gameObject);
    ObjectPool.Instance.Delete("Prefabs/TestPrefabs", gameObject2);
    ObjectPool.Instance.Clear();
    Assert.AreEqual(false, GameObject.Find("[Singleton] ObjectPool").transform.Find("TestPrefabs").gameObject.activeSelf);
  }
}
