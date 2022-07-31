using System;
using System.Windows.Input;
using System.Windows.Media;

namespace CHIP8Manager.Misc
{
    /// <summary>
    /// Provides an implementation for default values used inside the application.
    /// </summary>
    public static class Config
    {
        public const String TEXT_MANAGER_WINDOW_TITLE = "VM CHIP-8 Manager";
        public const String TEXT_MANAGER_TITLE = "Title";
        public const String TEXT_MANAGER_STATE = "State";
        public const String TEXT_MANAGER_ROM = "ROM";
        public const String TEXT_MANAGER_FREQUENCY = "Frequency";
        public const String TEXT_MANAGER_STATE_RUNNING = "Running";
        public const String TEXT_MANAGER_STATE_STOPPED = "Stopped";
        public const String TEXT_MANAGER_ACTION_START = "Start";
        public const String TEXT_MANAGER_ACTION_STOP = "Stop";
        public const String TEXT_MANAGER_ACTION_SHOW_KEYPAD = "Show keypad";
        public const String TEXT_MANAGER_ACTION_HIDE_KEYPAD = "Hide keypad";
        public const String TEXT_MANAGER_ACTION_SHOW_DISPLAY = "Show display";
        public const String TEXT_MANAGER_ACTION_HIDE_DISPLAY = "Hide display";
        public const String TEXT_MANAGER_ACTION_SHOW_DEBUG = "Show debug";
        public const String TEXT_MANAGER_ACTION_HIDE_DEBUG = "Hide debug";
        public const String TEXT_MANAGER_ACTION_IMPORT = "Import";
        public const String TEXT_MANAGER_ACTION_EXPORT = "Export";
        public const String TEXT_MANAGER_ACTION_SETTINGS = "Settings";
        public const String TEXT_MANAGER_ACTION_NEW_VM = "New VM";
        public const String TEXT_EMULATOR_WINDOW_TITLE = "CHIP-8 Interpreter";
        public const String TEXT_VM_OVERVIEW = "Virtual Machines";
        public const String TEXT_MANAGER_ACTION_COLUMN_OPERATE = "Operate";
        public const String TEXT_MANAGER_ACTION_COLUMN_KEYPAD = "Keypad";
        public const String TEXT_MANAGER_ACTION_COLUMN_DISPLAY = "Display";
        public const String TEXT_MANAGER_ACTION_COLUMN_DEBUG = "Debug";
        public const String TEXT_MANAGER_ACTION_COLUMN_DATA = "Data";
        public const String TEXT_MANAGER_ACTION_COLUMN_CONFIGURE = "Configure";
        public const String TEXT_MANAGER_ACTION_VM_NEW = "New";
        public const String TEXT_MANAGER_ACTION_VM_DELETE = "Delete";
        public const String TEXT_MANAGER_ACTION_VM_IMPORT = "Import";
        public const String TEXT_MANAGER_ACTION_VM_EXPORT = "Export";
        public const String TEXT_MANAGER_ACTION_VM_BOOT = "Boot";
        public const String TEXT_MANAGER_ACTION_VM_STOP = "Stop";
        public const String TEXT_MANAGER_ACTION_VM_SETTINGS = "Settings";
        public const String TEXT_APP_DIALOG_ERROR = "Error";
        public const String TEXT_CONFIG_HEX_OUTPUT_NIBBLE = "X1";
        public const String TEXT_CONFIG_HEX_OUTPUT_BYTE = "X2";
        public const String TEXT_CONFIG_HEX_OUTPUT_SHORT = "X4";
        public const String TEXT_NEWVM_NAME = "Name";
        public const String TEXT_NEWVM_DEFAULT_NAME = "My CHIP-8 emulator";
        public const String TEXT_NEWVM_DEFAULT_DELAY = "16";
        public const String TEXT_NEWVM_DELAY_TIMER_DURATION = "Delay timer (ms)";
        public const String TEXT_NEWVM_ROM = "ROM";
        public const String TEXT_NEWVM_BROWSE = "Browse";
        public const String TEXT_NEWVM_COLORFG = "Foreground";
        public const String TEXT_NEWVM_COLORBG = "Background";
        public const String TEXT_NEWVM_VIRTUALMACHINE = "CHIP-8";
        public const String TEXT_NEWVM_ADD = "Save";
        public const String TEXT_DEBUG_MEMORY_NEXT = "Next";
        public const String TEXT_DEBUG_MEMORY_PREVIOUS = "Previous";
        public const String TEXT_EMULATOR_DEFAULT_ROM_NAME = "OK";

