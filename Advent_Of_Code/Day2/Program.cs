using AoC_Utils;

namespace Day2 {
  internal class Program {
    static void Main(string[] args) {

      string input_path = @"D:\C3\Advent_Of_Code\Day2\input.txt";
      List<string> input = Utils.readInput(input_path);

      /* List<string> sampleData = new List<string> {
         "7 6 4 2 1","1 2 7 8 9","9 7 6 2 1","1 3 2 4 5","8 6 4 4 1","1 3 6 7 9"
       };
       input = sampleData;
 */
      //Format Input
      List<List<int>> levels = new List<List<int>>();
      foreach(string s in input) {
        List<int> newLevels = new List<int>();
        string[] levelStrings = s.Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToArray();
        foreach(string l in levelStrings) {
          int lev = 0;
          if(!int.TryParse(l, out lev)) {
            Console.WriteLine("Error Parsing Data: " + s);
            Environment.Exit(1);
          }
          newLevels.Add(lev);
        }
        levels.Add(newLevels);
      }

      Func<List<int>, bool> isSafe = (List<int> inData) => {
        if(!(inData.Distinct().OrderBy(x => x).SequenceEqual(inData) || inData.Distinct().OrderByDescending(x => x).SequenceEqual(inData))) {
          return false;
        }
        for(int i = 0; i < inData.Count - 1; i++) {
          if(Math.Abs(inData[i] - inData[i + 1]) > 3) {
            return false;
          }
        }
        return true;

      };

      int safeCount = levels.Count(x => isSafe(x));
      Console.WriteLine($"Safe Count: {safeCount}");



      Func<List<int>, bool> isSafeDampened = (List<int> inData) => {
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


      };

      Func<List<int>, bool> isSafeDampenedBrute = (List<int> inData) => {
        for(int i = 0; i < inData.Count; i++) {
          var checkData = inData.ToList();
          checkData.RemoveAt(i);
          if(isSafe(checkData)) { return true; }
        }
        return false;
      };

      /*foreach(var l in levels) {
        if(isSafeDampened(l) != isSafeDampenedBrute(l)) {
          Console.WriteLine("BLAH");
        }
        Console.Write("{0,6}{1,6}{2,6}", isSafe(l), isSafeDampened(l), isSafeDampenedBrute(l));
        Console.Write(" ");
        foreach(var x in l) {
          Console.Write(x);
          Console.Write(" ");

        }
        Console.WriteLine();
      }*/
      DateTime dampstart = DateTime.Now;
      int dampSafeCount = levels.Count(x => isSafeDampened(x));
      DateTime dampend = DateTime.Now;

      DateTime brutestart =   DateTime.Now;
      int bruteSafeCount = levels.Count(x => isSafeDampenedBrute(x));
      DateTime bruteEnd = DateTime.Now;
      Console.WriteLine($"Dampened Safe Count: {dampSafeCount}");
      Console.WriteLine($"Dampened Time: {dampend - dampstart}");
      Console.WriteLine($"Brute Safe Count: {bruteSafeCount}");
      Console.WriteLine($"Brute Time: {bruteEnd - brutestart}");


    }
  }
}
