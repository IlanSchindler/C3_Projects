using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Advent_Of_Code_2024.Utilities;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day18 : Day {


    class TileD18 : Utilities.Tile {
      public int DistanceScore = -1;

      public void Corrupt() {
        foreach(Utilities.directions dir in Enum.GetValues(typeof(Utilities.directions))) {
          if(this.Neighbors[dir] == null)
            continue;
          this.Neighbors[dir].Neighbors[dir.Opposite()] = null;
          this.Neighbors[dir] = null;
        }
      }

      public TileD18 GetBestNeighbor() {
        if(this.IsEnd)
          return null;
        return this.Neighbors.Values.Cast<TileD18>().Where(x => x != null).MinBy(x => x.DistanceScore);
      }

      public List<TileD18> GetBestPath() {
        if(this.IsEnd)
          return new List<TileD18> { this };
        return this.GetBestNeighbor().GetBestPath().AddReturn(this);
      }

    }

    int memSpaceSize = 0;
    int bytesToCheck = 0;
    List<(int x, int y)> fallingBytes;
    List<TileD18> memSpace;

    public Day18() : base("Day18.txt") {
      FormatData();
    }
    public override void FormatData() {
      /*
       *  EXTREMELY IMPORTANT NOTE
       *  
       *  I made a small change to the input data to simplify some things. 
       *  The first line of the data now specifies the size of the space the problem occurs in.
       *  The second line of the data now specifies how many bytes should be allowed to fall before checking for a path in part 1.
       *  This number is also used as a starting point from which to continue letting bytes fall in part 2.
       *  The rest of the data is unchanged.
       *  
       *  I did this so that when testing, switching back and forth between the sample data and actual data required only changing the name of the target input file 
       *    rather than the target input file AND 2 other values I needed to keep track of.
       */
      var input = Utilities.readInputFile(inputFile);

      memSpaceSize = int.Parse(input[0]);
      input.RemoveAt(0);
      bytesToCheck = int.Parse(input[0]);
      input.RemoveAt(0);

      memSpace = new List<TileD18>();
      //Load all squares into maze. Don't save any walls.
      for(int i = 0; i < memSpaceSize + 1; i++) {
        for(int j = 0; j < memSpaceSize + 1; j++) {
          var newPath = new TileD18() { Pos = (j, i) };
          memSpace.Add(newPath);
        }
      }

      //Set paths to point to neighbors
      foreach(var tile in memSpace) {
        foreach(Utilities.directions dir in Enum.GetValues(typeof(Utilities.directions))) {
          tile.Neighbors[dir] = memSpace.FirstOrDefault(x => x.Pos.x == tile.Pos.x + Utilities.movements[dir].dx && x.Pos.y == tile.Pos.y + Utilities.movements[dir].dy);
        }
      }

      memSpace.First(x => x.Pos.x == 0 && x.Pos.y == 0).IsStart = true;
      memSpace.First(x => x.Pos.x == memSpaceSize && x.Pos.y == memSpaceSize).IsEnd = true;

      fallingBytes = new List<(int x, int y)>();
      foreach(var line in input) {
        fallingBytes.Add((int.Parse(line.Split(',')[0]), int.Parse(line.Split(',')[1])));
      }

    }

    public override void Part1() {
      //drawSpace();
      for(int i = 0; i < bytesToCheck; i++) {
        var bytetile = memSpace.FirstOrDefault(x => x.Pos.x == fallingBytes[i].x && x.Pos.y == fallingBytes[i].y);
        if(bytetile != null) {
          bytetile.Corrupt();
          memSpace.Remove(bytetile);
          //Console.SetCursorPosition(bytetile.Pos.x, bytetile.Pos.y);
          //Console.Write("#");
        }
      }

      calculateDistanceScores();


      List<TileD18> bestPath = memSpace.First(x => x.IsStart).GetBestPath();
      //foreach(var tile in bestPath) {
      //  Console.SetCursorPosition(tile.Pos.x, tile.Pos.y);
      //  Console.Write("0");
      //}
      //Console.SetCursorPosition(0, memSpaceSize + 3);
      Console.WriteLine("Path Size: {0}", bestPath.Count - 1);


    }

    void drawSpace() {
      foreach(TileD18 tile in memSpace) {
        Console.SetCursorPosition(tile.Pos.x, tile.Pos.y);
        Console.Write(".");
      }
    }

    void calculateDistanceScores() {
      List<TileD18> nextCheck = new List<TileD18> { memSpace.First(x => x.IsEnd) };
      int step = 0;
      while(nextCheck.Count > 0) {
        int toCheck = nextCheck.Count;
        for(int i = 0; i < toCheck; i++) {
          nextCheck[i].DistanceScore = step;
          nextCheck.AddRange(nextCheck[i].Neighbors.Values.Cast<TileD18>().Where((TileD18 y) => y != null && y.DistanceScore == -1).ToList());
        }
        nextCheck.RemoveRange(0, toCheck);
        nextCheck = nextCheck.Distinct().ToList();
        step++;

      }
    }
    void resetDistanceScores() {
      foreach(var tile in memSpace)
        tile.DistanceScore = -1;
    }

    public override void Part2() {
      int i = bytesToCheck;

      while(memSpace.First(x => x.IsStart).DistanceScore != -1) {
        //drawSpace();
        resetDistanceScores();
        var bytetile = memSpace.FirstOrDefault(x => x.Pos.x == fallingBytes[i].x && x.Pos.y == fallingBytes[i].y);
        if(bytetile != null) {
          bytetile.Corrupt();
          memSpace.Remove(bytetile);
          //Console.SetCursorPosition(bytetile.Pos.x, bytetile.Pos.y);
          //Console.Write("#");
        }
        calculateDistanceScores();
        i++;
      }
      i--;

      //Console.SetCursorPosition(0, memSpaceSize + 5);
      Console.WriteLine("First Byte To Fail: ({0},{1})", fallingBytes[i].x, fallingBytes[i].y);
      Console.WriteLine("First Byte To Fail Index: {0}", i);
    }
  }
}
