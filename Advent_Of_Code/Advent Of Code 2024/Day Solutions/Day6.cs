using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day6 : Day {

    private char[,] map;
    private (int, int) currentPos = (0, 0);
    private int currentDir = 0;
    private (int, int)[] moves = [(-1, 0), (0, 1), (1, 0), (0, -1)];
    private char[] guardToken = ['^', '>', 'V', '<'];
    private int mapW, mapH;




    public Day6() : base("Day6.txt") {

      FormatData();
    }

    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile);
      mapW = input.Count;
      mapH = input[0].Length;
      map = new char[mapW, mapH];
      for(int i = 0; i < mapW; i++) {
        for(int j = 0; j < mapH; j++) {
          map[i, j] = input[i][j];
          if(guardToken.Contains(input[i][j])) {
            currentPos = (i, j);
            currentDir = Array.IndexOf(guardToken, input[i][j]);
          }
        }
      }
    }

    public override void Part1() {
      FormatData();
      int xCount = 1;
      (int, int) nextPos = (currentPos.Item1 + moves[currentDir].Item1, currentPos.Item2 + moves[currentDir].Item2);
      while(nextPos.Item1 >= 0 && nextPos.Item1 < mapW && nextPos.Item2 >= 0 && nextPos.Item2 < mapH) {
        switch(map[nextPos.Item1, nextPos.Item2]) {
          case '.':
            xCount++;
            map[nextPos.Item1, nextPos.Item2] = guardToken[currentDir];
            map[currentPos.Item1, currentPos.Item2] = 'X';
            currentPos = nextPos;
            break;
          case 'X':
            map[nextPos.Item1, nextPos.Item2] = guardToken[currentDir];
            map[currentPos.Item1, currentPos.Item2] = 'X';
            currentPos = nextPos;
            break;
          case '#':
            currentDir = (currentDir + 1) % 4;
            map[currentPos.Item1, currentPos.Item2] = guardToken[currentDir];
            break;
        }
        nextPos = (currentPos.Item1 + moves[currentDir].Item1, currentPos.Item2 + moves[currentDir].Item2);
      }
      printMap(xCount);

    }

    private void printMap(int xCount) {
      Console.Clear();
      Console.WriteLine($"Visited Spaces: {xCount}");
      for(int i = 0; i < mapW; i++) {
        for(int j = 0; j < mapH; j++) {
          Console.Write(map[i, j]);
        }
        Console.WriteLine();
      }
      System.Threading.Thread.Sleep(1000);

    }

    public override void Part2() {
      FormatData();
      int numObs = 1;
      foreach(var x in map) {
        if(x == '#')
          numObs++;
      }
      int numTurns = 0;
      int maxTurns = numObs * 4;
      int validLoop = 0;
      for(int iO = 0; iO < mapW; iO++) {
        for(int jO = 0; jO < mapH; jO++) {
          FormatData();
          numTurns = 0;
          if(map[iO, jO] == '#') continue;
          map[iO, jO] = '#';
          (int, int) nextPos = (currentPos.Item1 + moves[currentDir].Item1, currentPos.Item2 + moves[currentDir].Item2);
          while(nextPos.Item1 >= 0 && nextPos.Item1 < mapW && nextPos.Item2 >= 0 && nextPos.Item2 < mapH) {
            int nextDir = (currentDir + 1) % 4;               
            switch(map[nextPos.Item1, nextPos.Item2]) {
              case '.':
              case 'X':
                map[nextPos.Item1, nextPos.Item2] = guardToken[currentDir];
                map[currentPos.Item1, currentPos.Item2] = 'X';
                currentPos = nextPos;
                break;
              case '#':
                currentDir = (currentDir + 1) % 4;
                map[currentPos.Item1, currentPos.Item2] = guardToken[currentDir];
                numTurns++;
                break;
            }
            nextPos = (currentPos.Item1 + moves[currentDir].Item1, currentPos.Item2 + moves[currentDir].Item2);
            if(numTurns > maxTurns) {
              validLoop++;
              break;
            }
            //printMap(validLoop);
          }
        }
      }

      printMap(validLoop);

    }
  }
}
