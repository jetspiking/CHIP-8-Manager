using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace CHIP8Manager.Utilities
{
    /// <summary>
    /// Provides the ability to load an image from inside the project.
    /// </summary>
    internal class ImageUtilities
    {
        internal static BitmapImage GetBitmapImage(Images image, Extensions extension)
        {
            return new BitmapImage(BuildImageUri(image, extension));
        }
        private static Uri BuildImageUri(Images image, Extensions extension)
        {
            return BuildPackUri(Path.Join("/Resources", $"{image}.{extension.ToString().ToLowerInvariant()}"));
        }
        private static Uri BuildPackUri(String postFix)
        {
            return new Uri(Path.Join("pack://application:,,,", postFix));
        }
    }
}
