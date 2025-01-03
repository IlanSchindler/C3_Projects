using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day19 : Day {
    List<string> availablePatterns;
    List<string> targetPatterns;

    public Day19() : base("Day19.txt") {
      FormatData();
    }
    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile);
      availablePatterns = input[0].Split(", ").ToList();
      targetPatterns = input.Skip(2).ToList();
    }

    public override void Part1() {
      string regexString = buildRegex(availablePatterns);
      targetPatterns = targetPatterns.Where(x => {
        try { return Regex.IsMatch(" " + x + " ", regexString, RegexOptions.None, TimeSpan.FromMilliseconds(100)); } catch(RegexMatchTimeoutException) { return false; }
      }).ToList();
      var x = Regex.Match(" "+targetPatterns[0]+" ", regexString);
      Console.WriteLine("Possible patterns: {0}", targetPatterns.Count);
    }

    string buildRegex(List<string> inputs) {
      return " (" + String.Join('|', inputs) + ")* ";
    }

    public override void Part2() {
      long totalFoundPatterns = 0;
      //string regexString = buildRegex(availablePatterns);
      //Dictionary<string, List<List<string>>> why = new Dictionary<string, List<List<string>>>();
      //Stack<string> BuiltStrings = new Stack<string>();
      //availablePatterns.
      foreach(string pattern in targetPatterns) {
        long ways = countWays(pattern, availablePatterns, new Dictionary<string, long>());
        totalFoundPatterns += ways;
        continue;     
        /* BuiltStrings.Clear();
        BuiltStrings.Push(pattern);
        var usablePatterns = availablePatterns.Where(x => Regex.IsMatch(pattern, x)).ToList();
        while(BuiltStrings.Count > 0) {
          string curPattern = BuiltStrings.Pop();
          if(string.IsNullOrEmpty(curPattern)) {
            //Console.WriteLine(++totalFoundPatterns);
            totalFoundPatterns++;
            if(totalFoundPatterns % 1_000_000 == 0)
              Console.WriteLine(totalFoundPatterns);
            continue;          
          }
          foreach(var p in usablePatterns.Where(x => curPattern.IndexOf(x) == 0)) {
            BuiltStrings.Push(curPattern.Remove(0,p.Length));          
          }
        }*/
        /*why.Add(pattern, new List<List<string>> { new List<string> { pattern} });
        while(why[pattern].Any(x => x[0].Length > 0)) { 
          int patternCount = why[pattern].Count;
          for(int i = 0; i < patternCount; i++) {
            
            if(why[pattern][i][0].Length == 0)
              continue;
            foreach(var p in usablePatterns.Where(x => why[pattern][i][0].IndexOf(x) == 0).ToList()) {
              List<string> nPat = why[pattern][i].ToList();
              nPat.Add(p);
              nPat[0] = nPat[0].Remove(0, p.Length);
              why[pattern].Add(nPat);
            }
            why[pattern].RemoveAt(i);
            i--;
            patternCount--;
          }
        }*/
      }
      Console.WriteLine("Possible Combinations: {0}", totalFoundPatterns);
    }

    long countWays(string design, List<string> patterns, Dictionary<string, long> cache) {
      if(cache.ContainsKey(design))
        return cache[design];
      if(design.Length == 0)
        return 1;

      long count = patterns.Where(x => design.StartsWith(x)).Aggregate((long)0, (x, y) => x + countWays(design.Substring(y.Length), patterns, cache));
      cache.TryAdd(design, count);
      cache[design] = count;
      return count; 
    }
  }
}
