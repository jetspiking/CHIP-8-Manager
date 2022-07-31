using CHIP8Manager.Core.Software;
using System;
using System.Windows;
using System.Windows.Input;

namespace CHIP8Manager.Core.Emulation
{
    /// <summary>
    /// Maps physical key input (from a keyboard) to the CHIP-8 keypad.
    /// </summary>
    public class PhysicalKeyboardMapper
    {
        public PhysicalKeyboardMapper(Window window, Key[] keyMap, IKeyboardListener keyboardListener)
        {
            window.KeyDown += (sender, e) =>
            {
                for (Byte key = 0; key < keyMap.Length; key++)
                    if (keyMap[key] == e.Key)
                        keyboardListener.Pressed(key);
            };

            window.KeyUp += (sender, e) =>
            {
                for (Byte key = 0; key < keyMap.Length; key++)
                    if (keyMap[key] == e.Key)
                        keyboardListener.Released(key);
            };
        }
    }
}
