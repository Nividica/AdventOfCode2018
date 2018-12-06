using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Days
{
  public class DaySix : IDay
  {
    private class Coord
    {
      public int X;
      public int Y;
      public char Name;
      public bool Infinite = false;

    }
    public async Task<(string PartOne, string PartTwo)> Run(Func<Task<string[]>> loadInputs)
    {
      string[] inputLines =
      await loadInputs();
      //new string[] { "1, 1", "1, 6", "8, 3", "3, 4", "5, 5", "8, 9" };
      return this.Both(inputLines);
    }

    private (string, string) Both(string[] inputLines)
    {
      char nChar = 'A';
      var coords = inputLines.Select(line =>
      {
        string[] xy = line.Split(',');
        return new Coord
        {
          X = Int32.Parse(xy[0]),
          Y = Int32.Parse(xy[1]),
          Name = nChar++
        };
      }).ToList();

      // Define the square border
      var border = new
      {
        Left = coords.Select(c => c.X).OrderBy(x => x).First(),
        Right = 1 + coords.Select(c => c.X).OrderByDescending(x => x).First(),
        Top = coords.Select(c => c.Y).OrderBy(y => y).First(),
        Bottom = 1 + coords.Select(c => c.Y).OrderByDescending(y => y).First(),
      };

      List<Coord> cordsThatAreClosest = new List<Coord>();
      int safeRegionPoints = 0;

      for (int x = border.Left; x < border.Right; ++x)
      {
        for (int y = border.Top; y < border.Bottom; ++y)
        {
          Coord closest = null;
          int cDist = Int32.MaxValue;
          int distAll = 0;
          int cCount = 0;
          // What coord is this point closest to via Manhatten distance?
          foreach (Coord cord in coords)
          {
            int distance = Math.Abs(cord.X - x) + Math.Abs(cord.Y - y);
            // Equally close?
            if (cDist == distance)
            {
              cCount++;
            }
            // Closer?
            if (distance < cDist) { closest = cord; cDist = distance; cCount = 1; }

            distAll += distance;
          }
          // Only closest to one?
          if (cCount == 1)
          {
            bool infinite = (x == border.Left) || (x + 1 == border.Right) || (y == border.Top) || (y + 1 == border.Bottom);
            closest.Infinite = closest.Infinite || infinite;
            cordsThatAreClosest.Add(closest);
          }
          // Sum dist < 10000?
          if (distAll < 10000)
          {
            safeRegionPoints++;
          }
        }
      }

      var sums = cordsThatAreClosest.Where(c => !c.Infinite).GroupBy(c => c).Select(g => new { Coord = g.Key, Count = g.Count() }).ToList();
      var biggestArea = sums.OrderByDescending(s => s.Count).First();
      Console.WriteLine("{0} @ {1}", biggestArea.Coord.Name, biggestArea.Count);

      return (biggestArea.Count.ToString(), safeRegionPoints.ToString());
    }
  }
}