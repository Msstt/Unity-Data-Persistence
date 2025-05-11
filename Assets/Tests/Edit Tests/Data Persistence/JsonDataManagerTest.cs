using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class JsonDataManagerTest {
  public class Player {
    public int age;
    public string name;
    public float height;
    public bool sex;
    public Dictionary<string, Item> dict;
    public List<Item> items;
  }
  [Serializable]
  public class Item {
    public int id;
    public int count;
  }
  [Test]
  public void JsonDataManagerTestJsonUtility() {
    Player player = new() {
      age = 18,
      name = "Msstt",
      height = 175.5f,
      sex = true,
      items = new List<Item> {
        new() { id = 1, count = 2 },
        new() { id = 2, count = 3 }
      },
      dict = new Dictionary<string, Item> {
        { "item1", new Item { id = 1, count = 2 } },
        { "item2", new Item { id = 2, count = 3 } }
      }
    };

    JsonDataManager.Instance.SaveData("Msstt", player, JsonType.JsonUtility);
    var value = JsonDataManager.Instance.LoadData<Player>("Msstt", JsonType.JsonUtility);
    Assert.IsNotNull(value);
    Assert.IsInstanceOf<Player>(value);
    Assert.AreEqual(18, value.age);
    Assert.AreEqual("Msstt", value.name);
    Assert.AreEqual(175.5f, value.height);
    Assert.AreEqual(true, value.sex);

    Assert.AreEqual(2, value.items.Count);
    Assert.AreEqual(1, value.items[0].id);
    Assert.AreEqual(2, value.items[0].count);
    Assert.AreEqual(2, value.items[1].id);
    Assert.AreEqual(3, value.items[1].count);
  }
  [Test]
  public void JsonDataManagerTestLitJson() {
    Player player = new() {
      age = 18,
      name = "Msstt",
      height = 175.5f,
      sex = true,
      items = new List<Item> {
        new() { id = 1, count = 2 },
        new() { id = 2, count = 3 }
      },
      dict = new Dictionary<string, Item> {
        { "item1", new Item { id = 1, count = 2 } },
        { "item2", new Item { id = 2, count = 3 } }
      }
    };

    JsonDataManager.Instance.SaveData("Msstt", player, JsonType.LitJson);
    var value = JsonDataManager.Instance.LoadData<Player>("Msstt", JsonType.LitJson);
    Assert.IsNotNull(value);
    Assert.IsInstanceOf<Player>(value);
    Assert.AreEqual(18, value.age);
    Assert.AreEqual("Msstt", value.name);
    Assert.AreEqual(175.5f, value.height);
    Assert.AreEqual(true, value.sex);

    Assert.AreEqual(2, value.items.Count);
    Assert.AreEqual(1, value.items[0].id);
    Assert.AreEqual(2, value.items[0].count);
    Assert.AreEqual(2, value.items[1].id);
    Assert.AreEqual(3, value.items[1].count);

    Assert.AreEqual(2, value.dict.Count);
    Assert.IsTrue(value.dict.ContainsKey("item1"));
    Assert.AreEqual(1, value.dict["item1"].id);
    Assert.AreEqual(2, value.dict["item1"].count);
    Assert.IsTrue(value.dict.ContainsKey("item2"));
    Assert.AreEqual(2, value.dict["item2"].id);
    Assert.AreEqual(3, value.dict["item2"].count);
  }
}
