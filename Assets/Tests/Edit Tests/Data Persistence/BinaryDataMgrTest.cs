using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BinaryDataMgrTest {
  [Test]
  public void BinaryDataMgrIntKey() {
    PlayerInfoContainer table = BinaryDataMgr.Instance.GetTable(typeof(PlayerInfo)) as PlayerInfoContainer;
    Assert.IsNotNull(table);
    Assert.IsTrue(table.config.Count == 2);
    Assert.IsTrue(table.config.ContainsKey(1));
    Assert.IsTrue(table.config[1].id == 1);
    Assert.IsTrue(table.config[1].name == "Msstt");
    Assert.AreEqual(2.4f, table.config[1].ack, 0.0001f);
    Assert.IsTrue(table.config[1].sex == true);
    Assert.IsTrue(table.config.ContainsKey(3));
    Assert.IsTrue(table.config[3].id == 3);
    Assert.IsTrue(table.config[3].name == "qq123");
    Assert.AreEqual(1.1343, table.config[3].ack, 0.0001f);
    Assert.IsTrue(table.config[3].sex == false);
  }
  [Test]
  public void BinaryDataMgrStringKey() {
    PlayerInfo2Container table = BinaryDataMgr.Instance.GetTable(typeof(PlayerInfo2)) as PlayerInfo2Container;
    Assert.IsNotNull(table);
    Assert.IsTrue(table.config.Count == 2);
    Assert.IsTrue(table.config.ContainsKey("Msstt"));
    Assert.IsTrue(table.config["Msstt"].name == "Msstt");
    Assert.AreEqual(2.4f, table.config["Msstt"].ack, 0.0001f);
    Assert.IsTrue(table.config["Msstt"].sex == true);
    Assert.IsTrue(table.config.ContainsKey("qq123"));
    Assert.IsTrue(table.config["qq123"].name == "qq123");
    Assert.AreEqual(1.1343, table.config["qq123"].ack, 0.0001f);
    Assert.IsTrue(table.config["qq123"].sex == false);
  }
}
