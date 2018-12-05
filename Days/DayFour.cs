using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Days
{
  public class DayFour : IDay
  {
    private class LogEntry
    {
      public DateTime Timestamp { get; set; }
      public string Action { get; set; }
      public int? Guard { get; set; }

      public LogEntry(string line)
      {
        Match m = Regex.Match(line, @"\[(\d+)-(\d+)-(\d+) (\d+):(\d+)\] (.+)");

        int year = Int32.Parse(m.Groups[1].Value);
        int month = Int32.Parse(m.Groups[2].Value);
        int day = Int32.Parse(m.Groups[3].Value);
        int hour = Int32.Parse(m.Groups[4].Value);
        int minute = Int32.Parse(m.Groups[5].Value);
        this.Timestamp = new DateTime(year, month, day, hour, minute, 0);

        this.Action = m.Groups[6].Value;
        m = Regex.Match(this.Action, @"^Guard #(\d+) (.*)");
        if (m.Groups.Count() > 1)
        {
          this.Guard = Int32.Parse(m.Groups[1].Value);
          this.Action = m.Groups[2].Value;
        }
      }

    }

    private class Guard
    {
      public int ID { get; set; }

      public List<(int From, int To)> SleepPeriods { get; set; } = new List<(int From, int To)>();

      public int MinutesAsleep { get; set; } = 0;

      private DateTime? FellAsleep = null;

      public void AddAction(LogEntry log)
      {
        if (log.Action == "falls asleep")
        {
          this.FellAsleep = log.Timestamp;
        }
        else if (log.Action == "wakes up")
        {
          this.SleepPeriods.Add((FellAsleep.Value.Minute, log.Timestamp.Minute));
          this.MinutesAsleep += (log.Timestamp.Subtract(FellAsleep.Value).Minutes);
          this.FellAsleep = null;
        }
      }

      public (int minute, int times) MinuteMostAsleep()
      {
        int[] minutes = new int[60];
        int maxMinute = 0;
        int maxMinuteSum = 0;
        foreach (var period in this.SleepPeriods)
        {
          foreach (int minute in Enumerable.Range(period.From, period.To - period.From))
          {
            int minuteSum = ++minutes[minute];
            if (minuteSum > maxMinuteSum)
            {
              maxMinute = minute;
              maxMinuteSum = minuteSum;
            }
          }
        }
        return (maxMinute, minutes[maxMinute]);
      }
    }

    public async Task<(string PartOne, string PartTwo)> Run(Func<Task<string[]>> loadInputs)
    {
      string[] inputLines = await loadInputs();
      var parsed = this.ParseLogs(inputLines);
      return (
        (parsed.SleepyHead.MinuteMostAsleep().minute * parsed.SleepyHead.ID).ToString(),
        this.PartTwo(parsed.Gaurds)
      );
    }

    private (Guard[] Gaurds, Guard SleepyHead) ParseLogs(string[] inputLines)
    {
      // Random rng = new Random(527);
      // inputLines = (new List<string>{
      //   "[1518-11-01 00:00] Guard #10 begins shift",
      //   "[1518-11-01 00:05] falls asleep",
      //   "[1518-11-01 00:25] wakes up",
      //   "[1518-11-01 00:30] falls asleep",
      //   "[1518-11-01 00:55] wakes up",
      //   "[1518-11-01 23:58] Guard #99 begins shift",
      //   "[1518-11-02 00:40] falls asleep",
      //   "[1518-11-02 00:50] wakes up",
      //   "[1518-11-03 00:05] Guard #10 begins shift",
      //   "[1518-11-03 00:24] falls asleep",
      //   "[1518-11-03 00:29] wakes up",
      //   "[1518-11-04 00:02] Guard #99 begins shift",
      //   "[1518-11-04 00:36] falls asleep",
      //   "[1518-11-04 00:46] wakes up",
      //   "[1518-11-05 00:03] Guard #99 begins shift",
      //   "[1518-11-05 00:45] falls asleep",
      //   "[1518-11-05 00:55] wakes up"
      // }).OrderBy(i => rng.Next()).ToArray();

      LogEntry[] logs = inputLines
              .Select(line => new LogEntry(line))
              .OrderBy(log => log.Timestamp)
              .ToArray();

      // Create guards
      Dictionary<int, Guard> guards = new Dictionary<int, Guard>();
      Guard guard = null;
      Guard sleepyHead = null;
      foreach (LogEntry log in logs)
      {
        if (log.Guard.HasValue)
        {
          int gid = log.Guard.Value;
          if (!guards.TryGetValue(gid, out guard))
          {
            guards.Add(gid, guard = new Guard { ID = gid });
          }
        }
        guard.AddAction(log);
        sleepyHead = (sleepyHead == null ? guard : (sleepyHead.MinutesAsleep < guard.MinutesAsleep ? guard : sleepyHead));
      }

      return (guards.Values.ToArray(), sleepyHead);
    }

    private string PartTwo(Guard[] guards)
    {
      // Of all guards, which guard is most frequently asleep on the same minute?
      var regularSleeper = guards.Select(g =>
        {
          var mins = g.MinuteMostAsleep();
          return new
          {
            g.ID,
            mins.minute,
            mins.times
          };
        })
        .OrderByDescending(mg => mg.times)
        .First();

      return (regularSleeper.ID * regularSleeper.minute).ToString();
    }
  }
}