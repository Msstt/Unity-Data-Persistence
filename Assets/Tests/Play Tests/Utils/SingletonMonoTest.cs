using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestSingleton : MonoSingleton<TestSingleton> {
  public int Test(int x) {
    return x + 1;
  }
}

public class SingletonMonoTest {
  // A Test behaves as an ordinary method
  [Test]
  public void SingletonMonoTestSimplePasses() {
    Assert.AreEqual(2, TestSingleton.Instance.Test(1));
    Assert.NotNull(GameObject.Find("[Singleton] TestSingleton"));
  }
}
