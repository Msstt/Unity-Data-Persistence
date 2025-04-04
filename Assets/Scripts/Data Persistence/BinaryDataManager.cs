using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 使用 Excel 和二进制文件来保存和读取数据
/// 步骤：
/// 1. 编写 Excel 文件
///    第一行：变量名
///    第二行：变量类型
///    第三行：* 表示主键
///    第四行：变量描述
///    第五行-：数据
/// 2. 使用 Tool/ExcelExport 导出 cs, bytes 文件
/// 3. 使用 BinaryDataManager.LoadData<T>() 加载数据
/// </summary>
public class BinaryDataManager {
  public static readonly string DATA_PATH = Application.dataPath + "/StreamingAssets/BinaryData/";

  private static readonly BinaryDataManager instance = new();
  public static BinaryDataManager Instance {
    get => instance;
  }
  private BinaryDataManager() {
    if (!Directory.Exists(DATA_PATH)) {
      Directory.CreateDirectory(DATA_PATH);
    }
  }

  /// <summary>
  /// 加载数据
  /// 如果文件不存在，则会返回 null
  /// </summary>
  /// <param name="dataType">数据类型</param>
  /// <returns>dataType 对应的容器类</returns>
  public object GetTable(Type dataType) {
    Type containerType = Type.GetType(dataType.Name + "Container");
    if (containerType == null) {
      Debug.LogError("Container type not found for " + dataType.Name);
      return null;
    }
    string filePath = DATA_PATH + dataType.Name + ".bytes";
    if (!File.Exists(filePath)) {
      Debug.LogError("File not found: " + filePath);
      return null;
    }
    object result = Activator.CreateInstance(containerType);
    using (FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read)) {
      int index = 0;
      int rowCount = ReadInt(fileStream, ref index);
      string keyName = ReadString(fileStream, ref index);
      for (int i = 0; i < rowCount; i++) {
        object data = Activator.CreateInstance(dataType);
        foreach (var field in dataType.GetFields()) {
          if (field.FieldType == typeof(int)) {
            field.SetValue(data, ReadInt(fileStream, ref index));
          } else if (field.FieldType == typeof(float)) {
            field.SetValue(data, ReadFloat(fileStream, ref index));
          } else if (field.FieldType == typeof(string)) {
            field.SetValue(data, ReadString(fileStream, ref index));
          } else if (field.FieldType == typeof(bool)) {
            field.SetValue(data, ReadBool(fileStream, ref index));
          }
        }
        object container = result.GetType().GetField("config").GetValue(result);
        container.GetType().GetMethod("Add").Invoke(container, new object[] { dataType.GetField(keyName).GetValue(data), data });
      }
    }
    return result;
  }

  private int ReadInt(FileStream fileStream, ref int index) {
    byte[] buffer = new byte[4];
    index += fileStream.Read(buffer, 0, 4);
    return BitConverter.ToInt32(buffer, 0);
  }
  private float ReadFloat(FileStream fileStream, ref int index) {
    byte[] buffer = new byte[4];
    index += fileStream.Read(buffer, 0, 4);
    return BitConverter.ToSingle(buffer, 0);
  }
  private string ReadString(FileStream fileStream, ref int index) {
    int length = ReadInt(fileStream, ref index);
    byte[] buffer = new byte[length];
    index += fileStream.Read(buffer, 0, length);
    return System.Text.Encoding.UTF8.GetString(buffer);
  }
  private bool ReadBool(FileStream fileStream, ref int index) {
    byte[] buffer = new byte[1];
    index += fileStream.Read(buffer, 0, 1);
    return BitConverter.ToBoolean(buffer, 0);
  }
}
