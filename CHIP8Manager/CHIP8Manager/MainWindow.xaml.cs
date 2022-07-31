using CHIP8Manager.Misc;
using CHIP8Manager.Windows;
using System.Windows;
using System.Windows.Media;

namespace CHIP8Manager
{
    /// <summary>
    /// Creates the virtual machine manager window.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Title = Config.TEXT_MANAGER_WINDOW_TITLE;
            this.Background = new SolidColorBrush(Config.VALUE_EMULATOR_BORDER_COLOR);
            this.SnapsToDevicePixels = true;
            this.Width = Config.VALUE_SPLASHSCREEN_WINDOW_WIDTH;
            this.Height = Config.VALUE_SPLASHSCREEN_WINDOW_HEIGHT;
            this.Visibility = Visibility.Hidden;
            new ManagerWindow();
        }
    }
}
