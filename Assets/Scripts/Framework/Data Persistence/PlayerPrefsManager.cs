using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 使用 PlayerPrefs 来保存和读取数据
/// 目前支持的类型有 int, float, string, bool, IList, IDictionary, class
/// </summary>
public class PlayerPrefsManager : Singleton<PlayerPrefsManager> {
  /// <summary>
  /// 保存自定义数据类型
  /// </summary>
  /// <param name="key"></param>
  /// <param name="value"></param>
  public void SaveData(string key, object value) {
    if (value == null) {
      return;
    }
    Type type = value.GetType();
    foreach (var field in type.GetFields()) {
      var fieldValue = field.GetValue(value);
      SaveValue($"{key}_{type.Name}_{field.Name}", fieldValue);
    }
  }

  /// <summary>
  /// 保存基本数据类型
  /// </summary>
  /// <param name="key"></param>
  /// <param name="value"></param>
  public void SaveValue(string key, object value) {
    if (value is int v) {
      PlayerPrefs.SetInt(key, v);
    } else if (value is float v1) {
      PlayerPrefs.SetFloat(key, v1);
    } else if (value is string v2) {
      PlayerPrefs.SetString(key, v2);
    } else if (value is bool v3) {
      PlayerPrefs.SetInt(key, v3 ? 1 : 0);
    } else if (value is IList list) {
      SaveValue(key, list.Count);
      for (int i = 0; i < list.Count; i++) {
        SaveValue($"{key}_{i}", list[i]);
      }
    } else if (value is IDictionary dict) {
      SaveValue(key, dict.Count);
      int i = 0;
      foreach (DictionaryEntry entry in dict) {
        SaveValue($"{key}_key_{i}", entry.Key);
        SaveValue($"{key}_value_{entry.Key}", entry.Value);
        i++;
      }
    } else {
      this.SaveData(key, value);
    }
  }

  /// <summary>
  /// 读取自定义数据数据
  /// </summary>
  /// <param name="key"></param>
  /// <param name="type"></param>
  /// <returns></returns>
  public object LoadData(string key, Type type) {
    object value = Activator.CreateInstance(type);
    foreach (var field in type.GetFields()) {
      var fieldValue = LoadValue($"{key}_{type.Name}_{field.Name}", field.FieldType);
      field.SetValue(value, fieldValue);
    }
    return value;
  }

  /// <summary>
  /// 读取基本数据类型
  /// </summary>
  /// <param name="key"></param>
  /// <param name="type"></param>
  /// <returns></returns>
  public object LoadValue(string key, Type type) {
    if (type == typeof(int)) {
      return PlayerPrefs.GetInt(key);
    } else if (type == typeof(float)) {
      return PlayerPrefs.GetFloat(key);
    } else if (type == typeof(string)) {
      return PlayerPrefs.GetString(key);
    } else if (type == typeof(bool)) {
      return PlayerPrefs.GetInt(key) == 1;
    } else if (typeof(IList).IsAssignableFrom(type)) {
      int count = PlayerPrefs.GetInt(key);
      IList list = (IList)Activator.CreateInstance(type);
      for (int i = 0; i < count; i++) {
        var item = LoadValue($"{key}_{i}", type.GetGenericArguments()[0]);
        list.Add(item);
      }
      return list;
    } else if (typeof(IDictionary).IsAssignableFrom(type)) {
      int count = PlayerPrefs.GetInt(key);
      IDictionary dict = (IDictionary)Activator.CreateInstance(type);
      for (int i = 0; i < count; i++) {
        var keyItem = LoadValue($"{key}_key_{i}", type.GetGenericArguments()[0]);
        var valueItem = LoadValue($"{key}_value_{keyItem}", type.GetGenericArguments()[1]);
        dict.Add(keyItem, valueItem);
      }
      return dict;
    } else {
      return this.LoadData(key, type);
    }
  }
}
