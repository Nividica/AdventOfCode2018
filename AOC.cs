using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AdventOfCode
{
  public class AOC
  {
    /// <summary>
    /// HTTP Client for AOC.com
    /// </summary>
    private readonly HttpClient Client;

    /// <summary>
    /// Maps a day to a handler
    /// </summary>
    /// <typeparam name="int"></typeparam>
    /// <typeparam name="IDay"></typeparam>
    /// <returns></returns>
    private readonly Dictionary<int, IDay> DayHandlers;

    private readonly InputCache Cache;

    public AOC(Dictionary<int, IDay> dayHandlers, HttpClient client, InputCache cache)
    {
      this.DayHandlers = dayHandlers;
      this.Client = client;
      this.Cache = cache;
    }

    public async Task<int> Run()
    {
      // Enter main loop
      while (true)
      {
        Console.WriteLine("Enter day number, 'clean' to erase cache, 'exit' or nothing to exit");
        string userInput = Console.ReadLine();

        // Exit?
        if (String.IsNullOrWhiteSpace(userInput) || userInput == "exit")
        {
          return 0;
        }
        else if (userInput == "clean")
        {
          this.Cache.ClearCache();
          Console.WriteLine("Cache cleared");
          continue;
        }

        // Convert to day number
        int dayNumber;
        if (!Int32.TryParse(userInput, out dayNumber))
        {
          Console.WriteLine($"Warning: '{userInput}' is not a recognizable day number");
          continue;
        }

        // Is there a handler for that day?
        IDay dayHandler;
        if (!this.DayHandlers.TryGetValue(dayNumber, out dayHandler))
        {
          Console.WriteLine($"Handler for day {dayNumber} does not exist.");
          continue;
        }

        // Run the handler
        var results = await dayHandler.Run(async () => { return await this.LoadDayInput(dayNumber); });
        Console.WriteLine("Part One: {1}{0}Part Two: {2}{0}{0}", System.Environment.NewLine, results.PartOne, results.PartTwo);
      }
    }



    /// <summary>
    /// Loads the input for the specified day
    /// </summary>
    /// <param name="day"></param>
    /// <returns></returns>
    private async Task<string[]> LoadDayInput(int day)
    {
      // Check cache
      string[] lines = this.Cache.LoadDay(day);
      if (lines is null)
      {
        Console.WriteLine("Info: Cache miss, loading from web...");
        // Get the input data
        HttpResponseMessage msg = await this.Client.GetAsync($"https://adventofcode.com/2018/day/{day}/input");
        string data = await msg.Content.ReadAsStringAsync();

        // Split by newline
        lines = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        // Save
        this.Cache.SaveDay(day, String.Join(System.Environment.NewLine, lines));
        Console.WriteLine("Info: Input cached.");
      }
      return lines;
    }

  }
}