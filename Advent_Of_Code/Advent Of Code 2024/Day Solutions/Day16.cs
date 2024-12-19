using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day16 : Day {

    class Tile {
      public (int x, int y) Pos { get; set; }
      public Dictionary<Utilities.directions, Tile> Neighbors = new Dictionary<Utilities.directions, Tile> {
        {Utilities.directions.up,null },
        {Utilities.directions.down,null },
        {Utilities.directions.left,null },
        {Utilities.directions.right,null }
      };
      public bool IsStart { get; set; }
      public bool IsEnd { get; set; }


      public List<Tile> BestLastNeighbors { get; set; }

      public List<Tile> BuildBestLastPath() {
        if(this.IsStart)
          return new List<Tile>() { this };
        return this.BestLastNeighbors[0].BuildBestLastPath().AddReturn(this);
      }


    }



    List<Tile> maze;


    public Day16() : base("Day16.txt") {
      FormatData();
    }

    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile);
      maze = new List<Tile>();
      //Load all squares into maze. Don't save any walls.
      for(int i = 0; i < input.Count; i++) {
        for(int j = 0; j < input[0].Length; j++) {
          if(input[i][j] == '#')
            continue;
          var newPath = new Tile() { Pos = (j, i) };
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


      List<Tile> nextChecks = new List<Tile> { maze.First(x => x.IsStart) };
      while(nextChecks.Count > 0) {


        int checkCount = nextChecks.Count;
        for(int i = 0; i < checkCount; i++) {

          foreach(var neighbor in nextChecks[i].Neighbors.Values.Where(x => x != null).ToList()) {

            if(neighbor.BestLastNeighbors == null) {
              neighbor.BestLastNeighbors = new List<Tile>() { nextChecks[i] };
              nextChecks.Add(neighbor);
            } else if(neighbor.BestLastNeighbors.Contains(nextChecks[i]) || nextChecks[i].BestLastNeighbors.Contains(neighbor)) {
              continue;
            } else {
              var curBestPath = neighbor.BuildBestLastPath();
              var curBestScore = getPathScore(curBestPath);
              var testPath = nextChecks[i].BuildBestLastPath().AddReturn(neighbor);
              var testScore = getPathScore(testPath);

              if(curBestScore == testScore) {
                neighbor.BestLastNeighbors.Add(nextChecks[i]);
              } else {
                if(Math.Abs(curBestScore - testScore) == 1000) {
                  var nextNeighbors = neighbor.Neighbors.Values.Where(x => x != null && !curBestPath.Contains(x) && !testPath.Contains(x)).ToList();
                  
                  if(nextNeighbors.Any(x => getPathScore(curBestPath.ToList().AddReturn(x)) == getPathScore(testPath.ToList().AddReturn(x)))) {
                    neighbor.BestLastNeighbors.Add(nextChecks[i]);
                    continue;
                  }
                }
                if(curBestScore > testScore) {
                  nextChecks.Add(neighbor);
                  neighbor.BestLastNeighbors.Clear();
                  neighbor.BestLastNeighbors.Add(nextChecks[i]);
                }
              }
            }
          }

        }
        nextChecks.RemoveRange(0, checkCount);
      }
      
            drawMaze();
      var allPaths = new List<List<Tile>>();
      findAllPaths(new List<Tile>(), maze.First(x => x.IsEnd), allPaths);
      int bestScore = allPaths.Select(x => getPathScore(x)).Min();
      var bestPaths = allPaths.Where(x => getPathScore(x) == bestScore).ToList();
      foreach(var p in bestPaths) {
        drawPath(p);
      }
      
      Console.SetCursorPosition(0, maze.Select(x => x.Pos.y).Max() + 2);
      Console.WriteLine("Best Score: {0}", bestScore);
      //Part 2 solution
      Console.WriteLine("Possible Tiles: {0}", bestPaths.SelectMany(x=>x).Distinct().Count());

    }


    private void findAllPaths(List<Tile> path, Tile nextNode, List<List<Tile>> paths) {
      path.Add(nextNode);
      if(nextNode.IsStart) {
        paths.Add(path.ToList());
        path.Remove(nextNode);
        return;
      }
      foreach(var node in nextNode.BestLastNeighbors) {

        findAllPaths(path, node, paths);

      }
      path.Remove(nextNode);
      return;

    }

    private void drawMaze() {
      Console.SetCursorPosition(0, 0);
      for(int i = 0; i < maze.Select(x => x.Pos.y).Max() + 2; i++) {
        for(int j = 0; j < maze.Select(x => x.Pos.x).Max() + 2; j++) {
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
          Console.Write(" ");
      }
    }

    private int getPathScore(List<Tile> path, bool ignoreFirstTurn = false) {
      int turncount = 0;
      if(path[0].IsEnd || path[path.Count - 1].IsStart) {
        path.Reverse();
      }
      if(!ignoreFirstTurn && path[0].Neighbors.First(x => x.Value == path[1]).Key != Utilities.directions.right) { turncount++; }
      for(int i = 1; i < path.Count - 1; i++) {
        if(path[i].Neighbors.First(x => x.Value == path[i + 1]).Key != path[i].Neighbors.First(x => x.Value == path[i - 1]).Key.Opposite())
          turncount++;
      }
      return path.Count - 1 + turncount * 1000;
    }




    private void drawPath(List<Tile> path, char c = 'X') {
      foreach(var n in path) {
        Console.SetCursorPosition(n.Pos.x, n.Pos.y);
        Console.Write("" + c);
      }
    }

    public override void Part2() {
      //Implemented at end of Part 1

    }

  }
}
