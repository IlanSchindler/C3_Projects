using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day5 : Day {

    private List<(int, int)> Rules { get; set; }
    private List<List<int>> Updates { get; set; }

    public Day5():base("Day5.txt") {
      
      FormatData();
    }

    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile);
      Rules = new List<(int, int)>();
      Updates = new List<List<int>>();
      foreach(var s in input.Where(x => x.Contains('|')).ToList()) {
        Rules.Add((int.Parse(s.Split('|')[0]), int.Parse(s.Split('|')[1])));
      }
      foreach(var s in input.Where(x => x.Contains(',')).ToList()) {
        Updates.Add(s.Split(",").Select(x => int.Parse(x)).ToList());
      }
      Console.WriteLine("bla");
    }

    public override void Part1() {
      int targetSum = 0;
      foreach(var update in Updates) {
        var safeOrder = getSafeOrder(update);
        if(update.Where(x => safeOrder.Contains(x)).SequenceEqual(safeOrder)) {
          targetSum += safeOrder[(int)Math.Floor(update.Count / 2.0)];
        }
      }
      Console.WriteLine(targetSum);
    }

    public override void Part2() {
      int targetSum = 0;
      foreach(var update in Updates) {
        var safeOrder = getSafeOrder(update);
        if(!update.Where(x => safeOrder.Contains(x)).SequenceEqual(safeOrder)) {
          targetSum += safeOrder[(int)Math.Floor(update.Count / 2.0)];
        }
      }
      Console.WriteLine(targetSum);
    }

    private List<int> getSafeOrder(List<int> update) {
      var applicableRules = Rules.Where(x => update.Contains(x.Item1) && update.Contains(x.Item2));
      var safeOrder = applicableRules.OrderByDescending(x => applicableRules.Select(y => y.Item1).Count(y => y == x.Item1)).Select(x => x.Item1).Distinct().ToList();
      safeOrder.Add(applicableRules.First(x => !applicableRules.Select(y => y.Item1).Contains(x.Item2)).Item2);
      return safeOrder;
    }
  }
}