        public static readonly Int32 VALUE_KEYPAD_WINDOW_WIDTH_AND_HEIGHT = 50;
        public static readonly Int32 VALUE_MANAGER_WINDOW_WIDTH = 800;
        public static readonly Int32 VALUE_MANAGER_WINDOW_HEIGHT = 512;
        public static readonly Int32 VALUE_MANAGER_WINDOW_MARGIN = 5;
        public static readonly Int32 VALUE_MANAGER_BORDER_THICKNESS = 2;
        public static readonly Color VALUE_MANAGER_BORDER_COLOR = Colors.Black;
        public static readonly Int32 VALUE_MANAGER_VMTABLE_COLUMN_WIDTH = 60;
        public static readonly Int32 VALUE_SPLASHSCREEN_WINDOW_WIDTH = 256;
        public static readonly Int32 VALUE_SPLASHSCREEN_WINDOW_HEIGHT = 256;
        public static readonly Int32 VALUE_DEBUG_WINDOW_HEIGHT = 400;
        public static readonly Color VALUE_EMULATOR_FOREGROUND_COLOR = Colors.Blue;
        public static readonly Color VALUE_EMULATOR_BACKGROUND_COLOR = Colors.Black;
        public static readonly Color VALUE_EMULATOR_BORDER_COLOR = Colors.DarkGray;
        public static readonly Int32 VALUE_EMULATOR_WINDOW_WIDTH = 512;
        public static readonly Int32 VALUE_EMULATOR_WINDOW_HEIGHT = VALUE_EMULATOR_WINDOW_WIDTH/2;
        public static readonly Byte VALUE_EMULATOR_RESOLUTION_SCALE = 5;
        public static readonly UInt16 VALUE_EMULATOR_DELAY_DURATION = 1000/60;
        public static readonly Byte[] VALUE_EMULATOR_ROM = { /*K at 0,0*/ 0xA0, 0x00, 0x60, 0x00, 0x61, 0x00, 0xD0, 0x15, /*K at 0,0*/ 0xA0, 0x50, 0x62, 0x07, 0x63, 0x00, 0xD2, 0x35, /*Loop*/ 0xF0, 0x0A, 0x12, 0x00  };
        public static readonly Key[] VALUE_EMULATOR_KEYMAP_TRUE =
        {
            Key.D0, Key.D1, Key.D2, Key.D3,
            Key.D4, Key.D5, Key.D6, Key.D7,
            Key.D8, Key.D9, Key.A, Key.B,
            Key.C, Key.D, Key.E, Key.F
        };
        public static readonly Key[] VALUE_EMULATOR_KEYMAP_INTUITIVE =
        {
            Key.D1, Key.D2, Key.D3, Key.D4,
            Key.Q, Key.W, Key.E, Key.R,
            Key.A, Key.S, Key.D, Key.F,
            Key.Z, Key.X, Key.C, Key.V
        };
        public static readonly Byte[] VALUE_EMULATOR_CHARACTER_SET = 
        {
            0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
            0x20, 0x60, 0x20, 0x20, 0x70, // 1
            0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
            0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
            0x90, 0x90, 0xF0, 0x10, 0x10, // 4
            0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
            0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
            0xF0, 0x10, 0x20, 0x40, 0x40, // 7
            0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
            0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
            0xF0, 0x90, 0xF0, 0x90, 0x90, // A
            0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
            0xF0, 0x80, 0x80, 0x80, 0xF0, // C
            0xE0, 0x90, 0x90, 0x90, 0xE0, // D
            0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
            0xF0, 0x80, 0xF0, 0x80, 0x80, // F
            0x90, 0xA0, 0xC0, 0xA0, 0x90  // K
        };
        public static readonly Byte VALUE_CHIP8_PIXEL_WIDTH = 64;
        public static readonly Byte VALUE_CHIP8_PIXEL_HEIGHT = 32;
        public static readonly UInt16 VALUE_CHIP8_MEMORY_SIZE = 4096;
        public static readonly Byte VALUE_CHIP8_DATA_REGISTERS = 16;
        public static readonly Byte VALUE_CHIP8_STACK_DEPTH = 16;
        public static readonly Byte VALUE_CHIP8_KEYS_AMOUNT = 16;
        public static readonly Byte VALUE_CHIP8_CHARACTER_SET_LOAD_ADDRESS = 0x00;
        public static readonly UInt16 VALUE_CHIP8_PROGRAM_LOAD_ADDRESS = 0x200;
        public static readonly Byte VALUE_CHIP8_DEFAULT_SPRITE_HEIGHT = 5;
    }
}
