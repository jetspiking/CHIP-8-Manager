using System;

namespace CHIP8Manager.Core.Hardware
{
    /// <summary>
    /// Provides an implementation for the CHIP-8 display. 
    /// This class is not responsible for actually displaying the data.
    /// </summary>
    public class Display
    {
        public Boolean[,] Pixels;
        public Byte Width;
        public Byte Height;

        public Display(Byte width, Byte height)
        {
            this.Pixels = new Boolean[width, height];
            this.Width = width;
            this.Height = height;
        }

        public void Set(Byte x, Byte y)
        {
            this.Pixels[x, y] = true;
        }

        public Boolean IsSet(Byte x, Byte y)
        {
            return this.Pixels[x, y];
        }

        public void ClearScreen()
        {
            for (Byte x = 0; x < this.Width; x++)
                for (Byte y = 0; y < this.Height; y++)
                    this.Pixels[x, y] = false;
        }

        public Boolean DrawSprite(Byte x, Byte y, ref Byte[] sprite, UInt16 start, UInt16 length)
        {
            Boolean isOverwritten = false;

            for (UInt16 ly = 0; ly < length; ly++)
            {
                UInt16 b = sprite[ly+start];
                for (Byte lx = 0; lx < 8; lx++)
                {
                    if ((b & (0x80 >> lx)) == 0)
                        continue;

                    if (this.Pixels[(lx + x) % this.Width, (ly + y) % this.Height])
                        isOverwritten = true;

                    this.Pixels[(lx + x) % this.Width, (ly + y) % this.Height] ^= true;
                }
            }
            return isOverwritten;
        }
    }
}
