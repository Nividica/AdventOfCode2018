using System;
using System.Threading.Tasks;

namespace AdventOfCode
{
  public interface IDay
  {
    Task<(string PartOne, string PartTwo)> Run(Func<Task<string[]>> loadInputs);
  }
}