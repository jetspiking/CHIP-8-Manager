using System;

namespace CHIP8Manager.Core.Hardware
{
    /// <summary>
    /// Provides an implementation for the CHIP-8 memory.
    /// </summary>
    public class Memory
    {
        public Byte[] RAM;

        public Memory(UInt16 size)
        {
            this.RAM = new Byte[size];
        }

        public void Set(UInt16 index, Byte value)
        {
            this.RAM[index] = value;
        }

        public Byte Get(UInt16 index)
        {
            return this.RAM[index];
        }

        public UInt16 GetShort(UInt16 index)
        {
            Byte msByte = Get(index);
            Byte lsByte = Get((UInt16)(index + 1));
            return (UInt16)(msByte << 8 | lsByte);
        }
    }
}
