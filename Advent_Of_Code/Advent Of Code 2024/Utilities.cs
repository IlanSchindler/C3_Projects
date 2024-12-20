using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code_2024 {
  internal static class Utilities {

    public class Tile {
      public (int x, int y) Pos { get; set; }
      public Dictionary<Utilities.directions, Tile> Neighbors = new Dictionary<Utilities.directions, Tile> {
        {Utilities.directions.up,null },
        {Utilities.directions.down,null },
        {Utilities.directions.left,null },
        {Utilities.directions.right,null }
      };
      public bool IsStart { get; set; }
      public bool IsEnd { get; set; }


    }


    public static string InputPath { get { return @"D:\C3\Advent_Of_Code\Advent Of Code 2024\Inputs\"; } }

    public static List<T> AddReturn<T>(this List<T> collection, T item) {
      collection.Add(item);
      return collection;
    }
    public enum directions {
      up = 0,
      left = 1,
      down = 2,
      right = 3
    }

    public static directions Opposite(this directions dir) {
      switch(dir) {
        case directions.up:
          return directions.down;
        case directions.down:
          return directions.up;
        case directions.left:
          return directions.right;
        case directions.right:
          return directions.left;
      }
      throw new NotImplementedException();
    }

    public static directions Left(this directions dir) {
      switch(dir) {
        case directions.up:
          return directions.left;
        case directions.down:
          return directions.right;
        case directions.left:
          return directions.down;
        case directions.right:
          return directions.up;
      }
      throw new NotImplementedException();
    }

    public static directions Right(this directions dir) {
      switch(dir) {
        case directions.up:
          return directions.right;
        case directions.down:
          return directions.left;
        case directions.left:
          return directions.up;
        case directions.right:
          return directions.down;
      }
      throw new NotImplementedException();
    }

    public static Dictionary<directions, (int dy, int dx)> movements = new Dictionary<directions, (int, int)> {
      { directions.up , (-1,0) },
      { directions.left , (0,-1) },
      { directions.down , (1,0) },
      { directions.right , (0,1) }
    };
    public static List<string> readInputFile(string input_file) {
      return readInputFromPath(InputPath + input_file);
    }

    public static List<string> readInputFromPath(string input_path) {

      List<string> returnInput = new List<string>();

      using(StreamReader sr = File.OpenText(input_path)) {
        string inputLine = "";
        while((inputLine = sr.ReadLine()) != null) {
          returnInput.Add(inputLine);
        }
      }
      return returnInput;
    }

    public static void tryIgnore<TException>(System.Action action) where TException : Exception {
      try {
        action();
      } catch(TException e) {
      }
    }

    public struct Node {
      public int x, y;
    }

  }
}
