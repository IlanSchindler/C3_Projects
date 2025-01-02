namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day22 : Day {

    List<long> initSecretNums;

    public Day22() : base("Day22.txt") {
      FormatData();
    }
    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile);
      initSecretNums = new List<long>();
      foreach(var line in input) {
        initSecretNums.Add(long.Parse(line));
      }

    }

    long getNextSecretNum(long secretNum) {
      long nextNum = (((secretNum * 64) ^ secretNum) % 16777216);
      nextNum = (((nextNum / 32) ^ nextNum) % 16777216);
      nextNum = (((nextNum * 2048) ^ nextNum) % 16777216);
      return nextNum;
    }

    public override void Part1() {
      var sNums = initSecretNums.ToList();
      for(int i = 0; i < 2000; i++) {
                                          sNums = sNums.Select(x=>getNextSecretNum(x)).ToList();
      }
      Console.WriteLine("Sum: {0}", sNums.Sum(x => x));
    }

    public override void Part2() {
      Dictionary<string, long> transitionSums = new Dictionary<string, long>();
      foreach(var secretNum in initSecretNums) {
        long nextNum = 0;
        long prevNum = secretNum;
        string transition = "XX";
        Dictionary<string, long> transitions = new Dictionary<string, long>();
        for(int i = 0; i < 3; i++) {
          nextNum = getNextSecretNum(prevNum);
          short t = (short)((nextNum % 10) - (prevNum % 10));
          transition += ((t<0)?"":"+" )+t;
          prevNum = nextNum;
        }
        for(int i = 3; i < 2000; i++) {
          nextNum = getNextSecretNum(prevNum);
          short t = (short)((nextNum % 10) - (prevNum % 10));
          transition = transition.Substring(2) + ((t<0)?"":"+" )+t;
          transitions.TryAdd(transition, nextNum % 10);      
          prevNum = nextNum;
        }
        foreach( var t in transitions) {
          transitionSums.TryAdd(t.Key, 0);
          transitionSums[t.Key] += t.Value;
        }        
      }
      
      var optimalTransition = transitionSums.MaxBy(x => x.Value);
      Console.WriteLine("Optimal Transition: {0}",optimalTransition.Key);
      Console.WriteLine("Sum: {0}", optimalTransition.Value);

    }
  }
}
