using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Days
{
  public class DayOne : IDay
  {

    public async Task<(string PartOne, string PartTwo)> Run(Func<Task<string[]>> loadInputs)
    {
      List<long> frequencies = (await loadInputs()).Select(v => Int64.Parse(v)).ToList();
      return (
        this.PartOne(frequencies),
        this.PartTwo(frequencies)
      );
    }

    private string PartOne(List<long> frequencies)
    {
      // Add all values
      return frequencies
        .Aggregate(0L, (a, b) => a + b)
        .ToString();
    }

    private string PartTwo(List<long> frequencies)
    {
      // Build enumerator
      IEnumerator<long> nums = frequencies.GetEnumerator();

      // Tracks what values have been seen
      HashSet<long> seen = new HashSet<long>();

      // Holds the summation
      long accumulator = 0;

      // Loop until we see the same number twice
      while (!seen.Contains(accumulator))
      {
        // Add to set
        seen.Add(accumulator);

        // Next, loop back to begining if the end is reached
        if (!nums.MoveNext()) { nums.Reset(); nums.MoveNext(); }

        // Sum
        accumulator += nums.Current;
      }
      return accumulator.ToString();
    }
  }
}