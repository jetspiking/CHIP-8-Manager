using CHIP8Manager.Misc;
using CHIP8Manager.Models;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CHIP8Manager.Windows
{
    /// <summary>
    /// Builds the CHIP-8 window for creating or adjusting a virtual machine.
    /// </summary>
    public class NewVMWindow : Window
    {
        public EmulatorState EmulatorState { get; set; } = new();
        public Boolean IsConfirmed = false;

        public NewVMWindow(EmulatorState? emulatorState = null)
        {
            if (emulatorState != null)
                this.EmulatorState = emulatorState;

            StackPanel settingsPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN)
            };
            this.Content = settingsPanel;

            Label nameLabel = new Label
            {
                Content = Config.TEXT_NEWVM_NAME,
                FontWeight = FontWeights.Bold
            };
            settingsPanel.Children.Add(nameLabel);

            TextBox nameEditBox = new TextBox
            {
                Text = emulatorState == null ? Config.TEXT_NEWVM_DEFAULT_NAME : EmulatorState.Name
            };
            settingsPanel.Children.Add(nameEditBox);

            Label delayLabel = new Label
            {
                Content = Config.TEXT_NEWVM_DELAY_TIMER_DURATION,
                FontWeight = FontWeights.Bold
            };
            settingsPanel.Children.Add(delayLabel);

            TextBox delayEditBox = new TextBox
            {
                Text = emulatorState == null ? Config.TEXT_NEWVM_DEFAULT_DELAY : EmulatorState.DelayDuration.ToString()
            };
            settingsPanel.Children.Add(delayEditBox);

            Label romLabel = new Label
            {
                Content = emulatorState == null ? Config.TEXT_NEWVM_ROM : $"{Config.TEXT_NEWVM_ROM} ({EmulatorState.RomName})",
                FontWeight = FontWeights.Bold
            };
            settingsPanel.Children.Add(romLabel);

            Button browseButton = new Button
            {
                Content = Config.TEXT_NEWVM_BROWSE,
            };
            settingsPanel.Children.Add(browseButton);

            Label keymapLabel = new Label
            {
                Content = Config.TEXT_NEWVM_KEYMAP,
                FontWeight = FontWeights.Bold
            };
            settingsPanel.Children.Add(keymapLabel);

            ComboBox keymapBox = new();
            settingsPanel.Children.Add(keymapBox);

            foreach (KeyLayouts keyLayout in (KeyLayouts[])Enum.GetValues(typeof(KeyLayouts)))
                keymapBox.Items.Add(keyLayout);

            keymapBox.SelectedIndex = 0;

            if (emulatorState != null)
                keymapBox.SelectedItem = EmulatorState.EmulatorKeyMap;

            Label foregroundColorLabel = new Label
            {
                Content = Config.TEXT_NEWVM_COLORFG,
                FontWeight = FontWeights.Bold
            };
            settingsPanel.Children.Add(foregroundColorLabel);

            ComboBox foregroundColorBox = new();
            settingsPanel.Children.Add(foregroundColorBox);

            Label backgroundColorLabel = new Label
            {
                Content = Config.TEXT_NEWVM_COLORBG,
                FontWeight = FontWeights.Bold
            };
            settingsPanel.Children.Add(backgroundColorLabel);

            ComboBox backgroundColorBox = new();
            settingsPanel.Children.Add(backgroundColorBox);

            foreach (Themes theme in (Themes[])Enum.GetValues(typeof(Themes)))
            {
                foregroundColorBox.Items.Add(theme);
                backgroundColorBox.Items.Add(theme);
            }

            foregroundColorBox.SelectedIndex = 0;
            backgroundColorBox.SelectedIndex = 1;

            if (emulatorState != null)
            {
                foregroundColorBox.SelectedItem = EmulatorState.ForegroundColor;
                backgroundColorBox.SelectedItem = EmulatorState.BackgroundColor;
            }

            Label virtualMachineLabel = new Label
            {
                Content = Config.TEXT_NEWVM_VIRTUALMACHINE,
                FontWeight = FontWeights.Bold
            };
            settingsPanel.Children.Add(virtualMachineLabel);

            Button addButton = new Button
            {
                Content = Config.TEXT_NEWVM_ADD,
            };
            settingsPanel.Children.Add(addButton);

            OpenFileDialog openFileDialog = new();
            openFileDialog.ValidateNames = true;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            Boolean? dialogResult;

            browseButton.Click += (sender, e) =>
            {
                dialogResult = openFileDialog.ShowDialog();

                if (dialogResult != null && dialogResult.Value)
                {
                    romLabel.Content = $"{Config.TEXT_NEWVM_ROM} ({openFileDialog.SafeFileName})";
                    this.EmulatorState.RomName = openFileDialog.SafeFileName;
                    if (File.Exists(openFileDialog.FileName))
                        this.EmulatorState.Rom = File.ReadAllBytes(openFileDialog.FileName);
                }
            };

            addButton.Click += (sender, e) =>
            {
                this.EmulatorState.Name = nameEditBox.Text;
                this.EmulatorState.DelayDuration = UInt16.Parse(delayEditBox.Text);
                this.EmulatorState.ForegroundColor = (Themes)foregroundColorBox.SelectedItem;
                this.EmulatorState.BackgroundColor = (Themes)backgroundColorBox.SelectedItem;
                this.EmulatorState.EmulatorKeyMap = (KeyLayouts)keymapBox.SelectedItem;
                this.IsConfirmed = true;
                this.Close();
            };

            this.ResizeMode = ResizeMode.NoResize;
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.Show();
        }
    }
}
