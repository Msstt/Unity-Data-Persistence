using System.IO;
using LitJson;
using UnityEngine;

public enum JsonType {
  JsonUtility,
  LitJson
}

/// <summary>
/// 使用 Json 来保存和读取数据
/// 注意：Json 文件需使用 UTF-8 编码
/// JsonUtility:
/// 1. 自定义类需要添加 [System.Serializable] 特性
/// 2. 序列化私有变量需要添加 [SerializeField] 特性
/// LitJson:
/// 1. 支持序列化 键为 string 的 Dictionary
/// 2. 支持序列化 Json 数组
/// 3. 自定义类必须有无参构造函数
/// </summary>
public class JsonDataManager {
  private static readonly JsonDataManager instance = new();
  public static JsonDataManager Instance {
    get => instance;
  }
  private JsonDataManager() { }

  /// <summary>
  /// 保存数据
  /// </summary>
  /// <param name="filePath"></param>
  /// <param name="data"></param>
  /// <param name="type"></param>
  public void SaveData(string filePath, object data, JsonType type = JsonType.LitJson) {
    string path = Application.persistentDataPath + "/" + filePath + ".json";
    string json = "";
    switch (type) {
      case JsonType.JsonUtility:
        json = JsonUtility.ToJson(data);
        break;
      case JsonType.LitJson:
        json = JsonMapper.ToJson(data);
        break;
    }
    File.WriteAllText(path, json);
  }

  /// <summary>
  /// 加载数据
  /// 如果文件不存在，则会返回 null
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="filePath"></param>
  /// <param name="type"></param>
  /// <returns></returns>
  public T LoadData<T>(string filePath, JsonType type = JsonType.LitJson) where T : class, new() {
    string path = Application.persistentDataPath + "/" + filePath + ".json";
    if (!File.Exists(path)) {
      path = Application.streamingAssetsPath + "/" + filePath + ".json";
      if (!File.Exists(path)) {
        return null;
      }
    }
    string json = File.ReadAllText(path);
    T data = new();
    switch (type) {
      case JsonType.JsonUtility:
        data = JsonUtility.FromJson<T>(json);
        break;
      case JsonType.LitJson:
        data = JsonMapper.ToObject<T>(json);
        break;
    }
    return data;
  }
}
