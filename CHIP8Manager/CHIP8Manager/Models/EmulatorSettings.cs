using CHIP8Manager.Misc;
using System;
using System.Windows.Input;
using System.Windows.Media;

namespace CHIP8Manager.Models
{
    /// <summary>
    /// Provides an implementation for the CHIP-8 emulator settings.
    /// </summary>
    public class EmulatorSettings
    {
        public String emulatorWindowTitle = Config.TEXT_EMULATOR_WINDOW_TITLE;
        public Color emulatorForegroundColor = Config.VALUE_EMULATOR_FOREGROUND_COLOR;
        public Color emulatorBackgroundColor = Config.VALUE_EMULATOR_BACKGROUND_COLOR;
        public Color emulatorBorderColor = Config.VALUE_EMULATOR_BORDER_COLOR;
        public Int32 emulatorWindowWidth = Config.VALUE_EMULATOR_WINDOW_WIDTH;
        public Int32 emulatorWindowHeight = Config.VALUE_EMULATOR_WINDOW_HEIGHT;
        public Byte emulatorResolutionScale = Config.VALUE_EMULATOR_RESOLUTION_SCALE;
        public UInt16 emulatorDelayDuration = Config.VALUE_EMULATOR_DELAY_DURATION;
        public Byte[] emulatorCharacterSet = Config.VALUE_EMULATOR_CHARACTER_SET;
        public Byte[] emulatorRom = Config.VALUE_EMULATOR_ROM;
        public Key[] emulatorKeyMap = Config.VALUE_EMULATOR_KEYMAP_TRUE;
        public Byte chip8PixelWidth = Config.VALUE_CHIP8_PIXEL_WIDTH;
        public Byte chip8PixelHeight = Config.VALUE_CHIP8_PIXEL_HEIGHT;
        public UInt16 chip8MemorySize = Config.VALUE_CHIP8_MEMORY_SIZE;
        public Byte chip8DataRegisters = Config.VALUE_CHIP8_DATA_REGISTERS;
        public Byte chip8StackDepth = Config.VALUE_CHIP8_STACK_DEPTH;
        public Byte chip8KeyAmount = Config.VALUE_CHIP8_KEYS_AMOUNT;
        public Byte chip8CharacterSetLoadAddress = Config.VALUE_CHIP8_CHARACTER_SET_LOAD_ADDRESS;
        public UInt16 chip8ProgramLoadAddress = Config.VALUE_CHIP8_PROGRAM_LOAD_ADDRESS;
        public Byte chip8DefaultSpriteHeight = Config.VALUE_CHIP8_DEFAULT_SPRITE_HEIGHT;


    }
}
