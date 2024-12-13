using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Symbolics;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Complex;
using MathNet.Numerics.LinearAlgebra;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day13 : Day {

    struct machineData {
      public double Ax, Ay, Bx, By, PX, PY;
    }
    List<machineData> data;
    const double EPSILON = 1e-4;

    public Day13() : base("Day13.txt") {
    }

    public override void FormatData(){
      FormatData(0);
    }

    public void FormatData(double adjustment) {
      var input = Utilities.readInputFile(inputFile);
      data = new List<machineData>();
      for(int i = 0; i < input.Count; i += 4) {
        machineData thisData = new machineData();
        thisData.Ax = int.Parse(input[i].Split(['+', ','])[1]);
        thisData.Ay = int.Parse(input[i].Split(['+', ','])[3]);
        thisData.Bx = int.Parse(input[i + 1].Split(['+', ','])[1]);
        thisData.By = int.Parse(input[i + 1].Split(['+', ','])[3]);
        thisData.PX = int.Parse(input[i + 2].Split(['=', ','])[1]) + adjustment;
        thisData.PY = int.Parse(input[i + 2].Split(['=', ','])[3]) + adjustment;
        data.Add(thisData);
      }
    }

    public override void Part1() {
      FormatData();
      FindTotalCost();
      
    }

    public override void Part2() {
      FormatData(1e13);
      FindTotalCost();
    }

    private void FindTotalCost() {
            double cost = 0;
      foreach(machineData d in data) {
        var A = Matrix<double>.Build.DenseOfArray(new double[,] {
          { d.Ax , d.Bx },
          { d.Ay , d.By }
        });
        var b = Vector<double>.Build.Dense(new double[] { d.PX, d.PY });
        var x = A.Solve(b);
        if(Math.Abs(x[0] - Math.Round(x[0])) < EPSILON && Math.Abs(x[1] - Math.Round(x[1])) < EPSILON) {
          cost += 3 * Math.Round(x[0]) + Math.Round(x[1]);
        }
      }
      Console.WriteLine(cost);
    }


  }
}
