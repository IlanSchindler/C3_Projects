using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Advent_Of_Code_2024 {
  internal class Day4 : Day {

    private List<List<char>> FormattedData { get; set; }
    private int rCount { get; set; }
    private int cCount { get; set; }

    public Day4():base("Day4.txt") {
      

      FormatData();
    }

    public override void Part1() {
      int xmasCount = 0;
      (int dr, int dc)[] moves = [(1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1), (0, 1), (1, 1)];
      char[] targets = ['X', 'M', 'A', 'S'];
      for(int i = 0; i < rCount; i++) {
        for(int j = 0; j < cCount; j++) {
          foreach(var move in moves) {
            xmasCount++;
            for(int d = 0; d < 4; d++) {
              int di = i + move.dr * d;
              int dj = j + move.dc * d;
              if(di < 0 ||
                 di >= rCount ||
                 dj < 0 ||
                 dj >= cCount ||
                 FormattedData[di][dj] != targets[d]) {
                xmasCount--;
                break;
              }
            }
          }
        }
      }
      Console.WriteLine(xmasCount);
    }

    public override void Part2() {
      int xmasCount = 0;
      for(int i = 0; i < rCount; i++) {
        for(int j = 0; j < cCount; j++) {
          if(i == 0 || j == 0 || i == rCount - 1 || j == cCount - 1 || FormattedData[i][j] != 'A') {
            continue;
          }
          //Found 'A'
          string diag1 = "" + FormattedData[i + 1][j + 1] + FormattedData[i - 1][j - 1];
          string diag2 = "" + FormattedData[i + 1][j - 1] + FormattedData[i - 1][j + 1];
          if((diag1.Equals("MS") || diag1.Equals("SM")) && (diag2.Equals("MS") || diag2.Equals("SM")))
            xmasCount++;
        }
      }
      Console.WriteLine(xmasCount);
    }

    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile);
      FormattedData = new List<List<char>>();
      foreach(var line in input) {
        List<char> dataline = new List<char>();
        foreach(char c in line) {
          dataline.Add(c);
        }
        FormattedData.Add(dataline);
      }
      rCount = FormattedData.Count;
      cCount = FormattedData[0].Count;
    }
  }
}
