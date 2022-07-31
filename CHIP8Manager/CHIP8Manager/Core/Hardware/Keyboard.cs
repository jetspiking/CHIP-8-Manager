using System;

namespace CHIP8Manager.Core.Hardware
{
    /// <summary>
    /// Provides an implementation for the CHIP-8 keypad.
    /// This class is not responsible for actually handling the key input.
    /// 1  2  3  C
    /// 4  5  6  D
    /// 7  8  9  E
    /// A  0  B  F
    /// </summary>
    public class Keyboard
    {
        public Boolean[] Keys;

        public Keyboard(Byte keys)
        {
            this.Keys = new Boolean[keys];
        }

        public void Press(Byte key)
        {
            this.Keys[key] = true;
        }

        public void Release(Byte key)
        {
            this.Keys[key] = false;
        }

        public Boolean IsPressed(Byte key)
        {
            return this.Keys[key];
        }

        public Boolean IsReleased(Byte key)
        {
            return this.Keys[key];
        }
    }
}
