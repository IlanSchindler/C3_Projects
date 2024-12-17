using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code_2024 {
  internal static class Utilities {
    public static string InputPath { get { return @"D:\C3\Advent_Of_Code\Advent Of Code 2024\Inputs\"; } }
    public enum directions {
      up = 0, left = 1, down = 2, right = 3
    }
    public static Dictionary<directions, (int , int )> movements = new Dictionary<directions, (int, int)> {
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
