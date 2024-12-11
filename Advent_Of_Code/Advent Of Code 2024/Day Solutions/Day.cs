using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code_2024 {
  internal abstract class Day {
    public string inputFile { get; set; }

    public Day(string input) {
      inputFile = input;
    }

    public abstract void Part1();
    public abstract void Part2();

    public abstract void FormatData();

  }
}
