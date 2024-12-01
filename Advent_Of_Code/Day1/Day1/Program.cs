using System.IO;

namespace Day1 {
  internal class Program {
    static void Main(string[] args) {
      string path = @"D:\C3\Advent_Of_Code\Day1\Day1\input.txt";
      List<List<int>> input = new List<List<int>> { new List<int>(), new List<int>() };
      using(StreamReader sr = File.OpenText(path)) {
        string inputLine = "";
        string[] splitInput;
        int input0 = 0, input1 = 0;
        while((inputLine = sr.ReadLine()) != null) {
          splitInput = inputLine.Split(" ", StringSplitOptions.TrimEntries);
          if(!int.TryParse(splitInput[0], out input0) || !int.TryParse(splitInput[3], out input1)) {
            Console.WriteLine("Invalid Input Found: " + inputLine);
            continue;
          }
          input[0].Add(input0);
          input[1].Add(input1);
        }
      }

      input[0] = input[0].OrderBy(x => x).ToList();
      input[1] = input[1].OrderBy(x => x).ToList();

      if(input[0].Count != input[1].Count) {
        Console.WriteLine("Error: There are an inequal number of IDs");
        Environment.Exit(1);
      }
      
      int totalDistance = 0;
      for(int i = 0; i < input[0].Count; i++) {
        totalDistance += Math.Abs(input[0][i] - input[1][i]);
      }
      Console.WriteLine($"Total Distance: {totalDistance}");

      int similarityScore = 0;
      for(int i = 0;i < input[0].Count; i++) {
        similarityScore += input[0][i] * input[1].Count(x => x == input[0][i]);
      }
      Console.WriteLine($"Similarity Score: {similarityScore}");

    }
  }
}
