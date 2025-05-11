public class Singleton<T> where T : Singleton<T>, new() {
  private static readonly T instance = new();

  protected Singleton() { }

  public static T Instance {
    get => instance;
  }
}
