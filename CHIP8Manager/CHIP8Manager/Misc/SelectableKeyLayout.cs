using System.Windows.Input;

namespace CHIP8Manager.Misc
{
    /// <summary>
    /// Provides an implementation for the CHIP-8 key layouts that can be recognized.
    /// Literal (0 - 16, literal map):
    /// 1 2 3 4 5 6 7 8 9 0
    ///     E
    /// A   D F
    ///       C  B
    /// Intuitive:
    /// 1 2 3 4
    /// Q W E R
    /// A S D F
    /// Z X C V
    /// </summary>
    public static class SelectableKeyLayout
    {
        public static Key[] GetSelectableKeyLayout(KeyLayouts keyLayout)
        {
            switch (keyLayout)
            {
                case KeyLayouts.Intuitive:
                    return Config.VALUE_EMULATOR_KEYMAP_INTUITIVE;
                    break;
                case KeyLayouts.Literal:
                    return Config.VALUE_EMULATOR_KEYMAP_TRUE;
                    break;
                default:
                    return Config.VALUE_EMULATOR_KEYMAP_TRUE;
            }
        }
    }
}
