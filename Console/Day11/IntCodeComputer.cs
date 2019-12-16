using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Console.Day11
{
    class IntCodeComputer
    {
        public IntCodeComputer(long[] instructions, long initialInput)
        {
            Input = new Queue<long>();
            Input.Enqueue(initialInput);
            Output = new Queue<long>();
            Memory = instructions.Concat(new long[10000]).ToArray();
        }

        public long Offset { get; set; } = 0;
        public long[] Memory { get; set; }
        public bool Complete { get; private set; } = false;
        public Queue<long> Output { get; }
        public Queue<long> Input { get; }
        public long LastOutput { get; set;  }
        private long _relativeBase = 0;

        long GetAddress(ParameterMode mode, long parameterOffset)
        {
            return mode switch
            {
                ParameterMode.Immediate => parameterOffset,
                ParameterMode.Position => Memory[parameterOffset],
                ParameterMode.Relative => Memory[parameterOffset] + _relativeBase,
                _ => throw new Exception($"bad mode {mode}")
            };
        }

        long GetAddress(Instruction instruction, long offset)
        {
            return offset switch
            {
                1 => GetAddress(instruction.First, Offset + 1),
                2 => GetAddress(instruction.Second, Offset + 2),
                3 => GetAddress(instruction.Third, Offset + 3),
                _ => throw new Exception("bad offset")
            };
        }

        public List<long> Run()
        {
            var results = new List<long>();
            while (Offset < Memory.Length)
            {
                var instruction = Instruction.ParseInstruction(Memory, Offset);
                long paramAddress(long offset) => GetAddress(instruction, offset);

                if (instruction.OpCode == OpCode.Add)
                {
                    Memory[paramAddress(3)] = Memory[paramAddress(1)] + Memory[paramAddress(2)];
                    Offset += instruction.Size;
                }
                else if (instruction.OpCode == OpCode.Multiply)
                {
                    Memory[paramAddress(3)] = Memory[paramAddress(1)] * Memory[paramAddress(2)];
                    Offset += instruction.Size;
                }
                else if (instruction.OpCode == OpCode.Save)
                {
                    if (Input.Count == 0) break;
                    Memory[paramAddress(1)] = Input.Dequeue();
                    Offset += instruction.Size;
                }
                else if (instruction.OpCode == OpCode.Read)
                {
                    var output = Memory[paramAddress(1)];
                    Output.Enqueue(output);
                    LastOutput = output;
                    Offset += instruction.Size;
                }
                else if (instruction.OpCode == OpCode.JumpTrue)
                {
                    if (Memory[paramAddress(1)] != 0)
                        Offset = Memory[paramAddress(2)];
                    else Offset += instruction.Size;
                }
                else if (instruction.OpCode == OpCode.JumpFalse)
                {
                    if (Memory[paramAddress(1)] == 0)
                        Offset = Memory[paramAddress(2)];
                    else Offset += instruction.Size;
                }
                else if (instruction.OpCode == OpCode.LessThan)
                {
                    if (Memory[paramAddress(1)] < Memory[paramAddress(2)])
                        Memory[paramAddress(3)] = 1;
                    else Memory[paramAddress(3)] = 0;

                    Offset += instruction.Size;
                }
                else if (instruction.OpCode == OpCode.Equals)
                {
                    if (Memory[paramAddress(1)] == Memory[paramAddress(2)])
                        Memory[paramAddress(3)] = 1;
                    else Memory[paramAddress(3)] = 0;

                    Offset += instruction.Size;
                }
                else if (instruction.OpCode == OpCode.AdjustRelativeBase)
                {
                    _relativeBase += Memory[paramAddress(1)];
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
        /// takes a single longeger as input and saves it to the position given by its only parameter. 
        /// For example, the instruction 3,50 would take an input value and store it at address 50.
        ///</summary>
        Save = 3,

        ///<summary>
        /// outputs the value of its only parameter. For example, the instruction 4,50 
        /// would output the value at address 50
        ///</summary>
        Read = 4,

        /// <summary>
        /// if the first parameter is non-zero, it sets the instruction polonger to the value
        /// from the second parameter. Otherwise, it does nothing.
        /// </summary>
        JumpTrue = 5,
        JumpFalse = 6,
        LessThan = 7,
        Equals = 8,
        AdjustRelativeBase = 9,
        Break = 99
    }

    public enum ParameterMode
    {
        Position = 0,
        Immediate = 1,
        Relative = 2
    }

    public class Instruction
    {
        public OpCode OpCode { get; set; }
        public ParameterMode First { get; set; }
        public ParameterMode Second { get; set; }
        public ParameterMode Third { get; set; }
        public long Size => OpCode switch
        {
            OpCode.Save => 2,
            OpCode.Read => 2,
            OpCode.AdjustRelativeBase => 2,
            OpCode.JumpFalse => 3,
            OpCode.JumpTrue => 3,
            OpCode.LessThan => 4,
            OpCode.Equals => 4,
            OpCode.Add => 4,
            OpCode.Multiply => 4,
            _ => throw new Exception($"invalid opcode {OpCode}")
        };

        public long GetAddress(ParameterMode mode, long[] opCodes, long offset, long currentRelativeBase) =>
            mode switch
            {
                ParameterMode.Immediate => offset,
                ParameterMode.Position => opCodes[offset],
                ParameterMode.Relative => opCodes[offset]+currentRelativeBase,
                _ => throw new ArgumentException($"Invalid mode {mode}")
            };

        public static Instruction ParseInstruction(long[] instructions, long offset)
        {
            var instructionHeader = instructions[offset];
            var opCode = instructions[offset] % 100;
            if (!Enum.IsDefined(typeof(OpCode), (int)opCode)) throw new Exception($"Invalid op code: {opCode}");

            var third = ParseParameterMode(instructionHeader / 10000);
            instructionHeader %= 10000;
            var second = ParseParameterMode(instructionHeader / 1000);
            instructionHeader %= 1000;
            var first = ParseParameterMode(instructionHeader / 100);

            return new Instruction
            {
                OpCode = (OpCode)opCode,
                First = first,
                Second = second,
                Third = third
            };
        }

        public static ParameterMode ParseParameterMode(long mode)
        {
            if (!Enum.IsDefined(typeof(ParameterMode), (int)mode))
                throw new Exception($"Invalid parameter mode: {mode}");
            return (ParameterMode)mode;
        }
    }
}
