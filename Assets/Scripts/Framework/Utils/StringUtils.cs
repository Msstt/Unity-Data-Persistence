public static class StringUtils {
  public static string GetNameFromPath(string path) {
    if (string.IsNullOrEmpty(path)) {
      return string.Empty;
    }
    int lastSlashIndex = path.LastIndexOf('/');
    if (lastSlashIndex >= 0 && lastSlashIndex < path.Length - 1) {
      return path[(lastSlashIndex + 1)..];
    }
    return path;
  }
}
