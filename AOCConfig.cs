using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode
{
  /// <summary>
  /// Application configuration.
  /// </summary>
  public class AOCConfig
  {
    public static string Session { get; set; }

    public static void LoadConfig()
    {
      // Load the user secrets file.
      IConfigurationRoot config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddUserSecrets<AOCConfig>().Build();

      // Map to the properties
      foreach (PropertyInfo prop in typeof(AOCConfig).GetProperties(BindingFlags.Public | BindingFlags.Static))
      {
        prop.SetValue(null, config.GetSection(prop.Name).Value);
      }
    }
  }
}