using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class XmlDataMgrTest {
  public struct Player {
    public int age;
    public string name;
    public float height;
    public bool sex;
    public SerializableDictionary<string, Item> dict;
    public List<Item> items;
  }
  public struct Item {
    public int id;
    public int count;
  }
  [Test]
  public void XmlDataMgrTestSimplePasses() {
    Debug.Log(Application.persistentDataPath);
    Player player = new() {
      age = 18,
      name = "Msstt",
      height = 175.5f,
      sex = true,
      items = new List<Item> {
        new() { id = 1, count = 2 },
        new() { id = 2, count = 3 }
      },
      dict = new SerializableDictionary<string, Item> {
        { "item1", new Item { id = 1, count = 2 } },
        { "item2", new Item { id = 2, count = 3 } }
      }
    };

    XmlDataMgr.Instance.SaveData("Msstt", player);
    var value = XmlDataMgr.Instance.LoadData("Msstt", typeof(Player));
    Assert.IsNotNull(value);
    Assert.IsInstanceOf<Player>(value);
    Assert.AreEqual(18, ((Player)value).age);
    Assert.AreEqual("Msstt", ((Player)value).name);
    Assert.AreEqual(175.5f, ((Player)value).height);
    Assert.AreEqual(true, ((Player)value).sex);

    Assert.AreEqual(2, ((Player)value).items.Count);
    Assert.AreEqual(1, ((Player)value).items[0].id);
    Assert.AreEqual(2, ((Player)value).items[0].count);
    Assert.AreEqual(2, ((Player)value).items[1].id);
    Assert.AreEqual(3, ((Player)value).items[1].count);

    Assert.AreEqual(2, ((Player)value).dict.Count);
    Assert.IsTrue(((Player)value).dict.ContainsKey("item1"));
    Assert.AreEqual(1, ((Player)value).dict["item1"].id);
    Assert.AreEqual(2, ((Player)value).dict["item1"].count);
    Assert.IsTrue(((Player)value).dict.ContainsKey("item2"));
    Assert.AreEqual(2, ((Player)value).dict["item2"].id);
    Assert.AreEqual(3, ((Player)value).dict["item2"].count);
  }
}
