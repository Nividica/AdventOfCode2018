using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Days
{
  public class DayThree : IDay
  {
    private class SquareClaim
    {
      public int ID { get; set; }
      public int Top { get; set; }
      public int Left { get; set; }
      public int Bottom { get; set; }
      public int Right { get; set; }
      public bool HadOverlap { get; set; } = false;

      public void OverlapingSquareInches(SquareClaim other, HashSet<(int X, int Y)> overlaps)
      {
        // If other top > this bottom, no overlap
        // If other bottom < this top, no overlap
        // If other left > this right, no overlap
        // If other right < this left, no overlap
        if (
            other.Top > this.Bottom
            || other.Bottom < this.Top
            || other.Left > this.Right
            || other.Right < this.Left
          ) { return; }

        // Get the sqaure that represents the overlaps
        SquareClaim overlap = new SquareClaim()
        {
          Top = Math.Max(this.Top, other.Top),
          Left = Math.Max(this.Left, other.Left),
          Bottom = Math.Min(this.Bottom, other.Bottom),
          Right = Math.Min(this.Right, other.Right)
        };

        // Add each pair of cords to the set
        for (int x = overlap.Left; x < overlap.Right; ++x)
        {
          for (int y = overlap.Top; y < overlap.Bottom; ++y)
          {
            overlaps.Add((x, y));
          }
        }

        this.HadOverlap = true;
        other.HadOverlap = true;
      }
    }
    public async Task<(string PartOne, string PartTwo)> Run(Func<Task<string[]>> loadInputs)
    {
      string[] inputLines = await loadInputs();
      return (this.BothParts(inputLines));
    }

    private (string, string) BothParts(string[] inputLines)
    {
      // inputLines = new string[] {
      //   "#1 @ 1,3: 4x4",
      //   "#2 @ 3,1: 4x4",
      //   "#3 @ 5,5: 2x2"
      //   };

      SquareClaim[] claims = inputLines.Select(line =>
      {
        Match m = Regex.Match(line, "^#(.+)? @ (.+)?,(.+)?: (.+)?x(.+)?$");
        int left = Int32.Parse(m.Groups[2].Value);
        int top = Int32.Parse(m.Groups[3].Value);
        int width = Int32.Parse(m.Groups[4].Value);
        int height = Int32.Parse(m.Groups[5].Value);
        return new SquareClaim
        {
          ID = Int32.Parse(m.Groups[1].Value),
          Left = left,
          Top = top,
          Right = left + width,
          Bottom = top + height
        };
      }).ToArray();

      HashSet<(int X, int Y)> overlaps = new HashSet<(int, int)>();

      for (int idx = 0; idx < claims.Length - 1; ++idx)
      {
        SquareClaim c = claims[idx];
        for (int jdx = idx + 1; jdx < claims.Length; ++jdx)
        {
          c.OverlapingSquareInches(claims[jdx], overlaps);
        }
      }

      return (overlaps.Count().ToString(), claims.Single(c => !c.HadOverlap).ID.ToString());
    }
  }
}