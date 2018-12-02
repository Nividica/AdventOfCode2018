using System.IO;
using System.Linq;

namespace AdventOfCode
{
  public class InputCache
  {
    private readonly string TempPath;
    public InputCache()
    {
      this.TempPath = Path.Combine(Path.GetTempPath(), "AOC2018Inputs");
      if (!Directory.Exists(this.TempPath))
      {
        Directory.CreateDirectory(this.TempPath);
      }
    }

    public string[] LoadDay(int day)
    {
      string dayPath = Path.Combine(this.TempPath, $"{day}.txt");
      if (!File.Exists(dayPath)) { return null; }
      return File.ReadLines(dayPath).ToArray();
    }

    public void SaveDay(int day, string data)
    {
      string dayPath = Path.Combine(this.TempPath, $"{day}.txt");
      File.WriteAllText(dayPath, data);
    }

    public void ClearCache()
    {
      foreach (string item in Directory.EnumerateFiles(this.TempPath))
      {
        File.Delete(item);
      }
    }
  }
}