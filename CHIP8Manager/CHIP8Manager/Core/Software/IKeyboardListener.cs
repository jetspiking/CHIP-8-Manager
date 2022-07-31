using System;

namespace CHIP8Manager.Core.Software
{
    /// <summary>
    /// Provides an interface for handling key input.
    /// </summary>
    public interface IKeyboardListener
    {
        void Pressed(Byte key);
        void Released(Byte key);
    }
}
