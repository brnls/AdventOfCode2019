using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Console.Day7
{
    class IntCodeComputer
    {
        public int Offset { get; set; } = 0;
        public int[] Instructions { get; set; }
        public bool Complete { get; private set; } = false;
        public int Output { get; set; }

        public List<int> Run(Queue<int> inputs)
        {
            var results = new List<int>();
            while (Offset < Instructions.Length)
            {
                var instruction = Instruction.ParseInstruction(Instructions, Offset);
                if (instruction.OpCode == OpCode.Add)
                {
                    Instructions[Instructions[Offset + 3]] = instruction.First.GetValue(Instructions, Offset + 1) + instruction.Second.GetValue(Instructions, Offset + 2);
                    Offset += instruction.Size;
                }
                else if (instruction.OpCode == OpCode.Multiply)
                {
                    Instructions[Instructions[Offset + 3]] = instruction.First.GetValue(Instructions, Offset + 1) * instruction.Second.GetValue(Instructions, Offset + 2);
                    Offset += instruction.Size;
                }
                else if (instruction.OpCode == OpCode.Save)
                {
                    Instructions[Instructions[Offset + 1]] = inputs.Dequeue();
                    Offset += instruction.Size;
                }
                else if (instruction.OpCode == OpCode.Read)
                {
                    results.Add(instruction.First.GetValue(Instructions, Offset + 1));
                    Output = results[0];
                    Offset += instruction.Size;
                    break;
                }
                else if (instruction.OpCode == OpCode.JumpTrue)
                {
                    if (instruction.First.GetValue(Instructions, Offset + 1) != 0)
                        Offset = instruction.Second.GetValue(Instructions, Offset + 2);
                    else Offset += instruction.Size;
                }
                else if (instruction.OpCode == OpCode.JumpFalse)
                {
                    if (instruction.First.GetValue(Instructions, Offset + 1) == 0)
                        Offset = instruction.Second.GetValue(Instructions, Offset + 2);
                    else Offset += instruction.Size;
                }
                else if (instruction.OpCode == OpCode.LessThan)
                {
                    if (instruction.First.GetValue(Instructions, Offset + 1) < instruction.Second.GetValue(Instructions, Offset + 2))
                        Instructions[Instructions[Offset + 3]] = 1;
                    else Instructions[Instructions[Offset + 3]] = 0;

                    Offset += instruction.Size;
                }
                else if (instruction.OpCode == OpCode.Equals)
                {
                    if (instruction.First.GetValue(Instructions, Offset + 1) == instruction.Second.GetValue(Instructions, Offset + 2))
                        Instructions[Instructions[Offset + 3]] = 1;
                    else Instructions[Instructions[Offset + 3]] = 0;

                    Offset += instruction.Size;
                }
                else if (instruction.OpCode == OpCode.Break)
                {
                    Complete = true;
                    break;
                }
                else throw new Exception($"Unknown opcode: {instruction.OpCode}");
            }
            return results;
        }

    }

    public enum OpCode
    {
        Add = 1,
        Multiply = 2,

        ///<summary>
        /// takes a single integer as input and saves it to the position given by its only parameter. 
        /// For example, the instruction 3,50 would take an input value and store it at address 50.
        ///</summary>
        Save = 3,

        ///<summary>
        /// outputs the value of its only parameter. For example, the instruction 4,50 
        /// would output the value at address 50
        ///</summary>
        Read = 4,

        /// <summary>
        /// if the first parameter is non-zero, it sets the instruction pointer to the value
        /// from the second parameter. Otherwise, it does nothing.
        /// </summary>
        JumpTrue = 5,
        JumpFalse = 6,
        LessThan = 7,
        Equals = 8,
        Break = 99
    }

    public enum ParameterMode
    {
        Position = 0,
        Immediate = 1
    }

    public class Instruction
    {
        public OpCode OpCode { get; set; }
        public Parameter First { get; set; }
        public Parameter Second { get; set; }
        public Parameter Third { get; set; }
        public int Size => OpCode switch
        {
            OpCode.Save => 2,
            OpCode.Read => 2,
            OpCode.JumpFalse => 3,
            OpCode.JumpTrue => 3,
            OpCode.LessThan => 4,
            OpCode.Equals => 4,
            OpCode.Add => 4,
            OpCode.Multiply => 4,
            _ => throw new Exception($"invalid opcode {OpCode}")
        };

        public static Instruction ParseInstruction(int[] instructions, int offset)
        {
            var instructionHeader = instructions[offset];
            var opCode = instructions[offset] % 100;
            if (!Enum.IsDefined(typeof(OpCode), opCode)) throw new Exception($"Invalid op code: {opCode}");

            var third = ParseParameterMode(instructionHeader / 10000);
            instructionHeader %= 10000;
            var second = ParseParameterMode(instructionHeader / 1000);
            instructionHeader %= 1000;
            var first = ParseParameterMode(instructionHeader / 100);

            return new Instruction
            {
                OpCode = (OpCode)opCode,
                First = GetParameterCount((OpCode)opCode) >= 1 ?
                    new Parameter { Mode = first, Value = instructions[offset + 1] }
                    : null ,
                Second = GetParameterCount((OpCode)opCode) >= 2 ?
                    new Parameter { Mode = second, Value = instructions[offset + 2] }
                    : null,
                Third = GetParameterCount((OpCode)opCode) >= 3 ?
                    new Parameter { Mode = third, Value = instructions[offset + 3] }
                    : null,
            };
        }

        public static int GetParameterCount(OpCode opCode)
        {
            return opCode switch
            {
                var c when
                    c == OpCode.Break => 0,
                var c when
                    c == OpCode.Save ||
                    c == OpCode.Read => 1,
                var c when
                    c == OpCode.JumpFalse ||
                    c == OpCode.JumpTrue => 2,
                var c when
                    c == OpCode.Multiply ||
                    c == OpCode.Add ||
                    c == OpCode.LessThan ||
                    c == OpCode.Equals => 3,
                _ => throw new Exception($"unhandled op code: {opCode}")
            };
        }

        public static ParameterMode ParseParameterMode(int mode)
        {
            if (!Enum.IsDefined(typeof(ParameterMode), mode))
                throw new Exception($"Invalid parameter mode: {mode}");
            return (ParameterMode)mode;
        }
    }

    public class Parameter
    {
        public ParameterMode Mode { get; set; }
        public int Value { get; set; }

        public int GetValue(int[] opCodes, int offset) =>
            Mode switch
            {
                ParameterMode.Immediate => Value,
                ParameterMode.Position => opCodes[opCodes[offset]]
            };
    }
}
