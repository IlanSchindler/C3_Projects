using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day9 : Day {
    List<int> memory;
    public Day9() : base("Day9.txt") {
      FormatData();
    }
    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile)[0];
      memory = new List<int>();
      for(int i = 0; i < input.Length; i++) {
        for(int j = 0; j < int.Parse("" + input[i]); j++) {
          memory.Add((i % 2 == 0) ? i / 2 : -1);
        }
      }
    }

    public override void Part1() {
      FormatData();
      for(int i = memory.Count - 1; i >= 0; i--) {
        int j = memory.FindIndex(x => x == -1);
        if(j >= i)
          break;
        memory[j] = memory[i];
        memory[i] = -1;
      }
      BigInteger sum = 0;
      for(int i = 0; i < memory.Count; i++) {
        if(memory[i] == -1)
          continue;
        sum += memory[i] * i;

      }
      Console.WriteLine(sum);
    }

    public override void Part2() {
      FormatData();
      for(int i = memory.Count - 1; i >= 0; i--) {
        if(memory[i] == -1)
          continue;
        
        int fileSize = memory.Count(x => x == memory[i]);
        int emptyStart = 0, emptyEnd = 0, blockSize = 0;
        while(true) {
          emptyStart = memory.FindIndex(emptyEnd, x => x == -1);
          emptyEnd = memory.FindIndex(emptyStart, x => x != -1);
          if(emptyStart > i) {
            break;
          }
          blockSize = emptyEnd - emptyStart;
          if(fileSize <= blockSize) {
            for(int j = emptyStart; j < emptyStart + fileSize; j++) {
              memory[j] = memory[i];
            }
            for(int j = i - fileSize + 1; j < i + 1; j++) {
              memory[j] = -1;
            }
            
            break;
          }
        }
        
      }
      BigInteger sum = 0;
      for(int i = 0; i < memory.Count; i++) {
        if(memory[i] == -1)
          continue;
        sum += memory[i] * i;

      }
      Console.WriteLine(sum);
    }
  }
}
