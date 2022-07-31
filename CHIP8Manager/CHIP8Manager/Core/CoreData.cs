using CHIP8Manager.Core.Hardware;
using CHIP8Manager.Misc;
using System;

namespace CHIP8Manager.Core
{
    /// <summary>
    /// Provides an implementation for the CHIP-8 core internals.
    /// </summary>
    public class CoreData
    {
        public Memory Memory { get; set; }
        public Registry Registry { get; set; }
        public Keyboard Keyboard { get; set; }
        public Display Display { get; set; }
        public Boolean CanWaitForKeyInput { get; set; } = true;

        public void OpCodeUpdate()
        {
            UInt16 opCode = this.Memory.GetShort(this.Registry.PC);
            this.Registry.PC += 2;
            this.Execute(opCode);
        }

        private void Execute(UInt16 opCode)
        {
            switch (opCode)
            {
                // CLS: Clear the screen
                case 0x00E0:
                    this.Display.ClearScreen();
                    break;
                // RET: Return from a subroutine
                case 0x00EE:
                    this.Registry.PC = this.Registry.Pop();
                    break;
                // Handle extended instruction set
                default:
                    ExecuteExtended(opCode);
                    break;
            }
        }

        private void ExecuteExtended(UInt16 opCode)
        {
            UInt16 nnn = (UInt16)(opCode & 0x0FFF);
            UInt16 x = (UInt16)((opCode >> 8) & 0x000F);
            UInt16 y = (UInt16)((opCode >> 4) & 0x000F);
            Byte kk = (Byte)(opCode & 0x00FF);
            Byte n = (Byte)(opCode & 0x000F);
            switch (opCode & 0xF000)
            {
                case 0x0:
                    break;
                // JP addr: Jump to location nnn.
                case 0x1000:
                    this.Registry.PC = nnn;
                    break;
                // CALL addr: Call subroutine at location nnnn.
                case 0x2000:
                    this.Registry.Push(this.Registry.PC);
                    this.Registry.PC = nnn;
                    break;
                // SE Vx, byte: Skip next instruction if Vx == kk.
                case 0x3000:
                    if (this.Registry.V[x] == kk)
                        this.Registry.PC += 2;
                    break;
                // SNE Vx, byte: Skip next instruction if Vx != kk.
                case 0x4000:
                    if (this.Registry.V[x] != kk)
                        this.Registry.PC += 2;
                    break;
                // SE Vx, Vy: Skip next instruction if Vx == Vy.
                case 0x5000:
                    if (this.Registry.V[x] == this.Registry.V[y])
                        this.Registry.PC += 2;
                    break;
                // LD Vx, byte: Set Vx = kk.
                case 0x6000:
                    this.Registry.V[x] = kk;
                    break;
                // ADD Vx, byte: Set Vx = Vx + kk.
                case 0x7000:
                    this.Registry.V[x] += kk;
                    break;
                // Extended instructions for most significant nibble equal to 0x8.
                case 0x8000:
                    ExecuteExtended8(opCode, x, y);
                    break;
                // SNE Vx, Vy: Skip next instruction if Vx != Vy.
                case 0x9000:
                    if (this.Registry.V[x] != this.Registry.V[y])
                        this.Registry.PC += 2;
                    break;
                // LD I, addr: Set I = nnn.
                case 0xA000:
                    this.Registry.I = nnn;
                    break;
                // JP V0, addr: Jump to location nnn + V0.
                case 0xB000:
                    this.Registry.PC = (UInt16)(nnn + this.Registry.V[0x00]);
                    break;
                // RND Vx, byte: Set Vx = random byte AND kk.
                case 0xC000:
                    Random random = new Random();
                    Byte randomNumber = (Byte)random.Next(255);
                    this.Registry.V[x] = (Byte)(randomNumber & kk);
                    break;
                // DRW Vx, Vy, nibble: Display n-byte sprite starting at memory location I at (Vx, Vy), set VF = collision.
                case 0xD000:
                    Boolean spriteResult = this.Display.DrawSprite(this.Registry.V[x], this.Registry.V[y], ref this.Memory.RAM, this.Registry.I, n);
                    this.Registry.V[0x0F] = spriteResult ? (Byte)1 : (Byte)0;
                    break;
                // Extended instructions for most significant nibble equal to 0xE.
                case 0xE000:
                    ExecuteExtendedE(opCode, x, y);
                    break;
                // Extended instructions for most significant nibble equal to 0xF.
                case 0xF000:
                    ExecuteExtendedF(opCode, x, y);
                    break;
            }
        }

