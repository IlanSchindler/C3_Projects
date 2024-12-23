using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day20 : Day {


    class TileD20 : Utilities.Tile {
      public int DistanceScore = -1;  

    }

    class Cheat {
      public TileD20 Start { get; set; }
      public TileD20 End { get; set; }
      public int CheatDurration { get; set; }
      public int GetCheatValue() { return Start.DistanceScore - End.DistanceScore - CheatDurration; }
    }


    List<TileD20> track;
    List<Cheat> cheats;

    public Day20() : base("Day20.txt") {
      FormatData();
    }

    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile);

      track = new List<TileD20>();
      //Load all squares into maze. Don't save any walls.
      for(int i = 0; i < input.Count; i++) {
        for(int j = 0; j < input[0].Length; j++) {
          if(input[i][j] == '#')
            continue;
          var newPath = new TileD20() { Pos = (j, i) };
          if(input[i][j] == 'E')
            newPath.IsEnd = true;
          if(input[i][j] == 'S')
            newPath.IsStart = true;
          track.Add(newPath);
        }
      }

      //Set paths to point to neighbors
      foreach(var tile in track) {
        foreach(Utilities.directions dir in Enum.GetValues(typeof(Utilities.directions))) {
          tile.Neighbors[dir] = track.FirstOrDefault(x => x.Pos.x == tile.Pos.x + Utilities.movements[dir].dx && x.Pos.y == tile.Pos.y + Utilities.movements[dir].dy);
        }
      }

    }

    public override void Part1() {


      calculateDistanceScores();
      cheats = new List<Cheat>();
      calculateCheats();

      var cheatGroups = cheats.GroupBy(x => x.GetCheatValue()).OrderBy(x => x.Key);
      foreach(var cg in cheatGroups) {
        Console.WriteLine("There are {0} cheats that save {1} picoseconds.", cg.Count(), cg.Key);
      }

      Console.WriteLine("There are {0} cheats that save at least 100 picoseconds.", cheatGroups.Where(x => x.Key >= 100).Sum(x => x.Count()));




    }


    void calculateDistanceScores() {
      List<TileD20> nextCheck = new List<TileD20> { track.First(x => x.IsEnd) };
      int step = 0;
      while(nextCheck.Count > 0) {
        int toCheck = nextCheck.Count;
        for(int i = 0; i < toCheck; i++) {
          nextCheck[i].DistanceScore = step;
          nextCheck.AddRange(nextCheck[i].Neighbors.Values.Cast<TileD20>().Where((TileD20 y) => y != null && y.DistanceScore == -1).ToList());
        }
        nextCheck.RemoveRange(0, toCheck);
        nextCheck = nextCheck.Distinct().ToList();
        step++;

      }
    }

    void calculateCheats(int cheatDistance = 2) {

      foreach(TileD20 tile in track) {
        var cheatTiles = track.Where(x => Math.Abs(x.Pos.x - tile.Pos.x) + Math.Abs(x.Pos.y - tile.Pos.y) == cheatDistance && tile.DistanceScore - x.DistanceScore > cheatDistance).ToList();
        if(cheatTiles.Count == 0)
          continue;
        foreach(TileD20 cTile in cheatTiles) {
          cheats.Add(new Cheat() { Start = tile, End = cTile, CheatDurration = cheatDistance });
        }
      }

    }



    public override void Part2() {
      cheats = new List<Cheat>();
      for(int i = 2; i <= 20; i++) {
        calculateCheats(i);

      }

      var cheatGroups = cheats.GroupBy(x => x.GetCheatValue()).OrderBy(x => x.Key);
      foreach(var cg in cheatGroups) {
        Console.WriteLine("There are {0} cheats that save {1} picoseconds.", cg.Count(), cg.Key);
      }

      Console.WriteLine("There are {0} cheats that save at least 100 picoseconds.", cheatGroups.Where(x => x.Key >= 100).Sum(x => x.Count()));
    }
  }
}
