using System.Collections.Generic;
public class PlayerInfo2 {
  /*名称*/
  public string name;
  /*性别*/
  public bool sex;
  /*攻击力*/
  public float ack;
}

public class PlayerInfo2Container {
  public Dictionary<string, PlayerInfo2> config = new();
}
