using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day14 : Day {

    class robot {
      public (int, int) pos { get; set; }
      public (int, int) vel { get; set; }
    }

    List<robot> bots;
    int mapW = 101;
    int mapH = 103;
    int midW { get { return mapW / 2 ; } }
    int midH { get { return mapH / 2 ; } }

    public Day14() : base("Day14.txt") {
      FormatData();
    }
    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile);
      bots = new List<robot>();
      foreach(var line in input) {
        var splitLine = line.Split([' ', '=', ',']);
        bots.Add(new robot() { pos = (int.Parse(splitLine[1]), int.Parse(splitLine[2])), vel = (int.Parse(splitLine[4]), int.Parse(splitLine[5])) });
      }
      printMap();

    }

    public override void Part1() {
      int sCount = 100;
      for(int i = 0; i < bots.Count; i++) {
        bots[i].pos = (bots[i].pos.Item1 + bots[i].vel.Item1 * sCount, bots[i].pos.Item2 + bots[i].vel.Item2 * sCount);
        bots[i].pos = (((bots[i].pos.Item1 % mapW) + mapW) % mapW, ((bots[i].pos.Item2 % mapH) + mapH) % mapH);
      }
      int[] quadCounts = new int[4] {
        bots.Count(x=>x.pos.Item1 < midW && x.pos.Item2 < midH),
        bots.Count(x=>x.pos.Item1 > midW && x.pos.Item2 < midH),
        bots.Count(x=>x.pos.Item1 < midW && x.pos.Item2 > midH),
        bots.Count(x=>x.pos.Item1 > midW && x.pos.Item2 > midH)
      };
      printMap();
      printQuads();
      for(int i = 0; i < quadCounts.Length; i++) { 
      Console.WriteLine("Q{0}: {1}",i+1,quadCounts[i]);
      }
      Console.WriteLine("Safety Factor: {0}",quadCounts.Aggregate(1, (x, y) => x * y));
    }

    public override void Part2() {
      FormatData();
      int itt = 0;
      while(true) {
        itt++;
        //Console.Clear();
               int sCount = 1;
      for(int i = 0; i < bots.Count; i++) {
        bots[i].pos = (bots[i].pos.Item1 + bots[i].vel.Item1 * sCount, bots[i].pos.Item2 + bots[i].vel.Item2 * sCount);
        bots[i].pos = (((bots[i].pos.Item1 % mapW) + mapW) % mapW, ((bots[i].pos.Item2 % mapH) + mapH) % mapH);
      }
        //   printMap();
        //Console.WriteLine(bots.Select(x => x.pos).Distinct().Count() == bots.Count);
        if(bots.Select(x => x.pos).Distinct().Count() == bots.Count) {
          printMap();
          Console.WriteLine(itt);
          break;
        }
      }
    }

    private void printMap() {
      Console.WriteLine();
      int botcount = 0;
      for(int i = 0; i < mapH; i++) {
        for(int j = 0; j < mapW; j++) {
          botcount = bots.Where(x => x.pos.Item1 == j && x.pos.Item2 == i).Count();
          Console.Write("" + ((botcount == 0) ? '.' : botcount.ToString()));
        }
        Console.WriteLine();
      }
    }
    private void printQuads() {
      Console.WriteLine();
      int botcount = 0;
      for(int i = 0; i < mapH; i++) {
        for(int j = 0; j < mapW; j++) {
          if(i == midH || j == midW) {
            Console.Write(" ");
            continue;
          }
          botcount = bots.Where(x => x.pos.Item1 == j && x.pos.Item2 == i).Count();
          Console.Write("" + ((botcount == 0) ? '.' : botcount.ToString()));
        }
        Console.WriteLine();
      }
    }
  }
}
