using CHIP8Manager.Core.Software;
using CHIP8Manager.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CHIP8Manager.Windows
{
    /// <summary>
    /// Builds the CHIP-8 core window, which contains the virtual display of the CHIP-8.
    /// </summary>
    public class CoreVMWindow : Window
    {
        public IEmulationOperator Chip8CoreInstance { get; set; }

        private DebugVMWindow debugVMWindow;
        private KeypadVMWindow keypadVMWindow;

        public CoreVMWindow(IEmulationOperator chip8CoreInstance, EmulatorSettings emulatorSettings)
        {
            this.Chip8CoreInstance = chip8CoreInstance;

            this.Chip8CoreInstance.EMSetKeyListener(this);

            Image displayImage = new Image
            {
                Width = emulatorSettings.chip8PixelWidth * emulatorSettings.emulatorResolutionScale,
                Height = emulatorSettings.chip8PixelHeight * emulatorSettings.emulatorResolutionScale
            };

            this.debugVMWindow = new DebugVMWindow(this.Chip8CoreInstance as IEmulationOperator);
            this.keypadVMWindow = new KeypadVMWindow(this.Chip8CoreInstance as IEmulationOperator, this.Chip8CoreInstance as IKeyboardListener);

            this.Title = emulatorSettings.emulatorWindowTitle;
            this.Background = new SolidColorBrush(emulatorSettings.emulatorBorderColor);
            this.SnapsToDevicePixels = true;
            this.Width = emulatorSettings.emulatorWindowWidth;
            this.Height = emulatorSettings.emulatorWindowHeight;
            this.SizeToContent = SizeToContent.WidthAndHeight;

            this.SizeChanged += (sender, e) =>
            {
                displayImage.Width = this.Width;
                displayImage.Height = this.Height;
            };

            this.Closed += (sender, e) =>
            {
                this.debugVMWindow.Close();
                this.keypadVMWindow.Close();
                this.Chip8CoreInstance.EMStop();
            };

            this.Content = displayImage; 
            this.Chip8CoreInstance.EMSetDisplayImage(ref displayImage);
            this.Show();
        }

        public void OpenCoreWindow()
        {
            this.Show();
        }

        public void HideCoreWindow()
        {
            this.Hide();
        }

        public void OpenVirtualKeypadWindow()
        {
            this.keypadVMWindow.Show();
        }

        public void CloseVirtualKeypadWindow()
        {
            this.keypadVMWindow.Hide();
        }

        public void OpenDebugWindow()
        {
            this.debugVMWindow.Show();
        }

        public void CloseDebugWindow()
        {
            this.debugVMWindow.Hide();
        }
    }
}
