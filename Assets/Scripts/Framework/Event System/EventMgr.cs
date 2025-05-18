using System.Collections.Generic;
using UnityEngine.Events;

public class EventMgr : Singleton<EventMgr> {
  private readonly Dictionary<EventType, UnityAction<object>> eventDic = new();

  public void AddListener(EventType type, UnityAction<object> action) {
    if (eventDic.ContainsKey(type)) {
      eventDic[type] += action;
    } else {
      eventDic[type] = action;
    }
  }

  public void RemoveListener(EventType type, UnityAction<object> action) {
    if (eventDic.ContainsKey(type)) {
      eventDic[type] -= action;
      if (eventDic[type] == null) {
        eventDic.Remove(type);
      }
    }
  }

  public void SendEvent(EventType type, object param = null) {
    if (eventDic.ContainsKey(type)) {
      eventDic[type].Invoke(param);
    }
  }
}
