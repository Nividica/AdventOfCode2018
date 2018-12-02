using System;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Days
{
  public class DayTwo : IDay
  {
    public async Task<(string PartOne, string PartTwo)> Run(Func<bool, Task<string[]>> loadInputs)
    {
      string[] inputLines = await loadInputs(true);
      return (this.PartOne(inputLines), this.PartTwoFluent(inputLines) + "  vs  " + this.PartTwoNestedLoops(inputLines));
    }

    private string PartOne(string[] boxIDs)
    {
      int twos = 0;
      int threes = 0;

      // For each box ID
      foreach (string id in boxIDs)
      {
        // Convert down to an array of characters
        var repeats = id.ToCharArray()
        // Group by each character
          .GroupBy((character) => character)
          // Select the count
          .Select(group => group.Count());

        // Does any letter appear exactly twice?
        twos += repeats.Contains(2) ? 1 : 0;

        // Does any letter appear exactly thrice?
        threes += repeats.Contains(3) ? 1 : 0;
      }

      return (twos * threes).ToString();
    }

    private string PartTwoFluent(string[] boxIDs)
    {
      char[][] charMap = boxIDs.Select(id => id.ToCharArray()).ToArray();
      var match = (
        from i in Enumerable.Range(0, boxIDs.Length - 2)
        from j in Enumerable.Range(i + 1, boxIDs.Length - (1 + i))
        where charMap[i].Where((c, k) => charMap[j][k] != c).Count() == 1
        select new { i, j }
      ).First();
      return new String(charMap[match.i].Where((c, k) => charMap[match.j][k] == c).ToArray());
    }

    private string PartTwoNestedLoops(string[] boxIDs)
    {

      // Assumption 1: All IDs are the same length
      // Assumption 2: There exists two IDs which only differ by 1 char

      // Split box IDs into 2D array [IDIndex,CharIndex] = char
      char[][] charMap = boxIDs.Select(id => id.ToCharArray()).ToArray();

      int idxIDLeft = 0, diffIdx = 0;
      // Use of local function for an easy way to break nested loops.
      void LocateMinDiff()
      {
        // Loop over every id
        for (idxIDLeft = 0; idxIDLeft < boxIDs.Length; ++idxIDLeft)
        {
          char[] leftChars = charMap[idxIDLeft];
          // Loop over every id after the first id
          // Because all prior id's have already been checked against this one
          for (int idxIDRight = idxIDLeft + 1; idxIDRight < boxIDs.Length; ++idxIDRight)
          {
            char[] rightChars = charMap[idxIDRight];
            int diffCount = 0;
            // Check each char until there is more than 1 difference, or there are no more chars to check
            for (int idxChar = 0; idxChar < leftChars.Length && diffCount < 2; ++idxChar)
            {
              if (leftChars[idxChar] != rightChars[idxChar])
              {
                ++diffCount;
                // Mark the different char index, this will aid in building final string
                diffIdx = idxChar;
              }
            }
            // If a match is found, exit local function
            if (diffCount == 1) return;
          }
        }
      };
      LocateMinDiff();

      // Get all chars that are the same, that is, exclude the char at the marked index
      return new String(charMap[idxIDLeft].Where((_, idx) => idx != diffIdx).ToArray());

    }
  }
}