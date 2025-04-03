using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Codice.Client.Common;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

/// <summary>
/// 使用 Xml 来保存和读取数据
/// 注意：
/// 1. 只能序列化 public 的字段和属性
/// 2. 存储字典使用 SerialiableDictionary
/// 3. 请勿在类型中初始化字段
/// </summary>
public class XmlDataManager {
  private static readonly XmlDataManager instance = new();
  public static XmlDataManager Instance {
    get => instance;
  }
  private XmlDataManager() { }

  /// <summary>
  /// 保存数据
  /// </summary>
  /// <param name="filePath"></param>
  /// <param name="data"></param>
  public void SaveData(string filePath, object data) {
    string path = Application.persistentDataPath + "/" + filePath + ".xml";
    var serializer = new XmlSerializer(data.GetType());
    using (var stream = new StreamWriter(path)) {
      serializer.Serialize(stream, data);
    }
  }

  /// <summary>
  /// 加载数据
  /// 如果文件不存在，则会返回 null
  /// </summary>
  /// <param name="filePath"></param>
  /// <param name="type"></param>
  /// <returns></returns>
  public object LoadData(string filePath, Type type) {
    string path = Application.persistentDataPath + "/" + filePath + ".xml";
    if (!File.Exists(path)) {
      path = Application.streamingAssetsPath + "/" + filePath + ".xml";
      if (!File.Exists(path)) {
        return null;
      }
    }
    var serializer = new XmlSerializer(type);
    using (var stream = new StreamReader(path)) {
      return serializer.Deserialize(stream);
    }
  }
}
