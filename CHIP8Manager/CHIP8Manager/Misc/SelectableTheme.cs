using System.Windows.Media;

namespace CHIP8Manager.Misc
{
    /// <summary>
    /// Provides an implementation for the CHIP-8 key themes that can be chosen.
    /// </summary>
    public static class SelectableTheme
    {
        private static readonly Color Black = Color.FromRgb(0, 0, 0);
        private static readonly Color White = Color.FromRgb(255, 255, 255);
        private static readonly Color Apple = Color.FromRgb(228, 13, 0);
        private static readonly Color Banana = Color.FromRgb(228, 210, 0);
        private static readonly Color Blackberry = Color.FromRgb(75, 75, 75);
        private static readonly Color Blueberry = Color.FromRgb(0, 49, 228);
        private static readonly Color Raspberry = Color.FromRgb(228, 0, 147);
        private static readonly Color Lime = Color.FromRgb(19, 228, 0);
        private static readonly Color Mango = Color.FromRgb(228, 138, 0);
        private static readonly Color Plum = Color.FromRgb(149, 0, 228);
        private static readonly Color Coconut = Color.FromRgb(0, 0, 0);

        public static Color GetSelectableTheme(Themes theme)
        {
            switch (theme)
            {
                case Themes.Black: return Black;
                case Themes.White: return White;
                case Themes.Apple: return Apple;
                case Themes.Banana: return Banana;
                case Themes.Blackberry: return Blackberry;
                case Themes.Blueberry: return Blueberry;
                case Themes.Raspberry: return Raspberry;
                case Themes.Lime: return Lime;
                case Themes.Mango: return Mango;
                case Themes.Plum: return Plum;
                case Themes.Coconut: return Coconut;
                default: return Blackberry;
            }
        }
    }
}
