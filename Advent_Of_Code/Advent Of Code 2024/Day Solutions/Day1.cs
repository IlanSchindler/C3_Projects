using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day1 : Day {

    private List<List<int>> data;
    public Day1() : base("Day1.txt") {

    }
    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile);
      data = new List<List<int>> { new List<int>(), new List<int>() };
      foreach(var line in input) {
        string[] splitInput = line.Split(' ', StringSplitOptions.TrimEntries).Where(x => string.IsNullOrWhiteSpace(x)).ToArray();
        data[0].Add(int.Parse(splitInput[0]));
        data[1].Add(int.Parse(splitInput[1]));
      }
      data[0] = data[0].OrderBy(x => x).ToList();
      data[1] = data[1].OrderBy(x => x).ToList();
    }

    public override void Part1() {
      FormatData();
      int totalDistance = 0;
      for(int i = 0; i < data[0].Count; i++) {
        totalDistance += Math.Abs(data[0][i] - data[1][i]);
      }
      Console.WriteLine($"Total Distance: {totalDistance}");
    }

    public override void Part2() {
      FormatData();
      int similarityScore = 0;
      for(int i = 0; i < data[0].Count; i++) {
        similarityScore += data[0][i] * data[1].Count(x => x == data[0][i]);
      }
    }
  }
}