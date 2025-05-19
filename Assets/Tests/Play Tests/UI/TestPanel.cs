using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestPanel : BasePanel {
  [GetComs("Title")]
  private TMP_Text text;

  [GetComs("ButtonList/Button")]
  private Button button;

  [GetComs("ButtonList/Button2")]
  private Button button2;

  public override void OnShow() {
    text.text = "TestPanel";
    button.onClick.AddListener(() => {
      Debug.Log("Button clicked");
    });
    button2.onClick.AddListener(() => {
      Close();
    });
  }
}
