using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using Excel;
using System.Data;

public class ExcelExportTool {
  private static readonly string EXCEL_PATH = Application.dataPath + "/Config/Excel/";
  private static readonly string CODE_PATH = Application.dataPath + "/Scripts/ExcelData/";

  private static readonly int NAME_ROW_INDEX = 0;
  private static readonly int TYPE_ROW_INDEX = 1;
  private static readonly int KEY_ROW_INDEX = 2;
  private static readonly int NOTE_ROW_INDEX = 3;
  private static readonly int BEGIN_ROW_INDEX = 4;

  [MenuItem("Tools/ExportExcel")]
  public static void ExportToExcel() {
    if (!Directory.Exists(EXCEL_PATH)) {
      return;
    }
    if (!Directory.Exists(CODE_PATH)) {
      Directory.CreateDirectory(CODE_PATH);
    }
    DeleteFile(CODE_PATH);
    if (!Directory.Exists(BinaryDataManager.DATA_PATH)) {
      Directory.CreateDirectory(BinaryDataManager.DATA_PATH);
    }
    DeleteFile(BinaryDataManager.DATA_PATH);
    string[] files = Directory.GetFiles(EXCEL_PATH);
    foreach (string file in files) {
      FileInfo fileInfo = new(file);
      if (fileInfo.Extension != ".xlsx" && fileInfo.Extension != ".xls") {
        continue;
      }
      using (FileStream stream = fileInfo.Open(FileMode.Open, FileAccess.Read)) {
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet dataSet = excelReader.AsDataSet();
        foreach (DataTable table in dataSet.Tables) {
          GenerateCode(table);
          GenerateData(table);
        }
      }
    }
    AssetDatabase.Refresh();
    Debug.Log("Export completed");
  }

  private static void DeleteFile(string path) {
    string[] files = Directory.GetFiles(path);
    foreach (string file in files) {
      File.Delete(file);
    }
  }

  private static void GenerateCode(DataTable table) {
    string className = table.TableName;
    string filePath = CODE_PATH + className + ".cs";
    string content = "using System.Collections.Generic;\n";
    content += "public class " + className + " {\n";
    for (int i = 0; i < table.Columns.Count; i++) {
      string name = table.Rows[NAME_ROW_INDEX][i].ToString();
      string type = table.Rows[TYPE_ROW_INDEX][i].ToString();
      string note = table.Rows[NOTE_ROW_INDEX][i].ToString();
      content += "  /*" + note + "*/\n";
      content += "  public " + type + " " + name + ";\n";
    }
    content += "}\n\n";
    content += "public class " + className + "Container {\n";
    int keyIndex = GetKeyIndex(table);
    content += "  public Dictionary<" + table.Rows[TYPE_ROW_INDEX][keyIndex].ToString() + ", " + className + "> config = new();\n";
    content += "}\n";
    File.WriteAllText(filePath, content);
  }

  private static void GenerateData(DataTable table) {
    string className = table.TableName;
    string filePath = BinaryDataManager.DATA_PATH + className + ".bytes";
    using (FileStream stream = new(filePath, FileMode.Create, FileAccess.Write)) {
      // 行数
      WriteInt(stream, table.Rows.Count - BEGIN_ROW_INDEX);
      // 主键名
      WriteString(stream, table.Rows[NAME_ROW_INDEX][GetKeyIndex(table)].ToString());
      // 数据
      for (int i = BEGIN_ROW_INDEX; i < table.Rows.Count; i++) {
        for (int j = 0; j < table.Columns.Count; j++) {
          string name = table.Rows[NAME_ROW_INDEX][j].ToString();
          string type = table.Rows[TYPE_ROW_INDEX][j].ToString();
          string value = table.Rows[i][j].ToString();
          switch (type) {
            case "int":
              WriteInt(stream, int.Parse(value));
              break;
            case "float":
              WriteFloat(stream, float.Parse(value));
              break;
            case "bool":
              WriteBool(stream, bool.Parse(value));
              break;
            case "string":
              WriteString(stream, value);
              break;
            default:
              Debug.LogError("Unsupported type: " + type);
              break;
          }
        }
      }
    }
  }

  private static int GetKeyIndex(DataTable table) {
    for (int i = 0; i < table.Columns.Count; i++) {
      string columnName = table.Rows[KEY_ROW_INDEX][i].ToString();
      if (columnName == "*") {
        return i;
      }
    }
    return 0;
  }

  private static void WriteInt(FileStream stream, int value) {
    byte[] bytes = System.BitConverter.GetBytes(value);
    stream.Write(bytes, 0, bytes.Length);
  }
  private static void WriteFloat(FileStream stream, float value) {
    byte[] bytes = System.BitConverter.GetBytes(value);
    stream.Write(bytes, 0, bytes.Length);
  }
  private static void WriteBool(FileStream stream, bool value) {
    byte[] bytes = System.BitConverter.GetBytes(value);
    stream.Write(bytes, 0, bytes.Length);
  }
  private static void WriteString(FileStream stream, string value) {
    WriteInt(stream, value.Length);
    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(value);
    stream.Write(bytes, 0, bytes.Length);
  }
}
