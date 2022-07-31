using CHIP8Manager.Misc;
using System.Windows;

namespace CHIP8Manager
{
    /// <summary>
    /// Main application entry point.
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
#if !DEBUG
            this.Dispatcher.UnhandledException += (sender, e) =>
            {
                //MessageBox.Show(e.Exception.Message, Config.TEXT_APP_DIALOG_ERROR, MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
            };
#endif
        }
    }
}
