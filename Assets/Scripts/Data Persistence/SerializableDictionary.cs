using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable {
  public XmlSchema GetSchema() {
    return null;
  }

  public void ReadXml(XmlReader reader) {
    var keySerializer = new XmlSerializer(typeof(TKey));
    var valueSerializer = new XmlSerializer(typeof(TValue));
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement) {
      var key = (TKey)keySerializer.Deserialize(reader);
      var value = (TValue)valueSerializer.Deserialize(reader);
      this.Add(key, value);
    }
    reader.Read();
  }

  public void WriteXml(XmlWriter writer) {
    XmlSerializer keySerializer = new(typeof(TKey));
    XmlSerializer valueSerializer = new(typeof(TValue));
    foreach (var kvp in this) {
      keySerializer.Serialize(writer, kvp.Key);
      valueSerializer.Serialize(writer, kvp.Value);
    }
  }
}
