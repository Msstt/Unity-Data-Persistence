using System.Collections.Generic;
using UnityEngine;

public enum LayerType {
  First,
  Second,
  Third,
}

public class UIMgr : MonoSingleton<UIMgr> {
  private Transform canvas;
  private Transform firstCanvas;
  private Transform secondCanvas;
  private Transform thirdCanvas;
  private readonly Dictionary<string, BasePanel> panelDic = new();

  private new void Awake() {
    base.Awake();
    canvas = ResourcesMgr.Instance.Load<GameObject>("Prefabs/UI/Framework/UIRoot").transform;
    canvas.SetParent(transform);
    firstCanvas = canvas.Find("Canvas/First");
    secondCanvas = canvas.Find("Canvas/Second");
    thirdCanvas = canvas.Find("Canvas/Third");
  }

  public Transform GetCanvasTransform(LayerType layer) {
    switch (layer) {
      case LayerType.First:
        return firstCanvas;
      case LayerType.Second:
        return secondCanvas;
      case LayerType.Third:
        return thirdCanvas;
    }
    return secondCanvas;
  }

  private void SetPanelTransform(Transform panel, LayerType layer) {
    panel.SetParent(GetCanvasTransform(layer));

    (panel as RectTransform).anchorMin = Vector2.zero;
    (panel as RectTransform).anchorMax = Vector2.one;
    (panel as RectTransform).offsetMin = Vector2.zero;
    (panel as RectTransform).offsetMax = Vector2.zero;
  }

  public void ShowPanel<T>(string path, LayerType layer = LayerType.Second) where T : BasePanel {
    if (panelDic.ContainsKey(path)) {
      return;
    }
    ResourcesMgr.Instance.LoadAsync<GameObject>("Prefabs/UI/Panel/" + path, (obj) => {
      if (obj == null) {
        Debug.LogError("Load panel failed: " + path);
        return;
      }
      SetPanelTransform(obj.transform, layer);
      T panel = obj.EnsureComponent<T>();
      panelDic[path] = panel;
      panel.prefabsPath = path;
      UIGlue.InitGlue(panel);
      panel.OnShow();
    });
  }

  public void HidePanel(string path) {
    if (!panelDic.ContainsKey(path)) {
      return;
    }
    panelDic[path].OnHide();
    Destroy(panelDic[path].gameObject);
    panelDic.Remove(path);
  }

  public T GetPanel<T>(string path) where T : BasePanel {
    if (panelDic.ContainsKey(path)) {
      return panelDic[path] as T;
    }
    return null;
  }

  public void Clear() {
    foreach (var panel in panelDic.Values) {
      panel.OnHide();
      Destroy(panel.gameObject);
    }
    panelDic.Clear();
  }
}
