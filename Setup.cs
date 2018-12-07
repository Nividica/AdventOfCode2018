using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using AdventOfCode.Days;

namespace AdventOfCode
{
  public static class Setup
  {
    public static AOC InitAOC()
    {
      return new AOC(
        Setup.SetupDayHandlers(),
        Setup.SetupHTTPClient(),
        new InputCache()
        );
    }

    /// <summary>
    /// Register each day handler
    /// </summary>
    private static Dictionary<int, IDay> SetupDayHandlers()
    {
      Dictionary<int, IDay> handlers = new Dictionary<int, IDay>();
      handlers.Add(1, new DayOne());
      handlers.Add(2, new DayTwo());
      handlers.Add(3, new DayThree());
      handlers.Add(4, new DayFour());
      handlers.Add(5, new DayFive());
      handlers.Add(6, new DaySix());
      handlers.Add(7, new DaySeven());
      return handlers;
    }

    /// <summary>
    /// Creates the HTTP client for AdventOfCode.com
    /// </summary>
    private static HttpClient SetupHTTPClient()
    {
      CookieContainer cookies = new CookieContainer();
      cookies.Add(
        new Uri("https://adventofcode.com"),
        new Cookie("session", AOCConfig.Session)
      );
      return new HttpClient(new HttpClientHandler { CookieContainer = cookies });
    }
  }
}