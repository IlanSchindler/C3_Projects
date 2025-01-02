using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day23 : Day {

    Dictionary<string, List<string>> computers;
    List<string> groups;

    public Day23() : base("Day23.txt") {
      FormatData();
    }
    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile);
      computers = new Dictionary<string, List<string>>();
      foreach(var line in input) {
        var inComps = line.Split('-');
        computers.TryAdd(inComps[0], new List<string>());
        computers.TryAdd(inComps[1], new List<string>());
        computers[inComps[0]].Add(inComps[1]);
        computers[inComps[1]].Add(inComps[0]);
      }
      foreach(var comp in computers.Keys) {
        computers[comp] = computers[comp].Distinct().OrderBy(x => x).ToList();
      }
    }

    public override void Part1() {
      groups = new List<string>();
      string[] triple = new string[] { " ", " ", " " };
      foreach(var comp1 in computers.Keys) {
        triple[0] = comp1;
        foreach(var comp2 in computers[comp1]) {
          triple[1] = comp2;
          var triples = computers[comp2].Where(x => computers[x].Contains(comp1)).ToList();
          foreach(var comp3 in triples) {
            triple[2] = comp3;

            groups.Add(string.Join(',', triple.OrderBy(x => x).ToList()));
          }
        }
      }

      groups = groups.Distinct().OrderBy(x => x).ToList();
      string rString = @"t\w";
      List<string> validGroups = groups.Where(x => Regex.IsMatch(x, rString)).ToList();

      Console.WriteLine("Groups Found: {0}", validGroups.Count);
    }

    public override void Part2() {
      Queue<string> testGroups = new Queue<string>(groups.ToList());
      List<string> maxGroups = new List<string>();
      while(testGroups.Count > 0) {
        var group = testGroups.Dequeue();
        var groupSplit = group.Split(',');
        List<string> comps = computers.Keys.Where(x => groupSplit.All(y => computers[x].Contains(y))).ToList();
        if(comps.Count == 0) {
          maxGroups.Add(group);
        }
        foreach(var comp in comps) {
          string newGroup = group + "," + comp;
          newGroup = string.Join(',', newGroup.Split(',').OrderBy(y => y).ToList());
          if(!testGroups.Contains(newGroup))
            testGroups.Enqueue(newGroup);
        }
      }
      string largestGroup = maxGroups.MaxBy(x => x.Length);
      Console.WriteLine("Largest Group: {0}", largestGroup);
      
    }
  }
}
