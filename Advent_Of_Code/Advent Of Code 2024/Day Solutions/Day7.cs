using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day7 : Day {

    private List<List<BigInteger>> data;
    private char[] operators;

    public Day7() : base("Day7.txt") {
      FormatData();
    }

    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile);
      data = new List<List<BigInteger>>();
      foreach(var item in input) {
        var split = item.Split(' ');
        List<BigInteger> line = new List<BigInteger>();

        split[0] = split[0].Remove(split[0].Length - 1);
        foreach(var s in split) {
          line.Add(BigInteger.Parse(s));
        }
        data.Add(line);
      }

    }

    public override void Part1() {
      operators = new char[] { '+', '*' };
      BigInteger sum = findValidExpressionSum();
      Console.WriteLine($"Valid Sum: {sum}");
    }

    public override void Part2() {
      operators = new char[] { '+', '*', '|' };
      BigInteger sum = findValidExpressionSum();
      Console.WriteLine($"Valid Sum: {sum}");
    }

    private BigInteger findValidExpressionSum() {
      BigInteger sum = 0;
      foreach(var item in data) {
        BigInteger target = item[0];
        List<BigInteger> curEvals = new List<BigInteger> { item[1] };
        foreach(var v in item.TakeLast(item.Count - 2)) {
          List<BigInteger> nextEvals = new List<BigInteger>();
          foreach(var e in curEvals) {
            foreach(var o in operators) {
              switch(o) {
                case '+':
                  nextEvals.Add(e + v);
                  break;
                case '*':
                  nextEvals.Add(e * v);
                  break;
                case '|':
                  nextEvals.Add(e* ((int)Math.Pow(10,1+Math.Floor(BigInteger.Log10(v)))) + v);
                  break;
              }

            }
          }
          curEvals = nextEvals.ToList();

        }
        if(curEvals.Contains(target)) {
          sum += target;
        }
      }
      return sum;





    }
  }
}
