using CHIP8Manager.Core.Software;
using CHIP8Manager.Misc;
using CHIP8Manager.Utilities;
using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CHIP8Manager.Windows
{
    /// <summary>
    /// Builds the CHIP-8 debug window, which displays the core data of the CHIP-8 in realtime.
    /// </summary>
    public class DebugVMWindow : Window, IUpdatableUI
    {
        private IEmulationOperator chip8CoreWindow;
        private StackPanel registerVPanel;
        private StackPanel stackPanel;
        private StackPanel registryPanel;
        private StackPanel keyPanel;
        private StackPanel memoryPanel;
        private Int32 memoryIndex = 0;
        private Boolean runUpdateThread = true;

        public DebugVMWindow(IEmulationOperator chip8CoreWindow)
        {
            this.chip8CoreWindow = chip8CoreWindow;

            this.Title = $"{Config.TEXT_MANAGER_ACTION_COLUMN_DEBUG} {this.chip8CoreWindow.EMGetEmulatorSettings().emulatorWindowTitle}";
            this.Background = new SolidColorBrush(this.chip8CoreWindow.EMGetEmulatorSettings().emulatorBackgroundColor);
            this.SnapsToDevicePixels = true;
            this.ResizeMode = ResizeMode.NoResize;
            this.SizeToContent = SizeToContent.WidthAndHeight;

            StackPanel debugPanel = new StackPanel();
            debugPanel.Orientation = Orientation.Horizontal;
            this.Content = debugPanel;

            this.registerVPanel = new StackPanel();
            this.registerVPanel.Orientation = Orientation.Vertical;
            this.registerVPanel.Margin = new Thickness(Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN);
            debugPanel.Children.Add(this.registerVPanel);

            this.stackPanel = new StackPanel();
            this.stackPanel.Orientation = Orientation.Vertical;
            this.stackPanel.Margin = new Thickness(Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN);
            debugPanel.Children.Add(this.stackPanel);

            this.registryPanel = new StackPanel();
            this.registryPanel.Orientation = Orientation.Vertical;
            this.registryPanel.Margin = new Thickness(Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN);
            debugPanel.Children.Add(this.registryPanel);

            this.keyPanel = new StackPanel();
            this.keyPanel.Orientation = Orientation.Vertical;
            this.keyPanel.Margin = new Thickness(Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN);
            debugPanel.Children.Add(this.keyPanel);

            this.memoryPanel = new StackPanel();
            this.memoryPanel.Orientation = Orientation.Vertical;
            this.memoryPanel.Margin = new Thickness(Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN);
            debugPanel.Children.Add(this.memoryPanel);

            StackPanel memoryButtonPanel = new StackPanel();
            memoryButtonPanel.Orientation = Orientation.Vertical;
            memoryButtonPanel.Margin = new Thickness(Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN);
            debugPanel.Children.Add(memoryButtonPanel);

            TextBlock nextButton = new TextBlock();
            nextButton.Text = Config.TEXT_DEBUG_MEMORY_NEXT;
            nextButton.Foreground = new SolidColorBrush(this.chip8CoreWindow.EMGetEmulatorSettings().emulatorForegroundColor);
            nextButton.Width = 100;
            nextButton.Height = 20;
            nextButton.FontWeight = FontWeights.Bold;
            Border nextBorder = new Border();
            nextBorder.BorderBrush = new SolidColorBrush(this.chip8CoreWindow.EMGetEmulatorSettings().emulatorForegroundColor);
            nextBorder.BorderThickness = new Thickness(Config.VALUE_MANAGER_BORDER_THICKNESS, Config.VALUE_MANAGER_BORDER_THICKNESS, Config.VALUE_MANAGER_BORDER_THICKNESS, Config.VALUE_MANAGER_BORDER_THICKNESS);
            nextBorder.Child = nextButton;
            memoryButtonPanel.Children.Add(nextBorder);

            nextBorder.Margin = new Thickness(0,0,0, Config.VALUE_MANAGER_BORDER_THICKNESS);

            TextBlock previousButton = new TextBlock();
            previousButton.Text = Config.TEXT_DEBUG_MEMORY_PREVIOUS;
            previousButton.Foreground = new SolidColorBrush(this.chip8CoreWindow.EMGetEmulatorSettings().emulatorForegroundColor);
            previousButton.Width = 100;
            previousButton.Height = 20;
            previousButton.FontWeight = FontWeights.Bold;
            Border previousBorder = new Border();
            previousBorder.BorderBrush = new SolidColorBrush(this.chip8CoreWindow.EMGetEmulatorSettings().emulatorForegroundColor);
            previousBorder.BorderThickness = new Thickness(Config.VALUE_MANAGER_BORDER_THICKNESS, Config.VALUE_MANAGER_BORDER_THICKNESS, Config.VALUE_MANAGER_BORDER_THICKNESS, Config.VALUE_MANAGER_BORDER_THICKNESS);
            previousBorder.Child = previousButton;
            memoryButtonPanel.Children.Add(previousBorder);

            nextButton.MouseDown += (sender, e) =>
            {
                if (this.memoryIndex + 32 < this.chip8CoreWindow.EMGetMemory().Length)
                    this.memoryIndex += 16;
            };

            previousButton.MouseDown += (sender, e) =>
            {
                if (this.memoryIndex - 16 >= 0)
                    this.memoryIndex -= 16;
            };

            this.Closed += (sender, e) =>
             {
                 this.runUpdateThread = false;
             };

            Thread thread = new Thread(() =>
            {
                while (this.runUpdateThread)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        BuildRegisterViewPanel();
                        BuildStackViewPanel();
                        BuildRegistryViewPanel();
                        BuildKeyViewPanel();
                        BuildMemoryPanel();
                    }));
                    System.Threading.Thread.Sleep(50);
                }
            });
            thread.Start();

            this.Show();
        }

        private void BuildRegisterViewPanel()
        {
            this.registerVPanel.Children.Clear();

            GetDataPair(this.chip8CoreWindow.EMGetRegisterV(), "V").ToList().ForEach(element =>
            {
                this.registerVPanel.Children.Add(element);
            });
        }

        private void BuildStackViewPanel()
        {
            this.stackPanel.Children.Clear();

            GetDataPair(this.chip8CoreWindow.EMGetRegisterSS(), "S").ToList().ForEach(element =>
            {
                this.stackPanel.Children.Add(element);
            });
        }

        private void BuildRegistryViewPanel()
        {
            this.registryPanel.Children.Clear();
            this.registryPanel.Children.Add(GetDataPair(this.chip8CoreWindow.EMGetRegisterPC().ToString(Config.TEXT_CONFIG_HEX_OUTPUT_SHORT), "PC"));
            this.registryPanel.Children.Add(GetDataPair(this.chip8CoreWindow.EMGetOpcode().ToString(Config.TEXT_CONFIG_HEX_OUTPUT_SHORT), "OC"));
            this.registryPanel.Children.Add(GetDataPair(this.chip8CoreWindow.EMGetRegisterI().ToString(Config.TEXT_CONFIG_HEX_OUTPUT_SHORT), "I"));
            this.registryPanel.Children.Add(GetDataPair(this.chip8CoreWindow.EMGetRegisterSP().ToString(Config.TEXT_CONFIG_HEX_OUTPUT_BYTE), "SP"));
            this.registryPanel.Children.Add(GetDataPair(this.chip8CoreWindow.EMGetRegisterDT().ToString(Config.TEXT_CONFIG_HEX_OUTPUT_BYTE), "DT"));
            this.registryPanel.Children.Add(GetDataPair(this.chip8CoreWindow.EMGetRegisterST().ToString(Config.TEXT_CONFIG_HEX_OUTPUT_BYTE), "ST"));
        }

        private void BuildKeyViewPanel()
        {
            this.keyPanel.Children.Clear();

            GetDataPair(this.chip8CoreWindow.EMGetKeypadKeys(), "K").ToList().ForEach(element =>
            {
                this.keyPanel.Children.Add(element);
            });
        }

        private void BuildMemoryPanel()
        {
            this.memoryPanel.Children.Clear();

            Byte[] memory = this.chip8CoreWindow.EMGetMemory();
            for (Int32 i = this.memoryIndex; i < this.memoryIndex + 16; i++)
                this.memoryPanel.Children.Add(GetDataPair(memory[i].ToString(Config.TEXT_CONFIG_HEX_OUTPUT_BYTE), $"{"M"}{i.ToString(Config.TEXT_CONFIG_HEX_OUTPUT_SHORT)}"));
        }

        private StackPanel[] GetDataPair(Boolean[] data, String prefix)
        {
            StackPanel[] rootPanels = new StackPanel[data.Length];

            for (Int32 i = 0; i < data.Length; i++)
                rootPanels[i] = GetDataPair(data[i] ? "1" : "0", $"{prefix}{i.ToString(Config.TEXT_CONFIG_HEX_OUTPUT_NIBBLE)}");
            return rootPanels;
        }

        private StackPanel[] GetDataPair(Byte[] data, String prefix)
        {
            StackPanel[] rootPanels = new StackPanel[data.Length];

            for (Int32 i = 0; i < data.Length; i++)
                rootPanels[i] = GetDataPair(data[i].ToString(Config.TEXT_CONFIG_HEX_OUTPUT_BYTE), $"{prefix}{i.ToString(Config.TEXT_CONFIG_HEX_OUTPUT_NIBBLE)}");
            return rootPanels;
        }

        private StackPanel[] GetDataPair(UInt16[] data, String prefix)
        {
            StackPanel[] rootPanels = new StackPanel[data.Length];

            for (Int32 i = 0; i < data.Length; i++)
                rootPanels[i] = GetDataPair(data[i].ToString(Config.TEXT_CONFIG_HEX_OUTPUT_SHORT), $"{prefix}{i.ToString(Config.TEXT_CONFIG_HEX_OUTPUT_NIBBLE)}");
            return rootPanels;
        }

        private StackPanel GetDataPair(String data, String prefix)
        {
            StackPanel rootPanel = new();
            rootPanel.Orientation = Orientation.Horizontal;

            TextBlock keyBlock = new();
            keyBlock.Text = $"{prefix}:";
            keyBlock.FontWeight = FontWeights.Bold;
            keyBlock.Width = 100;
            keyBlock.Height = 20;
            keyBlock.Margin = new Thickness(Config.VALUE_MANAGER_WINDOW_MARGIN * 2, 0, Config.VALUE_MANAGER_WINDOW_MARGIN * 2, 0);
            keyBlock.Foreground = new SolidColorBrush(this.chip8CoreWindow.EMGetEmulatorSettings().emulatorForegroundColor);
            keyBlock.VerticalAlignment = VerticalAlignment.Center;
            rootPanel.Children.Add(keyBlock);

            TextBlock valueBlock = new();
            valueBlock.Text = data;
            valueBlock.Width = 100;
            valueBlock.Height = 20;
            valueBlock.FontWeight = FontWeights.Bold;
            valueBlock.Foreground = new SolidColorBrush(this.chip8CoreWindow.EMGetEmulatorSettings().emulatorForegroundColor);
            valueBlock.VerticalAlignment = VerticalAlignment.Center;
            rootPanel.Children.Add(valueBlock);

            return rootPanel;
        }

        public void Update()
        {
            BuildRegisterViewPanel();
            BuildStackViewPanel();
            BuildRegistryViewPanel();
            BuildKeyViewPanel();
            BuildMemoryPanel();
        }
    }
}
