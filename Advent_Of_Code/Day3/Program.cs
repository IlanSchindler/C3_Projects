using AoC_Utils;
using System.Text.RegularExpressions;

namespace Day3 {
  internal class Program {
    static void Main(string[] args) {
      string path = @"D:\C3\Advent_Of_Code\Day3\input.txt";
      List<string> input = Utils.readInput(path);

      string regexPattern1 = @"do\(\)[\s\S]*?((don\'t\(\))|$)";
      string regexPattern2 = @"mul\(\d{1,3},\d{1,3}\)";

      string inputString = "do()";
      foreach(string s in input) {
        inputString += s;
      }

      List<string> activeSections = Regex.Matches(inputString, regexPattern1).Select(x => x.Value).ToList();

      List<string> mults = new List<string>();
      foreach(var match in activeSections) {
        mults.AddRange(Regex.Matches(match, regexPattern2).Select(x => x.Value).ToList());
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
