using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day25 : Day {

    List<string> keys = new List<string>();
    List<string> locks = new List<string>();

    public Day25() : base("Day25.txt") {
      FormatData();
    }
    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile);
      for(int i = 0; i < input.Count; i += 8) {
        char[] levels = "00000".ToCharArray();
        for(int j = 1; j < 6; j++) {
          for(int k = 0; k < 5; k++) {
            if(input[i + j][k] == '#')
              levels[k]++;
          }
        }
        if(input[i] == "#####") {
          //Section is a lock
          locks.Add(new string(levels));
        } else if(input[i + 6] == "#####") {
          //Section is a key
          keys.Add(new string(levels));
        }
      }
    }

    public override void Part1() {
      //101 e
      List<(string key, string lok)> keyLockPairs = new List<(string key, string lok)>();
      foreach(var k in keys) {
        foreach(var l in locks) {
          var match = k.Zip(l, (x, y) => (char)(x + y));
          // Console.WriteLine(new string(match.ToArray()));
          if(match.Any(x => x > 'e')) {
            continue;
          }
          keyLockPairs.Add((k, l));

        }
      }
      Console.WriteLine(keyLockPairs.Count);
      
    }

    public override void Part2() {
      throw new NotImplementedException();
    }
  }
}
