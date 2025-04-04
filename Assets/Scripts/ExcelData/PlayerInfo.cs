using System.Collections.Generic;
public class PlayerInfo {
  /*唯一ID*/
  public int id;
  /*名称*/
  public string name;
  /*性别
true: 男
false:女*/
  public bool sex;
  /*攻击力*/
  public float ack;
}

public class PlayerInfoContainer {
  public Dictionary<int, PlayerInfo> config = new();
}
