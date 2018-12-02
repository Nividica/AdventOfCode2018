using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode
{
  public interface IDay
  {
    (string PartOne, string PartTwo) Run(string[] inputLines);
  }
}