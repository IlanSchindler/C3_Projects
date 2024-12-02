using System.IO;

namespace AoC_Utils {
  public static class Utils {
    public static List<string> readInput (string input_path) {

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