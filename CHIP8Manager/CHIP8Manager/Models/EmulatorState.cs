using CHIP8Manager.Core.Hardware;
using CHIP8Manager.Misc;
using System;

namespace CHIP8Manager.Models
{
    /// <summary>
    /// Provides an implementation for an entire CHIP-8 machine for import and export purposes (load and save).
    /// </summary>
    public class EmulatorState
    {
        public String? Name { get; set; }
        public Memory? Memory { get; set; }
        public Registry? Registry { get; set; }
        public Keyboard? Keyboard { get; set; }
        public Display? Display { get; set; }
        public Byte[]? Rom { get; set; } = Config.VALUE_EMULATOR_ROM;
        public String? RomName { get; set; } = Config.TEXT_EMULATOR_DEFAULT_ROM_NAME;
        public UInt16? DelayDuration { get; set; }
        public KeyLayouts? EmulatorKeyMap { get; set; }
        public Themes? ForegroundColor { get; set; }
        public Themes? BackgroundColor { get; set; }
    }
}
