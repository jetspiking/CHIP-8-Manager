using System;

namespace CHIP8Manager.Core.Hardware
{
    /// <summary>
    /// Provides an implementation for the CHIP-8 registries.
    /// V-register - V
    /// I-register - I
    /// Stack or StackSegment - SS
    /// DelayTimer - DT
    /// SoundTimer - ST
    /// ProgramCounter - PC
    /// StackPointer - SP
    /// </summary>
    public class Registry
    {
        public Byte[] V;    // V register
        public UInt16 I;    // I register
        public UInt16[] SS; // Stack Segment
        public Byte DT;     // Delay Timer
        public Byte ST;     // Sound Timer
        public UInt16 PC;   // Program Counter
        public Byte SP;     // Stack Pointer

        public Registry(Byte V, Byte S)
        {
            this.V = new Byte[V];
            this.SS = new UInt16[S];
        }

        public void Push(UInt16 value)
        {
            this.SS[this.SP] = value;
            this.SP++;
        }

        public UInt16 Pop()
        {
            this.SP--;
            return this.SS[this.SP];
        }
    }
}
