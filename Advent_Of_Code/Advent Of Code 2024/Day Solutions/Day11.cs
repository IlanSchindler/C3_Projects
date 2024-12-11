using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day11 : Day {
    int blinktotal = 0;
    List<string> stones;
    public Day11() : base("Day11.txt") {
      FormatData();
    }
    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile);
      stones = new List<string>();
      stones.AddRange(input[0].Split(' '));
    }

    public override void Part1() {
      FormatData();
      Console.WriteLine($"Blink 0: " + stones.Count);
      for(int i = 0; i < 25; i++) {
        blink();
        Console.WriteLine($"Blink {i + 1}: " + stones.Count);
      }
    }

    public override void Part2() {
      FormatData();
      blinkMap();
      /*int stoneCount = 0;
      foreach(string stone in stones) {
        blinkRecur(stone, 0, 75);
      }
      *//*DateTime start = DateTime.Now;
      Console.WriteLine($"Blink 0: " + stones.Count);
      for(int i = 0; i < 75; i++) {
        blink();
        Console.WriteLine($"Blink {i + 1}: " + stones.Count);
      }
      DateTime end = DateTime.Now;*//*
      //Console.WriteLine(stones.Count);
      //Console.WriteLine(end - start);
      Console.WriteLine(stoneCount);*/
    }

    private void blink() {
      List<string> newStones = new List<string>();
      foreach(var stone in stones) {
        if(stone.Equals("0"))
          newStones.Add("1");
        else if(stone.Length % 2 == 0) {
          newStones.Add("" + int.Parse(stone.Substring(0, stone.Length / 2)));
          newStones.Add("" + int.Parse(stone.Substring(stone.Length / 2)));
        } else {
          newStones.Add("" + (long.Parse(stone) * 2024));
        }
      }
      stones.Clear();
      stones.AddRange(newStones.ToList());
    }

    private void blinkRecur(string stone, int blinkCount, int limit) {
      if(blinkCount++ == limit) {
        blinktotal++;

      } else if(stone.Equals("0"))
        blinkRecur("1", blinkCount, limit);
      else if(stone.Length % 2 == 0) {
        blinkRecur("" + int.Parse(stone.Substring(0, stone.Length / 2)), blinkCount, limit);
        blinkRecur("" + int.Parse(stone.Substring(stone.Length / 2)), blinkCount, limit);
      } else {
        blinkRecur("" + (long.Parse(stone) * 2024), blinkCount, limit);
      }

    }

    private void blinkMap() {
      Dictionary<string, long> stoneCounts = new Dictionary<string, long>();
      Dictionary<string, long> newStones = new Dictionary<string, long>();
      Action<Dictionary<string, long>, string, long> updateOrAdd = new Action<Dictionary<string, long>, string, long>((dict, key, val) => {
        if(dict.ContainsKey(key)) {
          dict[key] += val;
        } else {
          dict.Add(key, val);
        }
      });
      foreach(var stone in stones) {
        updateOrAdd(stoneCounts, stone, 1);
      }
      for(int i = 0; i < 75; i++) {
        newStones.Clear();
        foreach(var stoneVal in stoneCounts) {
          if(stoneVal.Key.Equals("0"))
            updateOrAdd(newStones, "1", stoneCounts[stoneVal.Key]);
          else if(stoneVal.Key.Length % 2 == 0) {
            updateOrAdd(newStones, "" + int.Parse(stoneVal.Key.Substring(0, stoneVal.Key.Length / 2)), stoneCounts[stoneVal.Key]);
            updateOrAdd(newStones, "" + int.Parse(stoneVal.Key.Substring(stoneVal.Key.Length / 2)), stoneCounts[stoneVal.Key]);
          } else {
            updateOrAdd(newStones, "" + (long.Parse(stoneVal.Key) * 2024), stoneCounts[stoneVal.Key]);
          }
        }
        stoneCounts = newStones.ToDictionary();   
      }
      Console.WriteLine(stoneCounts.Values.Sum());
    }

    private void printStones(int blinks = 0) {
      Console.WriteLine($"After {blinks} Blinks: ");
      foreach(var stone in stones) {
        Console.Write(stone + " ");
      }
      Console.WriteLine();
      Console.WriteLine($"There are {stones.Count} stones");
      Console.WriteLine();
    }
  }
}
