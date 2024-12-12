using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using static Advent_Of_Code_2024.Utilities;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day12 : Day {

    class plot {
      public static int idCount = 0;
      public int id;
      public char crop;
      public Dictionary<Utilities.directions, plot> neighbors;
      public region region;

      public plot(char inId) : this(inId, null) {

      }

      public plot(char inId, region inRegion) {
        id=idCount++;
        crop = inId;
        region = inRegion;
        neighbors = new Dictionary<Utilities.directions, plot>{
          { directions.up , null },
          { directions.left , null },
          { directions.down , null},
          { directions.right , null}
        };
      }
    }

    class region {
      public static int idCount = 0;
      public int id;
      public char crop;
      public List<plot> plots;

      public region(char inId) {
        id=idCount++;
        crop = inId;
        plots = new List<plot>();
      }

      public void AddPlots(plot p) {
        if(!this.plots.Contains(p)) {
          plots.Add(p);
          p.region = this;
        }
        foreach(var n in p.neighbors.Values.Where(x => x != null && !this.plots.Contains(x)).ToList()) {
          AddPlots(n);
        }
      }

      public int getArea() {
        return plots.Count;
      }

      public int getPerimeter() {
        return plots.SelectMany(x => x.neighbors.Values).Count(x => x == null);
      }

      public int getPrice() {
        return getArea() * getPerimeter();
      }

      public int getSides() {
        int sideCount = 0;
        foreach( directions dir in Enum.GetValues(typeof(directions))) {
          var validPlots = plots.Where(x => x.neighbors[dir] == null).ToList();
          List<List<plot>> groups = new List<List<plot>>();
          foreach(var plot in validPlots.OrderBy(x=>x.id)) {
            List<plot> group = groups.FirstOrDefault(x => x.Any(y => plot.neighbors.ContainsValue(y)));
            if(group == null) {
              group = new List<plot>();
              groups.Add(group);
            }
            group.Add(plot);
          }
          sideCount += groups.Count;
        }
        return sideCount;
      }
    }

    List<List<plot>> map;
    List<region> regions;

    public Day12() : base("Day12.txt") {
                         FormatData();
    }

    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile);
      map = new List<List<plot>>();
      for(int i = 0; i < input.Count; i++) {
        map.Add(new List<plot>());
        for(int j = 0; j < input[0].Length; j++) {
          map[i].Add(new plot(input[i][j]));
        }
      }

      for(int i = 0; i < input.Count; i++) {
        for(int j = 0; j < input[0].Length; j++) {
          plot thisPlot = map[i][j];
          plot neighbor = null;
          foreach(Utilities.directions dir in Enum.GetValues(typeof(Utilities.directions))) {
            neighbor = null;
            Utilities.tryIgnore<ArgumentOutOfRangeException>(() => {
              neighbor = map[i + movements[dir].Item1][j + movements[dir].Item2];
            });
            if(neighbor != null && neighbor.crop == thisPlot.crop) {
              thisPlot.neighbors[dir] = neighbor;
            }
          }
        }
      }

      regions = new List<region>();
      for(int i = 0; i < input.Count; i++) {
        for(int j = 0; j < input[0].Length; j++) {
          if(regions.SelectMany(x => x.plots).Contains(map[i][j])) {
            continue;
          }
          var newRegion = new region(map[i][j].crop);
          regions.Add(newRegion);
          newRegion.AddPlots(map[i][j]);
        }
      }
    }
         
    public override void Part1() {
      /*int maxAreaCount = ("" + regions.Max(x => x.getArea())).Length;
      int maxPermCount = ("" + regions.Max(x => x.getPerimeter())).Length;
      int maxPriceCount = ("" + regions.Max(x =>x.getArea() * x.getPerimeter())).Length;
      foreach(var r in regions) {
        Console.WriteLine(String.Format("A region of {0} plants with {1," + maxAreaCount + "} area and {2," + maxPermCount + "} perimeter will cost {3," + maxPriceCount + "}", r.crop, r.getArea(), r.getPerimeter(), r.getArea()*r.getPerimeter()));
      }  */
      Console.WriteLine("Total Cost: {0}", regions.Sum(x => x.getArea()*x.getPerimeter()));
    }

    public override void Part2() {
      /*int maxAreaCount = ("" + regions.Max(x => x.getArea())).Length;
      int maxSideCount = ("" + regions.Max(x => x.getSides())).Length;
      int maxPriceCount = ("" + regions.Max(x =>x.getArea() * x.getSides())).Length;
      foreach(var r in regions) {
        Console.WriteLine(String.Format("A region of {0} plants with {1," + maxAreaCount + "} area and {2," + maxSideCount + "} perimeter will cost {3," + maxPriceCount + "}", r.crop, r.getArea(), r.getSides(), r.getArea()*r.getSides()));
      } */
      Console.WriteLine("Total Cost: {0}", regions.Sum(x => x.getArea()*x.getSides()));
    }
  }
}
