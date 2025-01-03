using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code_2024.Day_Solutions {
  internal class Day24 : Day {

    class gate {
      public string Input1 { get; set; }
      public string Input2 { get; set; }
      public string Output { get; set; }
      public string Func { get; set; }

      public gate(string in1, string in2, string output, string func) {
        Input1 = in1;
        Input2 = in2;
        Output = output;
        Func = func;
      }

      public void ReorderInputs() {
        //gates.Where(x=>x.Input1.Contains("AND")||x.Input1.Contains("OR")||x.Input1.Contains("XOR")||x.Input2.Contains("AND")||x.Input2.Contains("OR")||x.Input2.Contains("XOR")).ToList()
        if((Input1.Contains("AND") || Input1.Contains("OR") || Input1.Contains("XOR")) && (!Input2.Contains("AND") || !Input2.Contains("OR") || !Input2.Contains("XOR")))
          return;
        if((Input2.Contains("AND") || Input2.Contains("OR") || Input2.Contains("XOR")) && (!Input1.Contains("AND") || !Input1.Contains("OR") || !Input1.Contains("XOR"))) {
          string temp = Input1;
          Input1 = Input2;
          Input2 = temp;
          return;
        }

      }
    }

    List<gate> gates;
    Dictionary<string, bool?> wires;

    public Day24() : base("Day24.txt") {
      FormatData();
    }
    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile);
      gates = new List<gate>();
      wires = new Dictionary<string, bool?>();
      foreach(var inputWire in input.Where(x => x.Contains(':')).ToList()) {
        var wire = inputWire.Split(':', StringSplitOptions.TrimEntries);
        wires.Add(wire[0], wire[1] == "1");
      }
      foreach(var inputGate in input.Where(x => x.Contains('-')).ToList()) {
        var rawGate = inputGate.Split(' ');
        gates.Add(new gate(rawGate[0], rawGate[2], rawGate[4], rawGate[1]));
        wires.Add(rawGate[4], null);
      }
    }

    public override void Part1() {
      while(wires.Values.Any(x => x == null)) {
        foreach(var activeGate in gates.Where(x => wires[x.Input1] != null && wires[x.Input2] != null && wires[x.Output] == null).ToList()) {
          switch(activeGate.Func) {
            case "AND":
              wires[activeGate.Output] = (bool)wires[activeGate.Input1] && (bool)wires[activeGate.Input2];
              break;
            case "OR":
              wires[activeGate.Output] = (bool)wires[activeGate.Input1] || (bool)wires[activeGate.Input2];
              break;
            case "XOR":
              wires[activeGate.Output] = (bool)wires[activeGate.Input1] ^ (bool)wires[activeGate.Input2];
              break;
          }
        }
      }

      var outputs = wires.Where(x => x.Key.Contains('z')).OrderByDescending(x => x.Key).ToList();
      //string.Join("", wires.Where(x=>x.Key.Contains('x')).OrderByDescending(x=>x.Key).Select(x=>(bool)x.Value?1:0).ToList())
      string outputString = string.Join("", outputs.Select(x => (bool)x.Value ? 1 : 0).ToList());
      long outputValue = Convert.ToInt64(outputString, 2);
      Console.WriteLine("Output String: {0}", outputString);
      Console.WriteLine("Output Value:  {0}", outputValue);
      //Console.WriteLine("done");
    }


    class modifiedGate {
      public string Id { get; set; }
      public modifiedGate Input1 { get; set; }
      public modifiedGate Input2 { get; set; }
      public List<modifiedGate> Inputs { get; set; }
      public string Func { get; set; }
      public string Definition {
        get {
          if(Input1 == null || Input2 == null) {
            return Id;
          } else {
            return $"({Input1.Id}-{Func}-{Input2.Id})";
          }
        }
      }

      public string def {
        get {

          if(Input1 == null || Input2 == null) {
            return Id;
          } else {
            var orderedInputs = Inputs.OrderBy(x => modifiedGate.OrderBy(x)).ToList();
            return $"({orderedInputs[0].Id}-{Func}-{orderedInputs[1].Id})";
          }

        }
      }
      public string VerboseDefinition {
        get {
          if(Input1 == null || Input2 == null) {
            return Id;
          } else {
            return $"({Input1.VerboseDefinition}-{Func}-{Input2.VerboseDefinition})";
          }
        }
      }

      public string verboseDef {
        get {
          if(Input1 == null || Input2 == null) {
            return Id;
          } else {
            var orderedInputs = Inputs.OrderBy(x => OrderBy(x)).ToList();
            return $"({orderedInputs[0].verboseDef}-{Func}-{orderedInputs[1].verboseDef})";
          }
        }
      }

      private static int OrderBy(modifiedGate g) {
        if(g.Func == null) {
          return 0;
        } else if(g.Func == "AND") {
          if(g.Input1.Func == null) {
            return 2;
          } else {
            return 3;
          }
        } else if(g.Func == "OR") {
          return 4;

        } else if(g.Func == "XOR") {
          return 1;
        } else { return -1; }
      }
    }

    void fixData() {
               var input = Utilities.readInputFile(inputFile);
      gates = new List<gate>();
      wires = new Dictionary<string, bool?>();
      foreach(var inputWire in input.Where(x => x.Contains(':')).ToList()) {
        var wire = inputWire.Split(':', StringSplitOptions.TrimEntries);
        wires.Add(wire[0], wire[1] == "1");
      }
 

      foreach(var inputGate in input.Where(x => x.Contains('-')).ToList()) {
        var rawGate = inputGate.Split(' ');
          if(rawGate[4] == "z22")
          rawGate[4] = "gjh";
        else if(rawGate[4] == "gjh")
          rawGate[4] = "z22";
        if(rawGate[4] == "z31")
          rawGate[4] = "jdr";
        else if(rawGate[4] == "jdr")
          rawGate[4] = "z31";
        if(rawGate[4] == "z08")
          rawGate[4] = "ffj";
        else if(rawGate[4] == "ffj")
          rawGate[4] = "z08";

        if(rawGate[4] == "dwp")
          rawGate[4] = "kfm";
        else if(rawGate[4] == "kfm")
          rawGate[4] = "dwp";
        gates.Add(new gate(rawGate[0], rawGate[2], rawGate[4], rawGate[1]));
        wires.Add(rawGate[4], null);
      }
      

    }

    void reformatData() {
      var input = Utilities.readInputFile(inputFile);
      gates = new List<gate>();
      wires = new Dictionary<string, bool?>();
      foreach(var inputWire in input.Where(x => x.Contains(':')).ToList()) {
        var wire = inputWire.Split(':', StringSplitOptions.TrimEntries);
        wires.Add(wire[0], wire[1] == "1");
      }
      Dictionary<string, string> modifiedWires = new Dictionary<string, string>();
      foreach(var inputGate in input.Where(x => x.Contains('-')).ToList()) {
        var rawGate = inputGate.Split(' ');
        if(rawGate[4] == "z22")
          rawGate[4] = "gjh";
        else if(rawGate[4] == "gjh")
          rawGate[4] = "z22";
        if(rawGate[4] == "z31")
          rawGate[4] = "jdr";
        else if(rawGate[4] == "jdr")
          rawGate[4] = "z31";
        if(rawGate[4] == "z08")
          rawGate[4] = "ffj";
        else if(rawGate[4] == "ffj")
          rawGate[4] = "z08";

        if(rawGate[4] == "dwp")
          rawGate[4] = "kfm";
        else if(rawGate[4] == "kfm")
          rawGate[4] = "dwp";
        if(rawGate[0].StartsWith('y') || rawGate[0].StartsWith('x')) {
          string newWire = rawGate[1] + "_" + rawGate[0].Substring(1);
          if(newWire == "AND_00")
            newWire = "C_00";
          modifiedWires.Add(rawGate[4], newWire);
          rawGate[4] = newWire;
        }
        if(rawGate[0].StartsWith('y') && rawGate[2].StartsWith('x'))
          gates.Add(new gate(rawGate[2], rawGate[0], rawGate[4], rawGate[1]));
        else
          gates.Add(new gate(rawGate[0], rawGate[2], rawGate[4], rawGate[1]));
        wires.Add(rawGate[4], null);
      }
      

      foreach(var gate in gates) {
        if(modifiedWires.ContainsKey(gate.Input1)) {

          wires.Remove(gate.Input1);
          wires.TryAdd(modifiedWires[gate.Input1], null);
          gate.Input1 = modifiedWires[gate.Input1];
        }
        if(modifiedWires.ContainsKey(gate.Input2)) {
          wires.Remove(gate.Input2);
          wires.TryAdd(modifiedWires[gate.Input2], null);
          gate.Input2 = modifiedWires[gate.Input2];
        }
        gate.ReorderInputs();
      }

   
      foreach(var gate in gates.Where(x => x.Func == "OR")) {
        if(gate.Input1.Contains("AND")) {
          string newWire = "C_" + gate.Input1.Substring(4);
          if(gate.Output == "z45") {
            Console.Write("done");
            wires.Remove(gate.Output);
          wires.TryAdd(newWire, null);
          }
          modifiedWires.Add(gate.Output, newWire);
          gate.Output = newWire;
        }
      }

      
      foreach(var gate in gates) {
        if(modifiedWires.ContainsKey(gate.Input1)) {

          wires.Remove(gate.Input1);
          wires.TryAdd(modifiedWires[gate.Input1], null);
          gate.Input1 = modifiedWires[gate.Input1];
        }
        if(modifiedWires.ContainsKey(gate.Input2)) {
          wires.Remove(gate.Input2);
          wires.TryAdd(modifiedWires[gate.Input2], null);
          gate.Input2 = modifiedWires[gate.Input2];
        }
        gate.ReorderInputs();
      }

       foreach(var gate in gates.Where(x => x.Func == "AND")) {
        if(gate.Input1.StartsWith("XOR") &&
          gate.Input2.StartsWith("C") &&
          (int.Parse(gate.Input1.Substring(4)) == int.Parse(gate.Input2.Substring(2)) + 1)) {
          string newWire = "AND_" + gate.Input2.Substring(2) + "+" + gate.Input1.Substring(4);
          modifiedWires.Add(gate.Output, newWire);
          gate.Output = newWire;
        }
        
      }

      
      foreach(var gate in gates) {
        if(modifiedWires.ContainsKey(gate.Input1)) {

          wires.Remove(gate.Input1);
          wires.TryAdd(modifiedWires[gate.Input1], null);
          gate.Input1 = modifiedWires[gate.Input1];
        }
        if(modifiedWires.ContainsKey(gate.Input2)) {
          wires.Remove(gate.Input2);
          wires.TryAdd(modifiedWires[gate.Input2], null);
          gate.Input2 = modifiedWires[gate.Input2];
        }
        gate.ReorderInputs();
      }

      foreach(var w in modifiedWires) {
        wires.Remove(w.Key);
        wires.TryAdd(w.Value, null);
      }
    }
    public override void Part2() {

      fixData();
      while(wires.Values.Any(x => x == null)) {
        foreach(var activeGate in gates.Where(x => wires[x.Input1] != null && wires[x.Input2] != null && wires[x.Output] == null).ToList()) {
          switch(activeGate.Func) {
            case "AND":
              wires[activeGate.Output] = (bool)wires[activeGate.Input1] && (bool)wires[activeGate.Input2];
              break;
            case "OR":
              wires[activeGate.Output] = (bool)wires[activeGate.Input1] || (bool)wires[activeGate.Input2];
              break;
            case "XOR":
              wires[activeGate.Output] = (bool)wires[activeGate.Input1] ^ (bool)wires[activeGate.Input2];
              break;
          }
        }
      }

      var outputs = wires.Where(x => x.Key.Contains('z')).OrderByDescending(x => x.Key).ToList();
      //string.Join("", wires.Where(x=>x.Key.Contains('x')).OrderByDescending(x=>x.Key).Select(x=>(bool)x.Value?1:0).ToList())
      string outputString = string.Join("", outputs.Select(x => (bool)x.Value ? 1 : 0).ToList());
      long outputValue = Convert.ToInt64(outputString, 2);
      Console.WriteLine("Output String: {0}", outputString);
      Console.WriteLine("Output Value:  {0}", outputValue);
      //Console.WriteLine("done");
      List<modifiedGate> modifiedGates = new List<modifiedGate>();
      foreach(var wire in wires) {
        modifiedGates.Add(new modifiedGate() { Id = wire.Key, Inputs = new List<modifiedGate>() });
      }
      foreach(var gate in gates) {
        var targetGate = modifiedGates.First(x => x.Id == gate.Output);
        targetGate.Func = gate.Func;
        targetGate.Inputs.Add(modifiedGates.First(x => x.Id == gate.Input1));
        targetGate.Inputs.Add(modifiedGates.First(x => x.Id == gate.Input2));
        targetGate.Input1 = modifiedGates.First(x => x.Id == gate.Input1);
        targetGate.Input2 = modifiedGates.First(x => x.Id == gate.Input2);
      }
      Console.WriteLine(modifiedGates.First(x => x.Id == "z45").verboseDef);
      Console.WriteLine("done");
      // dwp,ffj,gjh,jdr,kfm,z08,z22,z31
    }
  }
}

/*
 * 
 

" 11101 0110111010 1111101100 1100101000 1000111001"
" 11100 0000010101 0001010000 0101000111 1010101111"
"111001 0111001110 0001000001 0010010000 0011101000"
"111001 0111010000 0000111101 0001110000 0011101000"
 111001 0111010000 0000111101 0001110000 0011101000
 444444 3333333333 2222222222 1111111111 0000000000
 543210 9876543210 9876543210 9876543210 9876543210
 111001 0111001110 0001000001 0010010000 0011101000
 */