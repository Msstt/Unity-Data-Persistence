using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
  void Start() {
    UIMgr.Instance.ShowPanel<TestPanel>("TestPanel", LayerType.Third);
  }
}
