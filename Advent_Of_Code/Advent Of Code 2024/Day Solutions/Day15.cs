using FParsec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advent_Of_Code_2024;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day15 : Day {

    enum space {
      none = '.',
      wall = '#',
      crate = 'O',
      bot = '@',
      crateL = '[',
      crateR = ']',
      botUp = '^',
      botDown = 'v',
      botLeft = '<',
      botRight = '>'
    }

    space[,] map;
    space[,] map2;
    List<Utilities.directions> moves;
    (int I, int J) curPos = (0, 0);
    (int I, int J) cPos2 = (0, 0);

    public Day15() : base("Day15.txt") {
      FormatData();
    }

    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile);
      var inMap = input.Where(x => x.Contains('#')).ToList();
      map = new space[inMap[0].Length, inMap.Count];
      map2 = new space[inMap[0].Length, inMap.Count * 2];
      for(int i = 0; i < map.GetLength(0); i++) {
        for(int j = 0; j < map.GetLength(1); j++) {
          map[i, j] = (space)inMap[i][j];
          switch((space)inMap[i][j]) {
            case space.none:
            case space.wall:
              map2[i, 2 * j] = (space)inMap[i][j];
              map2[i, 2 * j + 1] = (space)inMap[i][j];
              break;
            case space.bot:
              map2[i, 2 * j] = (space)inMap[i][j];
              map2[i, 2 * j + 1] = space.none;
              cPos2 = (i, 2 * j);
              break;
            case space.crate:
              map2[i, 2 * j] = space.crateL;
              map2[i, 2 * j + 1] = space.crateR;
              break;
          }
          if(map[i, j] == space.bot)
            curPos = (i, j);
        }
      }
      moves = new List<Utilities.directions>();
      foreach(var line in input.Where(x => !x.Contains('#')).ToList()) {
        foreach(char c in line) {
          switch(c) {
            case '^':
              moves.Add(Utilities.directions.up);
              break;
            case '>':
              moves.Add(Utilities.directions.right);
              break;
            case 'v':
              moves.Add(Utilities.directions.down);
              break;
            case '<':
              moves.Add(Utilities.directions.left);
              break;
          }
        }
      }

    }

    public override void Part1() {
      //FormatData();
      //printMap();
      foreach(var dir in moves) {
        if(tryMove(map, curPos, dir)) {
          makeMove(map, curPos, dir);
          curPos = (curPos.I + Utilities.movements[dir].Item1, curPos.J + Utilities.movements[dir].Item2);
        }
        // Console.WriteLine("Move: {0}", dir);
      }
      // printMap();
      Console.WriteLine(getBoxGPSSum(map));
    }

    public override void Part2() {
      /*//FormatData();
      printMap(map2);
      space[,] lastMap = map2.Clone() as space[,];
              */
      foreach(var dir in moves) {
        /*
        Console.SetCursorPosition(100, 0);
        Console.Write($"{dir}     ");
        switch(dir) {
          case Utilities.directions.up:
            map2[cPos2.I, cPos2.J] = space.botUp;
            break;
          case Utilities.directions.right:
            map2[cPos2.I, cPos2.J] = space.botRight;
            break;
          case Utilities.directions.down:
            map2[cPos2.I, cPos2.J] = space.botDown;
            break;
          case Utilities.directions.left:
            map2[cPos2.I, cPos2.J] = space.botLeft;
            break;

        }
        updateMap(lastMap, map2); */
        if(tryMove(map2, cPos2, dir)) {
          makeMove(map2, cPos2, dir);
          cPos2 = (cPos2.I + Utilities.movements[dir].Item1, cPos2.J + Utilities.movements[dir].Item2);
        }
        //updateMap(lastMap, map2);
        //lastMap = map2.Clone() as space[,];
        //  Console.WriteLine("Move: {0}", dir);
        //printMap(map2);
        //1486658
        //1551061
      }
      //Console.SetCursorPosition(0,50);
      Console.WriteLine(getBoxGPSSum(map2));
    }

    void printMap(space[,] inMap) {
      Console.Clear();
      for(int i = 0; i < inMap.GetLength(0); i++) {
        for(int j = 0; j < inMap.GetLength(1); j++) {
          Console.Write((char)inMap[i, j]);
        }
        Console.WriteLine();
      }
      Console.WriteLine();
    }

    void updateMap(space[,] lastMap, space[,] inMap) {
      for(int i = 0; i < inMap.GetLength(0); i++) {
        for(int j = 0; j < inMap.GetLength(1); j++) {
          if(lastMap[i, j] != inMap[i, j]) {
            Console.SetCursorPosition(j, i);
            Console.Write((char)inMap[i, j]);
          }
        }

      }
      Console.SetCursorPosition(110, 0);

    }



    long getBoxGPSSum(space[,] inMap) {
      long sum = 0;
      for(int i = 0; i < inMap.GetLength(0); i++) {
        for(int j = 0; j < inMap.GetLength(1); j++) {
          if(inMap[i, j] == space.crate || inMap[i, j] == space.crateL)
            sum = sum + i * 100 + j;
        }
      }
      return sum;
    }


    bool tryMove(space[,] inMap, (int I, int J) pos, Utilities.directions dir, bool ignoreNextTo = false) {
      if(inMap[pos.I, pos.J] == space.none)
        return true;
      else if(inMap[pos.I, pos.J] == space.wall)
        return false;
      else if((dir == Utilities.directions.up || dir == Utilities.directions.down) && inMap[pos.I, pos.J] == space.crateL) {
        if(tryMove(inMap, (pos.I + Utilities.movements[dir].Item1, pos.J + Utilities.movements[dir].Item2), dir) && (tryMove(inMap, (pos.I + Utilities.movements[dir].Item1, pos.J + 1 + Utilities.movements[dir].Item2), dir, true))) {
          
          return true;
        }
      } else if((dir == Utilities.directions.up || dir == Utilities.directions.down) && inMap[pos.I, pos.J] == space.crateR) {
        if(tryMove(inMap, (pos.I + Utilities.movements[dir].Item1, pos.J + Utilities.movements[dir].Item2), dir) && (tryMove(inMap, (pos.I + Utilities.movements[dir].Item1, pos.J - 1 + Utilities.movements[dir].Item2), dir, true))) {
          
          return true;
        }
      } else if(tryMove(inMap, (pos.I + Utilities.movements[dir].Item1, pos.J + Utilities.movements[dir].Item2), dir)) {
        return true;
      }
      return false;
    }

    bool makeMove       (space[,] inMap, (int I, int J) pos, Utilities.directions dir, bool ignoreNextTo = false) {
            if(inMap[pos.I, pos.J] == space.none)
        return true;
      else if(inMap[pos.I, pos.J] == space.wall)
        return false;
      else if((dir == Utilities.directions.up || dir == Utilities.directions.down) && inMap[pos.I, pos.J] == space.crateL) {
        if(makeMove(inMap, (pos.I + Utilities.movements[dir].Item1, pos.J + Utilities.movements[dir].Item2), dir) && (makeMove(inMap, (pos.I + Utilities.movements[dir].Item1, pos.J + 1 + Utilities.movements[dir].Item2), dir, true))) {
         
          inMap[pos.I + Utilities.movements[dir].Item1, pos.J + Utilities.movements[dir].Item2] = inMap[pos.I, pos.J];
          inMap[pos.I, pos.J] = space.none;
          //if(!ignoreNextTo) {
          inMap[pos.I + Utilities.movements[dir].Item1, pos.J + 1 + Utilities.movements[dir].Item2] = inMap[pos.I, pos.J + 1];
          inMap[pos.I, pos.J + 1] = space.none;
          //}*/
          return true;
        }
      } else if((dir == Utilities.directions.up || dir == Utilities.directions.down) && inMap[pos.I, pos.J] == space.crateR) {
        if(makeMove(inMap, (pos.I + Utilities.movements[dir].Item1, pos.J + Utilities.movements[dir].Item2), dir) && (makeMove(inMap, (pos.I + Utilities.movements[dir].Item1, pos.J - 1 + Utilities.movements[dir].Item2), dir, true))) {
          
          
          inMap[pos.I + Utilities.movements[dir].Item1, pos.J + Utilities.movements[dir].Item2] = inMap[pos.I, pos.J];
          inMap[pos.I, pos.J] = space.none;

          //if(!ignoreNextTo) {
          inMap[pos.I + Utilities.movements[dir].Item1, pos.J - 1 + Utilities.movements[dir].Item2] = inMap[pos.I, pos.J - 1];
          inMap[pos.I, pos.J - 1] = space.none;
          //}
          return true;
        }
      } else if(makeMove(inMap, (pos.I + Utilities.movements[dir].Item1, pos.J + Utilities.movements[dir].Item2), dir)) {
        inMap[pos.I + Utilities.movements[dir].Item1, pos.J + Utilities.movements[dir].Item2] = inMap[pos.I, pos.J];
        inMap[pos.I, pos.J] = space.none;
        return true;
      }
      return false;
    }

  }
}
