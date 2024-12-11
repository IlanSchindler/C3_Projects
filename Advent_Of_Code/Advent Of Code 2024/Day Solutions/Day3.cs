using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day3 : Day {
    private string data = "";
    private string regexPatternDo = @"do\(\)[\s\S]*?((don\'t\(\))|$)";
      private string regexPatternMul = @"mul\(\d{1,3},\d{1,3}\)";
    
    public Day3() : base("Day3.txt") {

    }
    
    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile);
      data = "do()";
      foreach(string s in input) {
        data += s;
      }
    }

    public override void Part1() {
      List<string> mults = Regex.Matches(data, regexPatternMul).Select(x => x.Value).ToList();

      int sum = 0;
      foreach(string mult in mults) {
        string[] ints = Regex.Matches(mult, @"\d{1,3}").Select(x => x.Value).ToArray();
        sum += int.Parse(ints[0]) * int.Parse(ints[1]);
      }
      Console.Write("Sum: ");
      Console.WriteLine(sum);
    }

    public override void Part2() {
      List<string> activeSections = Regex.Matches(data, regexPatternDo).Select(x => x.Value).ToList();

      List<string> mults = new List<string>();
      foreach(var match in activeSections) {
        mults.AddRange(Regex.Matches(match, regexPatternMul).Select(x => x.Value).ToList());
      }

      int sum = 0;
      foreach(string mult in mults) {
        string[] ints = Regex.Matches(mult, @"\d{1,3}").Select(x => x.Value).ToArray();
        sum += int.Parse(ints[0]) * int.Parse(ints[1]);
      }
      Console.Write("Sum: ");
      Console.WriteLine(sum);
    }
  }
}

