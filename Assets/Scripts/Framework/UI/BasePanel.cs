using UnityEngine;

public class BasePanel : MonoBehaviour {
  public string prefabsPath;

  public virtual void OnShow() { }
  public virtual void OnHide() { }

  public void Close() {
    UIMgr.Instance.HidePanel(prefabsPath);
  }
}
