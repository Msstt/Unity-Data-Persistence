using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EventSystemTest {
  [Test]
  public void EventSystemTestSimplePasses() {
    int? test = null;
    int? test2 = null;
    EventManager.Instance.AddListener(EventType.TestEvent, (param) => {
      test = param as int?;
    });
    EventManager.Instance.AddListener(EventType.TestEvent, (param) => {
      test2 = param as int?;
    });
    EventManager.Instance.SendEvent(EventType.TestEvent, 12);
    Assert.AreEqual(12, test);
    Assert.AreEqual(12, test2);
  }
}
