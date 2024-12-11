using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day10 : Day {

    int[,] map;
    int mapW { get { return map.GetLength(0); } }
    int mapH { get { return map.GetLength(1); } }
    public Day10() : base("Day10.txt") {
      FormatData();
    }
    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile);
      map = new int[input.Count, input[0].Length];
      for(int i = 0; i < mapW; i++) {
        for(int j = 0; j < mapH; j++) {
          map[i, j] = int.Parse(""+input[i][j]);
        }
      }
    }

    public override void Part1() {
      int validPaths = 0;
      for(int i = 0; i < mapW; i++) {
        for(int j = 0; j < mapH; j++) {
          if(map[i, j] == 0) {
            validPaths += countPaths((i, j)).Distinct().Count();
          }
        }
      }
      Console.WriteLine($"Valid Paths: {validPaths}");
    }

    private List<(int,int)> countPaths((int,int) node) {
      if(map[node.Item1, node.Item2] == 9)
        return new List<(int, int)> { node };
      List<(int,int)> validPaths = new List<(int, int)>();
      int nextTop = -1;
      foreach(var move in Utilities.movements.Values) {
        nextTop = -1;
        Utilities.tryIgnore<IndexOutOfRangeException>(()=>nextTop = map[node.Item1 + move.Item1,node.Item2 + move.Item2]);
        if(nextTop == map[node.Item1, node.Item2] + 1) {
          validPaths.AddRange(countPaths((node.Item1 + move.Item1, node.Item2 + move.Item2)));
        }
      }

      return validPaths;
    }

    public override void Part2() {
      int validPaths = 0;
      for(int i = 0; i < mapW; i++) {
        for(int j = 0; j < mapH; j++) {
          if(map[i, j] == 0) {
            validPaths += countPaths((i, j)).Count();
          }
        }
      }
      Console.WriteLine($"Valid Paths: {validPaths}");
    }
  }
}
