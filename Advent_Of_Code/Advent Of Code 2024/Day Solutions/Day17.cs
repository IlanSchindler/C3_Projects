using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.FSharp.Core.ByRefKinds;

namespace Advent_Of_Code_2024.Day_Solutions {


  class CPU {
    long regA = 0;
    long regB = 0;
    long regC = 0;
    int insPt = 0;
    List<byte> program;
    List<byte> output;

    public CPU(long regA, long regB, long regC, List<byte> program) {
      SetCPU(regA, regB, regC, program);
    }

    public void SetCPU(long regA, long regB, long regC, List<byte> program) {
      this.regA = regA;
      this.regB = regB;
      this.regC = regC;
      this.program = program.ToList();
      if(this.output == null)
        this.output = new List<byte>();
      this.output.Clear();
    }

    public void RunProgram() {
      for(insPt = 0; insPt < program.Count; insPt += 2) {
        runCommand(program[insPt], program[insPt + 1]);
      }
    }

    public List<byte> GetOutput() {
      return output;
    }

    public void ResetCPU(long regA) {
      SetCPU(regA, 0, 0, program);
    }

    public List<byte> GetProgram() {
      return program;
    }



    long asCombo(byte b) {
      switch(b) {
        case 0:
        case 1:
        case 2:
        case 3:
          return asLiteral(b);
        case 4:
          return regA;
        case 5:
          return regB;
        case 6:
          return regC;
        case 7:
        default:
          throw new NotImplementedException();
      }
    }

    long asLiteral(byte b) {
      return (long)b;
    }


    void runCommand(byte opcode, byte operand) {
      switch(opcode) {
        case 0:
          adv(operand);
          break;
        case 1:
          bxl(operand);
          break;
        case 2:
          bst(operand);
          break;
        case 3:
          jnz(operand);
          break;
        case 4:
          bxc();
          break;
        case 5:
          _ot(operand);
          break;
        case 6:
          bdv(operand);
          break;
        case 7:
          cdv(operand);
          break;
        default:
          throw new NotImplementedException();
      }
    }

    void adv(byte operand) {
      regA = (long)(regA / Math.Pow(2, asCombo(operand)));
    }
    void bxl(byte operand) {
      regB = regB ^ asLiteral(operand);
    }
    void bst(byte operand) {
      regB = asCombo(operand) % 8;
    }
    void jnz(byte operand) {
      if(regA != 0) {
        insPt = (int)asLiteral(operand);
        insPt -= 2;
      }

    }
    void bxc() {
      regB = regB ^ regC;
    }
    void _ot(byte operand) {
      output.Add((byte)(asCombo(operand) % 8));

    }
    void bdv(byte operand) {
      regB = (long)(regA / Math.Pow(2, asCombo(operand)));
    }
    void cdv(byte operand) {
      regC = (long)(regA / Math.Pow(2, asCombo(operand)));
    }

  }

  internal class Day17 : Day {

    CPU cpu;


    public Day17() : base("Day17.txt") {
      FormatData();
    }
    public override void FormatData() {
      var input = Utilities.readInputFile(inputFile);
      int regA = int.Parse(input[0].Substring(12));
      int regB = int.Parse(input[1].Substring(12));
      int regC = int.Parse(input[2].Substring(12));
      List<byte> prog = input[4].Substring(9).Split(',').Select(x => byte.Parse(x)).ToList();
      cpu = new CPU(regA, regB, regC, prog);
    }


    public override void Part1() {
      
      cpu.RunProgram();
      var output = cpu.GetOutput();
      Console.Write("Program Output: ");
      Console.WriteLine(String.Join(',', output));
    }

    public override void Part2() {

      char[] initSol = "1000000000000000".ToCharArray();
      char[] sol = findSolution(cpu.GetProgram(), 0, initSol);
      Console.Write("Register A Initial Value: ");
      Console.WriteLine(Convert.ToInt64(new string(sol), 8));


    }

    char[] findSolution(List<byte> targetProg, int cIndex, char[] solString) {
      if(cIndex == solString.Length) {
        return solString;
      }
      for(int i =0; i < 8; i++) {
        solString[cIndex] = i.ToString()[0];
        cpu.ResetCPU(Convert.ToInt64(new string(solString), 8));
        cpu.RunProgram();
        var output = cpu.GetOutput();
        if(output.Count == targetProg.Count && output[targetProg.Count-cIndex -1] == targetProg[targetProg.Count - cIndex-1]) {
          char[] rString = findSolution(targetProg, cIndex+1, solString);
          if(rString != null)
            return rString;

        }

      }
      return null;
    }
  }
}
