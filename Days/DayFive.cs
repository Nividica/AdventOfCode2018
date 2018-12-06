using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode.Days
{
  public class DayFive : IDay
  {

    public async Task<(string PartOne, string PartTwo)> Run(Func<Task<string[]>> loadInputs)
    {
      string[] inputList =
       //new string[] { "DCaAbBcdc" };
       await loadInputs();

      return (
        this.PartOne(new List<char>(inputList[0].ToCharArray())).ToString(),
        this.PartTwo(inputList[0])
      );
    }

    private int PartOne(List<char> sequence)
    {
      int count = sequence.Count;
      for (int idx = 1; idx < sequence.Count - 1; ++idx)
      {
        if (idx > 0 && count > 1 && Math.Abs(sequence[idx] - sequence[idx - 1]) == 32)
        {
          sequence.RemoveAt(--idx);
          sequence.RemoveAt(idx--);
          count -= 2;
        }
      }
      return sequence.Count;
    }

    private string PartTwo(string input)
    {
      IEnumerable<char> uniqueChars = input.ToLower().Distinct();
      int smallestChain = Int32.MaxValue;
      SpinLock spinner = new SpinLock(false);

      Parallel.ForEach(uniqueChars, (keyChar) =>
         {
           int len = this.PartOne(input.Where(c => ((c != keyChar) && ((c + 32) != keyChar))).ToList());

           bool b = false;
           spinner.Enter(ref b);
           smallestChain = Math.Min(smallestChain, len);
           spinner.Exit();
         });

      return (smallestChain).ToString();

    }
  }
}