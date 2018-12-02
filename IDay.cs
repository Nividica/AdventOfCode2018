using System;

namespace AdventOfCode
{
  public interface IDay
  {
    (string PartOne, string PartTwo) Run(string[] inputLines);
  }
}