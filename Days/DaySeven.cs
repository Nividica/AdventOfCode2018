using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Days
{
  public class DaySeven : IDay
  {
    private class Node
    {
      public string ID;
      public List<Node> Parents = new List<Node>();
      public List<Node> Children = new List<Node>();
      public int RemainingWorkTime = -1;
    }
    public async Task<(string PartOne, string PartTwo)> Run(Func<Task<string[]>> loadInputs)
    {
      string[] inputLines =
      await loadInputs();
      //new string[] { "Step C must be finished before step A can begin.", "Step C must be finished before step F can begin.", "Step A must be finished before step B can begin.", "Step A must be finished before step D can begin.", "Step B must be finished before step E can begin.", "Step D must be finished before step E can begin.", "Step F must be finished before step E can begin." };

      return (this.PartOne(BuildMap(inputLines)), this.PartTwo(BuildMap(inputLines)));
    }

    private Dictionary<string, Node> BuildMap(string[] inputLines)
    {
      var oSteps =
      inputLines.Select(line => Regex.Replace(line, "Step (.+)? must be finished before step (.+)? can begin.", "$1,$2")
        .Split(","))
        .Select(p => new { First = p[0], Then = p[1] });

      var allSteps = oSteps.Select(p => p.First).ToList();
      allSteps.AddRange(oSteps.Select(p => p.Then));
      allSteps = allSteps.Distinct().OrderBy(a => a).ToList();

      Dictionary<string, Node> nodes = allSteps.ToDictionary(s => s, s => new Node { ID = s });

      // Build relationships
      foreach (var o in oSteps)
      {
        Node parent = nodes[o.First];
        Node child = nodes[o.Then];
        child.Parents.Add(parent);
        parent.Children.Add(child);
      }
      return nodes;
    }

    private string PartOne(Dictionary<string, Node> nodes)
    {

      string partOneResult = "";

      // Start with steps with no parent
      List<Node> work = nodes.Values.Where(n => n.Parents.Count == 0).OrderBy(n => n.ID).ToList();

      while (work.Count > 0)
      {
        Node next = work[0];
        work.RemoveAt(0);

        partOneResult += next.ID;

        // Add children
        foreach (Node child in next.Children.OrderByDescending(c => c.ID))
        {
          child.Parents.Remove(next);
          if (child.Parents.Count == 0)
          {
            work.Add(child);
          }
        }
        work = work.OrderBy(n => n.ID).ToList();
      }

      return partOneResult;
      //new String(result.Distinct().ToArray());
    }

    private string PartTwo(Dictionary<string, Node> nodes)
    {
      const int numWorkers = 5;
      const int flatTaskTime = 60;

      // Calc times
      foreach (Node n in nodes.Values)
      {
        n.RemainingWorkTime = flatTaskTime + (n.ID[0] - 64);
      }

      int elapsedSeconds = 0;

      List<Node> readyTasks = nodes.Values.Where(n => n.Parents.Count == 0).OrderBy(n => n.ID).ToList();
      List<Node> workingTasks = new List<Node>();

      int freeWorkers = numWorkers;

      while (readyTasks.Count + workingTasks.Count > 0)
      {
        elapsedSeconds++;

        // Any tasks done?
        for (int idx = workingTasks.Count - 1; idx >= 0; --idx)
        {
          Node node = workingTasks[idx];
          if (--node.RemainingWorkTime == 0)
          {
            // Task is done
            workingTasks.RemoveAt(idx);
            freeWorkers++;

            // Add children to ready
            foreach (Node child in node.Children.OrderByDescending(c => c.ID))
            {
              child.Parents.Remove(node);
              if (child.Parents.Count == 0)
              {
                readyTasks.Add(child);
              }
            }
            readyTasks = readyTasks.OrderBy(n => n.ID).ToList();
          }
        }

        while (readyTasks.Count > 0 && freeWorkers > 0)
        {
          // Get the next task
          Node node = readyTasks[0];
          readyTasks.RemoveAt(0);

          workingTasks.Add(node);
          freeWorkers--;
        }
      }
      elapsedSeconds--;


      return elapsedSeconds.ToString();

    }

  }
}