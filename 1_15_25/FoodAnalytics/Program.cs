using System.Runtime.InteropServices;

namespace FoodAnalytics {

  class food {
    public int Id;
    public string Category;
    public string Subcategory;
    public double Price;
    public DateTime TimeStamp;
  }
  internal class Program {
    static void Main(string[] args) {
      var input = readInputFile("dataset.csv").Select(x => x.Replace("\"", "").Replace("\'", "").Replace("$", ""));
      var b = input.Count(x => x.Contains('\"'));
      var c = input.Where(x => x.Contains('$')).ToList();
      var splitInput = input.Skip(1).Select(x => x.Split(',').Select(y => y.Trim()).ToArray()).Where(x => x.Length == 5).ToList();
      List<food> foods = new List<food>();
      foreach(var line in splitInput) {
        food newFood = new food();
        if(!int.TryParse(line[0], out newFood.Id)) {
          continue;
        }
        newFood.Category = line[1];
        newFood.Subcategory = line[2];
        if(!double.TryParse(line[3], out newFood.Price)) {
          continue;
        }
        if(!DateTime.TryParse(line[4], out newFood.TimeStamp)) {
          continue;
        }
        foods.Add(newFood);
      }
      var overallValues = calculateValues(foods);
      Console.WriteLine("Overall: Total: {0,10}, Min: {1}, Max: {2}, Avg: {3}", overallValues.total.ToString("F"), overallValues.min.ToString("F"), overallValues.max.ToString("F"), overallValues.avg.ToString("F"));
      foreach(var category in foods.GroupBy(x => x.Category).ToList()) {
        var categoryValues = calculateValues(category.ToList());
        Console.WriteLine("  Category: {0,8}, Total: {1,10}, Min: {2}, Max: {3}, Avg: {4}", category.Key, categoryValues.total.ToString("F"), categoryValues.min.ToString("F"), categoryValues.max.ToString("F"), categoryValues.avg.ToString("F"));
        foreach(var subcategory in category.GroupBy(x => x.Subcategory).ToList()) {
          var subcategoryValues = calculateValues(subcategory.ToList());
          Console.WriteLine("    Subcategory: {0,8}, Total: {1,10}, Min: {2}, Max: {3}, Avg: {4}", subcategory.Key, subcategoryValues.total.ToString("F"), subcategoryValues.min.ToString("F"), subcategoryValues.max.ToString("F"), subcategoryValues.avg.ToString("F"));
        }
      }
      
      
    }

    public static (double total, double avg, double min, double max) calculateValues(List<food> foods) {
      var values = foods.Select(x => x.Price).ToList();
      return (values.Sum(), values.Average(), values.Min(), values.Max());
    }

  
    public static string InputPath { get { return @"D:\C3\1_15_25\20250115-food-analytics-challenge\"; } }

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
  }
}
