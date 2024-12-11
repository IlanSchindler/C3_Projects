using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day2 : Day {

    private List<List<int>> levels;

    public Day2() : base("Day2.txt") {
    }

    public override void FormatData() {
      List<string> input = Utilities.readInputFile(inputFile);

      //Format Input
      levels = new List<List<int>>();
      foreach(string s in input) {
        List<int> newLevels = new List<int>();
        string[] levelStrings = s.Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToArray();
        foreach(string l in levelStrings) {
          newLevels.Add(int.Parse(l));
        }
        levels.Add(newLevels);
      }
    }

    public override void Part1() {
      FormatData();
      int safeCount = levels.Count(x => isSafe(x));
      Console.WriteLine($"Safe Count: {safeCount}");
    }

    public override void Part2() {
      FormatData();
      int dampSafeCount = levels.Count(x => isSafeDamp(x));
      Console.WriteLine($"Dampened Safe Count: {dampSafeCount}");
    }

    private bool isSafe(List<int> inData) {
      if(!(inData.Distinct().OrderBy(x => x).SequenceEqual(inData) || inData.Distinct().OrderByDescending(x => x).SequenceEqual(inData))) {
        return false;
      }
      for(int i = 0; i < inData.Count - 1; i++) {
        if(Math.Abs(inData[i] - inData[i + 1]) > 3) {
          return false;
        }
      }
      return true;
    }

    private bool isSafeDamp(List<int> inData) {
      List<int> transitions = new List<int>();
      for(int i = 0; i < inData.Count - 1; i++) {
        transitions.Add(inData[i + 1] - inData[i]);
      }

      if(transitions.Sum() < 0)
        transitions = transitions.Select(x => x * -1).ToList();

      if(transitions.Count(x => x < 1 || x > 3) == 0)
        return true;

      int errorIndex = transitions.IndexOf(transitions.First(x => x < 1 || x > 3));
      int errorTransition = transitions[errorIndex];
      transitions.RemoveAt(errorIndex);

      if(errorIndex != 0)
        transitions[errorIndex - 1] += errorTransition;
      if(transitions.Count(x => x < 1 || x > 3) == 0)
        return true;
      if(errorIndex != 0)
        transitions[errorIndex - 1] -= errorTransition;

      if(errorIndex != transitions.Count)
        transitions[errorIndex] += errorTransition;
      if(transitions.Count(x => x < 1 || x > 3) == 0)
        return true;
      if(errorIndex != transitions.Count)
        transitions[errorIndex] += errorTransition;

      return false;

    }

    private bool isSafeDampBrute(List<int> inData) {
      for(int i = 0; i < inData.Count; i++) {
        var checkData = inData.ToList();
        checkData.RemoveAt(i);
        if(isSafe(checkData)) { return true; }
      }
      return false;
    }
  }
}