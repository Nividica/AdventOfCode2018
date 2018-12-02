
using System;

namespace AdventOfCode
{
  /// <summary>
  /// My solutions to the 2018 Advent Of Code  
  /// Author: Chris McGhee
  /// </summary>
  public class Program
  {
    public static int Main(string[] args)
    {
      try
      {
        // Load the config
        AOCConfig.LoadConfig();

        // Create the AOC
        AOC adventOfCode = Setup.InitAOC();

        // Run AOC
        return adventOfCode.Run().GetAwaiter().GetResult();
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error: Exiting due to exception {0}{1}", System.Environment.NewLine, ex.ToString());
        Console.ReadKey(true);
        return -1;
      }
    }
  }
}
