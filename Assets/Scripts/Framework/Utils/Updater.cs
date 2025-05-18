using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class Updater : MonoSingleton<Updater> {
  private readonly Dictionary<int, UnityAction> actions = new();

  public void Register(UnityAction action, int priority = 0) {
    if (actions.ContainsKey(priority)) {
      actions[priority] += action;
    } else {
      actions.Add(priority, action);
    }
  }

  public void Unregister(UnityAction action, int priority = 0) {
    if (actions.ContainsKey(priority)) {
      actions[priority] -= action;
      if (actions[priority] == null) {
        actions.Remove(priority);
      }
    }
  }

  void Update() {
    foreach (var action in actions.OrderBy(item => item.Key)) {
      action.Value?.Invoke();
    }
  }
}