        private void ExecuteExtended8(UInt16 opCode, UInt16 x, UInt16 y)
        {
            Byte leastSignificantNibble = (Byte)(opCode & 0x000F);
            switch (leastSignificantNibble)
            {
                // LD Vx, Vy: Set Vx = Vy.
                case 0x00:
                    this.Registry.V[x] = this.Registry.V[y];
                    break;
                // OR Vx, Vy: Set Vx = Vx OR Vy
                case 0x01:
                    this.Registry.V[x] |= this.Registry.V[y];
                    break;
                // AND Vx, Vy: Set Vx = Vx AND Vy
                case 0x02:
                    this.Registry.V[x] &= this.Registry.V[y];
                    break;
                // XOR Vx, Vy: Set Vx = Vx XOR Vy
                case 0x03:
                    this.Registry.V[x] ^= this.Registry.V[y];
                    break;
                // ADD Vx, Vy: Set Vx = Vx + Vy, set VF = carry.
                case 0x04:
                    UInt16 sum = (UInt16)(this.Registry.V[x] + this.Registry.V[y]);
                    this.Registry.V[0x0F] = 0;
                    if (sum > 255)
                    {
                        this.Registry.V[0x0F] = 1;
                        sum &= 0x00FF;
                    }
                    this.Registry.V[x] = (Byte)sum;
                    break;
                // SUB Vx, Vy:  Set Vx = Vx - Vy, set VF = NOT borrow.
                case 0x05:
                    this.Registry.V[0x0F] = this.Registry.V[x] >= this.Registry.V[y] ? (Byte)1 : (Byte)0;
                    this.Registry.V[x] -= this.Registry.V[y];
                    break;
                // SHR Vx {, Vy}: Set Vx = Vx SHR 1.
                case 0x06:
                    this.Registry.V[0x0F] = (Byte)(this.Registry.V[x] & 0x01);
                    this.Registry.V[x] /= 2;
                    break;
                // SUBN Vx, Vy: Set Vx = Vy - Vx, set VF = NOT borrow.
                case 0x07:
                    this.Registry.V[0x0F] = this.Registry.V[y] >= this.Registry.V[x] ? (Byte)1 : (Byte)0;
                    this.Registry.V[x] = (Byte)(this.Registry.V[y] - this.Registry.V[x]);
                    break;
                // SHL Vx {, Vy}: Set Vx = Vx SHL 1.
                case 0x0E:
                    this.Registry.V[0x0F] = (this.Registry.V[x] >> 7) == 1 ? (Byte)1 : (Byte)0;
                    this.Registry.V[x] *= 2;
                    break;
            }
        }

        private void ExecuteExtendedE(UInt16 opCode, UInt16 x, UInt16 y)
        {
            Byte leastSignificantByte = (Byte)(opCode & 0x00FF);
            switch (leastSignificantByte)
            {
                // SKP Vx: Skip next instruction if key with the value of Vx is pressed.
                case 0x9E:
                    if (this.Keyboard.IsPressed(this.Registry.V[x]))
                        this.Registry.PC += 2;
                    break;
                case 0xA1:
                    // SKNP Vx: Skip next instruction if key with the value of Vx is not pressed.
                    if (!this.Keyboard.IsPressed(this.Registry.V[x]))
                        this.Registry.PC += 2;
                    break;
            }
        }

        private void ExecuteExtendedF(UInt16 opCode, UInt16 x, UInt16 y)
        {
            Byte leastSignificantByte = (Byte)(opCode & 0x00FF);
            switch (leastSignificantByte)
            {
                // LD Vx, DT: Set Vx = delay timer value.
                case 0x07:
                    this.Registry.V[x] = this.Registry.DT;
                    break;
                // LD Vx, K: Wait for a key press, store the value of the key in Vx.
                case 0x0A:
                    while (this.CanWaitForKeyInput)
                    {
                        for (Byte key = 0; key < this.Keyboard.Keys.Length; key++)
                            if (Keyboard.Keys[key] == true)
                            {
                                this.Registry.V[x] = 1;
                                break;
                            }
                        if (this.Registry.V[x] == 1) break;
                    }
                    break;
                // LD DT, Vx: Set delay timer = Vx.
                case 0x15:
                    this.Registry.DT = this.Registry.V[x];
                    break;
                // LD ST, Vx: Set sound timer = Vx.
                case 0x18:
                    this.Registry.ST = this.Registry.V[x];
                    break;
                // ADD I, Vx: Set I = I + Vx.
                case 0x1E:
                    this.Registry.I += this.Registry.V[x];
                    break;
                // LD F, Vx: Set I = location of sprite for digit Vx.
                case 0x29:
                    this.Registry.I = (UInt16)(this.Registry.V[x] * Config.VALUE_CHIP8_DEFAULT_SPRITE_HEIGHT);
                    break;
                // LD B, Vx: Store BCD representation of Vx in memory locations I, I+1, and I+2.
                case 0x33:
                    Byte ten2 = (Byte)(this.Registry.V[x] / 100);
                    Byte ten1 = (Byte)(this.Registry.V[x] / 10 % 10);
                    Byte ten0 = (Byte)(this.Registry.V[x] % 10);
                    this.Memory.RAM[this.Registry.I] = ten2;
                    this.Memory.RAM[this.Registry.I + 1] = ten1;
                    this.Memory.RAM[this.Registry.I + 2] = ten0;
                    break;
                // Fx55 - LD [I], Vx: Store registers V0 through Vx in memory starting at location I.
                case 0x55:
                    Buffer.BlockCopy(this.Registry.V, 0, this.Memory.RAM, this.Registry.I, x + 1);
                    break;
                // LD Vx, [I]: Read registers V0 through Vx from memory starting at location I.
                case 0x65:
                    Buffer.BlockCopy(this.Memory.RAM, this.Registry.I, this.Registry.V, 0, x + 1);
                    break;
            }
        }
    }
}
