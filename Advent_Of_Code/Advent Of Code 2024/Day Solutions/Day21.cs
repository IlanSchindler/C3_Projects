using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day21 : Day {

    Dictionary<(char start, char end), string> numDict = new Dictionary<(char start, char end), string> {
      { ('A','A'),"" },
      { ('A','0'),"<" },
      { ('A','1'),"^<<" },
      { ('A','2'),"<^" },
      { ('A','3'),"^" },
      { ('A','4'),"^^<<" },
      { ('A','5'),"<^^" },
      { ('A','6'),"^^" },
      { ('A','7'),"^^^<<" },
      { ('A','8'),"<^^^" },
      { ('A','9'),"^^^" },
      { ('0','A'),">" },
      { ('0','0'),"" },
      { ('0','1'),"^<" },
      { ('0','2'),"^" },
      { ('0','3'),"^>" },
      { ('0','4'),"^^<" },
      { ('0','5'),"^^" },
      { ('0','6'),"^^>" },
      { ('0','7'),"^^^<" },
      { ('0','8'),"^^^" },
      { ('0','9'),"^^^>" },
      { ('1','A'),">>v" },
      { ('1','0'),">v" },
      { ('1','1'),"" },
      { ('1','2'),">" },
      { ('1','3'),">>" },
      { ('1','4'),"^" },
      { ('1','5'),"^>" },
      { ('1','6'),"^>>" },
      { ('1','7'),"^^" },
      { ('1','8'),"^^>" },
      { ('1','9'),"^^>>" },
      { ('2','A'),"v>" },
      { ('2','0'),"v" },
      { ('2','1'),"<" },
      { ('2','2'),"" },
      { ('2','3'),">" },
      { ('2','4'),"<^" },
      { ('2','5'),"^" },
      { ('2','6'),"^>" },
      { ('2','7'),"<^^" },
      { ('2','8'),"^^" },
      { ('2','9'),"^^>" },
      { ('3','A'),"v" },
      { ('3','0'),"<v" },
      { ('3','1'),"<<" },
      { ('3','2'),"<" },
      { ('3','3'),"" },
      { ('3','4'),"<<^" },
      { ('3','5'),"<^" },
      { ('3','6'),"^" },
      { ('3','7'),"<<^^" },
      { ('3','8'),"<^^" },
      { ('3','9'),"^^" },
      { ('4','A'),">>vv" },
      { ('4','0'),">vv" },
      { ('4','1'),"v" },
      { ('4','2'),"v>" },
      { ('4','3'),"v>>" },
      { ('4','4'),"" },
      { ('4','5'),">" },
      { ('4','6'),">>" },
      { ('4','7'),"^" },
      { ('4','8'),"^>" },
      { ('4','9'),"^>>" },
      { ('5','A'),"vv>" },
      { ('5','0'),"vv" },
      { ('5','1'),"<v" },
      { ('5','2'),"v" },
      { ('5','3'),"v>" },
      { ('5','4'),"<" },
      { ('5','5'),"" },
      { ('5','6'),">" },
      { ('5','7'),"<^" },
      { ('5','8'),"^" },
      { ('5','9'),"^>" },
      { ('6','A'),"vv" },
      { ('6','0'),"<vv" },
      { ('6','1'),"<<v" },
      { ('6','2'),"<v" },
      { ('6','3'),"v" },
      { ('6','4'),"<<" },
      { ('6','5'),"<" },
      { ('6','6'),"" },
      { ('6','7'),"<<^" },
      { ('6','8'),"<^" },
      { ('6','9'),"^" },
      { ('7','A'),">>vvv" },
      { ('7','0'),">vvv" },
      { ('7','1'),"vv" },
      { ('7','2'),"vv>" },
      { ('7','3'),"vv>>" },
      { ('7','4'),"v" },
      { ('7','5'),"v>" },
      { ('7','6'),"v>>" },
      { ('7','7'),"" },
      { ('7','8'),">" },
      { ('7','9'),">>" },
      { ('8','A'),"vvv>" },
      { ('8','0'),"vvv" },
      { ('8','1'),"<vv" },
      { ('8','2'),"vv" },
      { ('8','3'),"vv>" },
      { ('8','4'),"<v" },
      { ('8','5'),"v" },
      { ('8','6'),"v>" },
      { ('8','7'),"<" },
      { ('8','8'),"" },
      { ('8','9'),">" },
      { ('9','A'),"vvv" },
      { ('9','0'),"<vvv" },
      { ('9','1'),"<<vv" },
      { ('9','2'),"<vv" },
      { ('9','3'),"vv" },
      { ('9','4'),"<<v" },
      { ('9','5'),"<v" },
      { ('9','6'),"v" },
      { ('9','7'),"<<" },
      { ('9','8'),"<" },
      { ('9','9'),"" }
    };

    Dictionary<(char start, char end), string> dirDict = new Dictionary<(char start, char end), string> {
      { ('A','A'),"" },
      { ('A','^'),"<" },
      { ('A','v'),"<v" },
      { ('A','<'),"v<<" },
      { ('A','>'),"v" },
      { ('^','A'),">" },
      { ('^','^'),"" },
      { ('^','v'),"v" },
      { ('^','<'),"v<" },
      { ('^','>'),"v>" },
      { ('v','A'),"^>" },
      { ('v','^'),"^" },
      { ('v','v'),"" },
      { ('v','<'),"<" },
      { ('v','>'),">" },
      { ('<','A'),">>^" },
      { ('<','^'),">^" },
      { ('<','v'),">" },
      { ('<','<'),"" },
      { ('<','>'),">>" },
      { ('>','A'),"^" },
      { ('>','^'),"<^" },
      { ('>','v'),"<" },
      { ('>','<'),"<<" },
      { ('>','>'),"" }
    };

    Dictionary<(char start, char end), long> dirTransitions = new Dictionary<(char start, char end), long> {
      { ('A','A'),0 },
      { ('A','^'),0 },
      { ('A','v'),0 },
      { ('A','<'),0 },
      { ('A','>'),0 },
      { ('^','A'),0 },
      { ('^','^'),0 },
      { ('^','v'),0 },
      { ('^','<'),0 },
      { ('^','>'),0 },
      { ('v','A'),0 },
      { ('v','^'),0 },
      { ('v','v'),0 },
      { ('v','<'),0 },
      { ('v','>'),0 },
      { ('<','A'),0 },
      { ('<','^'),0 },
      { ('<','v'),0 },
      { ('<','<'),0 },
      { ('<','>'),0 },
      { ('>','A'),0 },
      { ('>','^'),0 },
      { ('>','v'),0 },
      { ('>','<'),0 },
      { ('>','>'),0 }
    };

    List<string> codes;
    public Day21() : base("Day21.txt") {
      FormatData();
    }
    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile);
      codes = new List<string>();
      foreach(var s in input) {
        codes.Add(s);
      }
    }

    public string generateCommandString(Dictionary<(char start, char end), string> dict, string outputCommands) {
      string returnString = "";
      outputCommands = "A" + outputCommands;
      for(int i = 0; i < outputCommands.Length - 1; i++) {
        returnString += dict[(outputCommands[i], outputCommands[i + 1])] + "A";
      }
      return returnString;
    }

    public override void Part1() {
      Dictionary<string, string> commandSequences = new Dictionary<string, string>();

      long sum = 0;
      foreach(var s in codes) {
        string bot1Seq = generateCommandString(numDict, s);
        string bot2Seq = generateCommandString(dirDict, bot1Seq);
        string mySeq = generateCommandString(dirDict, bot2Seq);
        commandSequences.Add(s, mySeq);
        Console.WriteLine(s + ": " + mySeq);
        sum += int.Parse(s.Substring(0, s.Length - 1)) * mySeq.Length;
      }

      Console.WriteLine("Total Complexity: {0}", sum);
    }

    public override void Part2() {
      Dictionary<string, string> commandSequences = new Dictionary<string, string>();

      long sum = 0;
      foreach(var s in codes) {
        string comSeq = "A" + generateCommandString(numDict, s);
        var comTrans = dirTransitions.ToDictionary();

        for(int i = 0; i < comSeq.Length - 1; i++) {
          comTrans[(comSeq[i], comSeq[i + 1])]++;
        }
        for(int i = 0; i < 25; i++) {
          var newTrans = dirTransitions.ToDictionary();
          foreach(var t in comTrans) {
            string tString = "A" + dirDict[t.Key] + "A"; 
            for(int j = 0; j < tString.Length - 1; j++) {
              newTrans[(tString[j], tString[j + 1])]+=t.Value;
            }
          }
          comTrans = newTrans.ToDictionary();

          
        }
        Console.WriteLine("Code Count: {0}", comTrans.Values.Sum());

        
        sum += int.Parse(s.Substring(0, s.Length - 1)) * comTrans.Values.Sum();
      }

      Console.WriteLine("Total Complexity: {0}", sum);
    }
  }
}

