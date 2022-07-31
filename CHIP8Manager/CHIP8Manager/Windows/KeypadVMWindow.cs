using CHIP8Manager.Core.Software;
using CHIP8Manager.Misc;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CHIP8Manager.Windows
{
    /// <summary>
    /// Builds the CHIP-8 keypad window, which can be used to generate key input for the CHIP-8 without using a keyboard.
    /// </summary>
    public class KeypadVMWindow : Window
    {
        private Grid keypad;
        private Color foregroundColor;
        private Color backgroundColor;
        private IKeyboardListener keyboardListener;

        public KeypadVMWindow(IEmulationOperator chip8CoreWindow, IKeyboardListener keyboardListener)
        {
            this.foregroundColor = chip8CoreWindow.EMGetEmulatorSettings().emulatorForegroundColor;
            this.backgroundColor = chip8CoreWindow.EMGetEmulatorSettings().emulatorBackgroundColor;
            this.keyboardListener = keyboardListener;
            this.Background = new SolidColorBrush(this.backgroundColor);
            this.keypad = new Grid();
            this.ResizeMode = ResizeMode.NoResize;
            this.Content = this.keypad;
            this.Title = $"{Config.TEXT_MANAGER_ACTION_COLUMN_KEYPAD} {chip8CoreWindow.EMGetEmulatorSettings().emulatorWindowTitle}";

            this.SizeToContent = SizeToContent.WidthAndHeight;

            for (Byte b = 0; b < 4; b++)
            {
                this.keypad.ColumnDefinitions.Add(new ColumnDefinition());
                this.keypad.RowDefinitions.Add(new RowDefinition());
            }

            BuildKeypadView(0, 0, "1", 0x01);
            BuildKeypadView(1, 0, "2", 0x02);
            BuildKeypadView(2, 0, "3", 0x03);
            BuildKeypadView(3, 0, "C", 0x0C);
            BuildKeypadView(0, 1, "4", 0x04);
            BuildKeypadView(1, 1, "5", 0x05);
            BuildKeypadView(2, 1, "6", 0x06);
            BuildKeypadView(3, 1, "D", 0x0D);
            BuildKeypadView(0, 2, "7", 0x07);
            BuildKeypadView(1, 2, "8", 0x08);
            BuildKeypadView(2, 2, "9", 0x09);
            BuildKeypadView(3, 2, "E", 0x0E);
            BuildKeypadView(0, 3, "A", 0x0A);
            BuildKeypadView(1, 3, "0", 0x00);
            BuildKeypadView(2, 3, "B", 0x0B);
            BuildKeypadView(3, 3, "F", 0x0F);

            this.Show();
        }

        private void BuildKeypadView(int gridPosX, int gridPosY, String keypadText, Byte characterByte)
        {
            TextBlock keypadButton = new TextBlock();
            keypadButton.Text = keypadText;
            keypadButton.Foreground = new SolidColorBrush(this.foregroundColor);
            keypadButton.Width = Config.VALUE_KEYPAD_WINDOW_WIDTH_AND_HEIGHT;
            keypadButton.Height = Config.VALUE_KEYPAD_WINDOW_WIDTH_AND_HEIGHT;
            keypadButton.FontSize = 20;
            keypadButton.FontWeight = FontWeights.Bold;
            keypadButton.HorizontalAlignment = HorizontalAlignment.Center;
            keypadButton.VerticalAlignment = VerticalAlignment.Center;
            Border keypadBorder = new Border();
            keypadBorder.BorderBrush = new SolidColorBrush(this.foregroundColor);
            keypadBorder.BorderThickness = new Thickness(Config.VALUE_MANAGER_BORDER_THICKNESS, Config.VALUE_MANAGER_BORDER_THICKNESS, Config.VALUE_MANAGER_BORDER_THICKNESS, Config.VALUE_MANAGER_BORDER_THICKNESS);
            keypadBorder.Child = keypadButton;
            keypadBorder.MouseDown += (sender, e) =>
              {
                  keyboardListener.Pressed(characterByte);
              };
            keypadBorder.MouseUp += (sender, e) =>
            {
                keyboardListener.Released(characterByte);
            };
            this.keypad.Children.Add(keypadBorder);
            Grid.SetColumn(keypadBorder, gridPosX);
            Grid.SetRow(keypadBorder, gridPosY);
        }
    }
}
