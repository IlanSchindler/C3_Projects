using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day16 : Day {

    class path {
      public (int x, int y) Pos { get; set; }
      public Dictionary<Utilities.directions, path> Neighbors = new Dictionary<Utilities.directions, path> {
        {Utilities.directions.up,null },
        {Utilities.directions.down,null },
        {Utilities.directions.left,null },
        {Utilities.directions.right,null }
      };
      public bool IsStart { get; set; }
      public bool IsEnd { get; set; }
      //public Utilities.directions bestDir { get; set; }

     /* public List<path> buildBestPath() {
        if(this.IsEnd) return new List<path>() { this };
        return this.Neighbors[this.bestDir].buildBestPath().AddReturn(this);
      }*/
    }



    List<path> maze;


    public Day16() : base("sample.txt") {
      FormatData();
    }

    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile);
      maze = new List<path>();
      //Load all squares into maze. Don't save any walls.
      for(int i = 0; i < input.Count; i++) {
        for(int j = 0; j < input[0].Length; j++) {
          if(input[i][j] == '#')
            continue;
          var newPath = new path() { Pos = (j, i) };
          if(input[i][j] == 'E')
            newPath.IsEnd = true;
          if(input[i][j] == 'S')
            newPath.IsStart = true;
          maze.Add(newPath);
        }
      }

      //Set paths to point to neighbors
      foreach(var path in maze) {
        foreach(Utilities.directions dir in Enum.GetValues(typeof(Utilities.directions))) {
          path.Neighbors[dir] = maze.FirstOrDefault(x => x.Pos.x == path.Pos.x + Utilities.movements[dir].dx && x.Pos.y == path.Pos.y + Utilities.movements[dir].dy);
        }
      }

      //recursively remove dead ends
      while(maze.Count(x => !x.IsStart && !x.IsEnd && x.Neighbors.Values.Count(v => v != null) == 1) > 0) {
        var toRemove = maze.Where(x => !x.IsStart && !x.IsEnd && x.Neighbors.Values.Count(y => y != null) == 1).ToList();
        foreach(var remove in toRemove) {
          var n = remove.Neighbors.First(x => x.Value != null);
          n.Value.Neighbors[n.Key.Opposite()] = null;
          remove.Neighbors[n.Key] = null;
          maze.Remove(remove);
        }
      }
    }

    public override void Part1() {
      //List<path> path = new List<path>();
      int maxY = maze.Select(x => x.Pos.y).Max() + 2;
      int maxX = maze.Select(x => x.Pos.x).Max() + 2;
      List<List<path>> paths = new List<List<path>>();
      drawMaze();
      traverse(new List<path>(), maze.First(x => x.IsStart), paths);
      foreach(var path in paths.OrderBy(x=>getPathScore(x))) {
        drawMaze();
        foreach(var p in path) {
          Console.SetCursorPosition(p.Pos.x, p.Pos.y);
          Console.Write("X");
        }
        Console.SetCursorPosition(maxX + 3, 0);
        Console.Write("{0,-10}",getPathScore(path));
      }
      Console.WriteLine();
    }

    private void drawMaze() {
      Console.SetCursorPosition(0, 0);
      for(int i = 0; i < maze.Select(x=>x.Pos.y).Max()+2; i++) {
                                               for(int j = 0; j < maze.Select(x=>x.Pos.x).Max()+2; j++) {
          Console.Write("#");
        }
        Console.WriteLine();
      }
      foreach(var p in maze) {
        Console.SetCursorPosition(p.Pos.x, p.Pos.y);
        if(p.IsStart)
          Console.Write("S");
        else if(p.IsEnd)
          Console.Write("E");
        else
          Console.Write(".");
      }
    }

    private int getPathScore(List<path> path) {
      int turncount = 0;
      if(path[0].Neighbors.First(x => x.Value == path[1]).Key != Utilities.directions.right) { turncount++; }
      for(int i = 1; i < path.Count-1; i++) {
        if(path[i].Neighbors.First(x => x.Value == path[i+1]).Key != path[i].Neighbors.First(x => x.Value == path[i-1]).Key.Opposite())
          turncount++;
      }
      return path.Count - 1 + turncount * 1000;
    }

    private void traverse(List<path> path, path nextNode, List<List<path>> paths) {
      path.Add(nextNode);
      if(nextNode.IsEnd) {
        paths.Add(path.ToList());
        path.Remove(nextNode);
        return;
      }
      foreach(var node in nextNode.Neighbors) {
        if(node.Value == null)
          continue;
        if(path.Contains(node.Value))
          continue;
        traverse(path, node.Value, paths);

      }
      path.Remove(nextNode);
      return;

    }

    public override void Part2() {
      throw new NotImplementedException();
    }

  }
}
