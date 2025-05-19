using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class GetComsAttribute : Attribute {
  public string Path { get; }
  public GetComsAttribute(string path) {
    Path = path;
  }
}

public static class UIGlue {
  public static void InitGlue<T>(T panel) where T : BasePanel {
    InitComsGlue(panel);
  }

  private static void InitComsGlue<T>(T panel) where T : BasePanel {
    var panelType = panel.GetType();
    var fields = panelType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
    foreach (var field in fields) {
      var attr = field.GetCustomAttribute<GetComsAttribute>();
      if (attr == null) {
        continue;
      }
      if (!typeof(UIBehaviour).IsAssignableFrom(field.FieldType)) {
        continue;
      }
      var targetTransform = panel.transform.Find(attr.Path);
      if (targetTransform == null) {
        Debug.LogError("ComsGlue not found: " + attr.Path);
        continue;
      }
      field.SetValue(panel, targetTransform.GetComponent(field.FieldType));
    }
  }
}
