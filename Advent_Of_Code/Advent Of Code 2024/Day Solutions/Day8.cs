using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day8 : Day {


    Dictionary<char, List<Utilities.Node>> antenae;
    char[,] map;
    int mapW { get { return map.GetLength(0); } }
    int mapH { get { return map.GetLength(1); } }

    public Day8() : base("Day8.txt") {
      FormatData();
    }

    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile);
      antenae = new Dictionary<char, List<Utilities.Node>>();
      map = new char[input.Count, input[0].Length];
      for(int i = 0; i < mapW; i++) {
        for(int j = 0; j < mapH; j++) {
          if(input[i][j] == '.')
            continue;

          if(!antenae.ContainsKey(input[i][j]))
            antenae.Add(input[i][j], new List<Utilities.Node>());

          antenae[input[i][j]].Add(new Utilities.Node { x = i, y = j });
        }
      }
    }

    public override void Part1() {
      resetMap();
      foreach(var k in antenae.Keys) {
        for(int i = 0; i < antenae[k].Count; i++) {
          for(int j = i + 1; j < antenae[k].Count; j++) {
            Utilities.tryIgnore<IndexOutOfRangeException>(() => map[antenae[k][j].x + (antenae[k][j].x - antenae[k][i].x), antenae[k][j].y + (antenae[k][j].y - antenae[k][i].y)] = '#');
            Utilities.tryIgnore<IndexOutOfRangeException>(() => map[antenae[k][i].x - (antenae[k][j].x - antenae[k][i].x), antenae[k][i].y - (antenae[k][j].y - antenae[k][i].y)] = '#');
          }
        }
      }
      int antinodes = getAnitnodeCount();
      Console.WriteLine($"Antinode Count: {antinodes}");
    }

    public override void Part2() {
      resetMap();
      foreach(var k in antenae.Keys) {
        for(int i = 0; i < antenae[k].Count; i++) {
          for(int j = i + 1; j < antenae[k].Count; j++) {
            for(int n = 0; true; n++) {
              try { map[antenae[k][j].x + (antenae[k][j].x - antenae[k][i].x) * n, antenae[k][j].y + (antenae[k][j].y - antenae[k][i].y) * n] = '#'; } catch(IndexOutOfRangeException e) { break; }
            }
            for(int n = 0; true; n++) {
              try { map[antenae[k][i].x - (antenae[k][j].x - antenae[k][i].x) * n, antenae[k][i].y - (antenae[k][j].y - antenae[k][i].y) * n] = '#'; } catch(IndexOutOfRangeException e) { break; }
            }
          }
        }
      }
      int antinodes = getAnitnodeCount();
      Console.WriteLine($"Antinode Count: {antinodes}");
    }

    private void resetMap() {
      for(int i = 0; i < mapW; i++) {
        for(int j = 0; j < mapH; j++) {
          map[i, j] = '0';

        }
      }
    }
    private int getAnitnodeCount() {
      int count = 0;
      for(int i = 0; i < mapW; i++) {
        for(int j = 0; j < mapH; j++) {
          if(map[i, j] == '#')
            count++;
        }
      }
      return count;
    }

  }
}
