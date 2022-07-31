using CHIP8Manager.Core;
using CHIP8Manager.Misc;
using CHIP8Manager.Models;
using CHIP8Manager.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CHIP8Manager.Windows
{
    /// <summary>
    /// Builds the CHIP-8 manager window, which contains actions for managing CHIP-8 virtual machines.
    /// </summary>
    public class ManagerWindow : Window
    {
        private StackPanel creatorPanel;
        private StackPanel actionPanel;
        private StackPanel emulatorsPanel;
        private StackPanel detailsPanel;

        private List<ManagedVirtualMachine> machines = new();
        private Int32 selectedMachineIndex = -1;

        private Border? SelectedBorder = null;

        public ManagerWindow()
        {
            this.Title = Config.TEXT_MANAGER_WINDOW_TITLE;
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.ResizeMode = ResizeMode.NoResize;
            this.Show();

            BuildVirtualMachinesOverviewPanel();
        }

        private void BuildVirtualMachinesOverviewPanel()
        {
            DockPanel dockPanel = new();
            dockPanel.Margin = new Thickness(Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN);
            this.Content = dockPanel;

            this.creatorPanel = new();
            this.creatorPanel.Orientation = Orientation.Vertical;
            this.creatorPanel.Margin = new Thickness(Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN);
            dockPanel.Children.Add(this.creatorPanel);
            DockPanel.SetDock(this.creatorPanel, Dock.Left);

            StackPanel separatorPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            dockPanel.Children.Add(separatorPanel);
            DockPanel.SetDock(separatorPanel, Dock.Right);

            this.detailsPanel = new();
            this.detailsPanel.Orientation = Orientation.Vertical;
            this.detailsPanel.Margin = new Thickness(Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN);
            separatorPanel.Children.Add(this.detailsPanel);

            this.actionPanel = new();
            this.actionPanel.Orientation = Orientation.Horizontal;
            AddImageAndDescriptionToActionPanel(Images.PlusGreen, Config.TEXT_MANAGER_ACTION_VM_NEW);
            AddImageAndDescriptionToActionPanel(Images.MinusBlue, Config.TEXT_MANAGER_ACTION_VM_DELETE, true);
            AddImageAndDescriptionToActionPanel(Images.BlankCd, Config.TEXT_MANAGER_ACTION_VM_IMPORT);
            AddImageAndDescriptionToActionPanel(Images.Move, Config.TEXT_MANAGER_ACTION_VM_EXPORT, true);
            AddImageAndDescriptionToActionPanel(Images.AutoPlay, Config.TEXT_MANAGER_ACTION_VM_BOOT, true);
            AddImageAndDescriptionToActionPanel(Images.Error, Config.TEXT_MANAGER_ACTION_STOP, true);
            AddImageAndDescriptionToActionPanel(Images.StopWatch, Config.TEXT_MANAGER_ACTION_VM_SETTINGS, true);

            this.creatorPanel.Children.Add(actionPanel);
            Separator separator = new Separator();
            this.creatorPanel.Children.Add(separator);

            this.emulatorsPanel = new StackPanel
            {
                Orientation = Orientation.Vertical
            };
            this.creatorPanel.Children.Add(this.emulatorsPanel);
        }

        private void AddImageAndDescriptionToActionPanel(Images image, String tag, Boolean isSelectionRequired = false)
        {
            StackPanel imageAndDescriptionPanel = new();
            imageAndDescriptionPanel.Orientation = Orientation.Vertical;
            imageAndDescriptionPanel.Margin = new Thickness(Config.VALUE_MANAGER_WINDOW_MARGIN, 0, Config.VALUE_MANAGER_WINDOW_MARGIN, 0);
            actionPanel.Children.Add(imageAndDescriptionPanel);

            Image addChipImage = new();
            addChipImage.Source = Utilities.ImageUtilities.GetBitmapImage(image, Extensions.Png);
            addChipImage.Width = 45;
            addChipImage.Height = 45;
            addChipImage.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            addChipImage.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            imageAndDescriptionPanel.Children.Add(addChipImage);

            TextBlock descriptionTextBlock = new();
            descriptionTextBlock.Text = tag;
            descriptionTextBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            descriptionTextBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            imageAndDescriptionPanel.Children.Add(descriptionTextBlock);

            imageAndDescriptionPanel.MouseEnter += (sender, e) =>
            {
                imageAndDescriptionPanel.Background = new SolidColorBrush(Colors.LightBlue);

                if (isSelectionRequired && this.selectedMachineIndex == -1)
                    imageAndDescriptionPanel.Background = new SolidColorBrush(Colors.Gray);
            };

            imageAndDescriptionPanel.MouseLeave += (sender, e) =>
            {
                imageAndDescriptionPanel.Background = new SolidColorBrush(Colors.Transparent);
            };

            imageAndDescriptionPanel.MouseDown += (sender, e) =>
            {
                this.detailsPanel.Children.Clear();

                switch (tag)
                {
                    case Config.TEXT_MANAGER_ACTION_VM_NEW:
                        {
                            NewVMWindow newVMWindow = new NewVMWindow();
                            newVMWindow.Closed += (sender, e) =>
                            {
                                if (newVMWindow.IsConfirmed)
                                {
                                    this.machines.Add(new ManagedVirtualMachine(newVMWindow.EmulatorState));
                                    BuildVirtualMachineEntries();
                                }
                            };
                        }
                        break;
                    case Config.TEXT_MANAGER_ACTION_VM_DELETE:
                        {
                            if (this.SelectedBorder == null || this.selectedMachineIndex == -1)
                                break;
                            ManagedVirtualMachine selectedMachine = (ManagedVirtualMachine)this.machines[this.selectedMachineIndex];
                            this.SelectedBorder.Background = new SolidColorBrush(Colors.Transparent);
                            if (selectedMachine.CoreVMWindow != null)
                                selectedMachine.CoreVMWindow.Close();
                            this.machines.Remove(selectedMachine);
                            this.selectedMachineIndex = -1;
                        }
                        break;
                    case Config.TEXT_MANAGER_ACTION_VM_IMPORT:
                        {
                            OpenFileDialog openFileDialog = new();
                            openFileDialog.ValidateNames = true;
                            openFileDialog.CheckFileExists = true;
                            openFileDialog.CheckPathExists = true;
                            Boolean? dialogResult = openFileDialog.ShowDialog();
                            if (File.Exists(openFileDialog.FileName))
                            {
                                EmulatorState? emulatorState = JSONManager.DeserializeFromFile<EmulatorState>(openFileDialog.FileName);
                                if (emulatorState != null)
                                {
                                    this.machines.Add(new ManagedVirtualMachine(emulatorState));
                                }
                            }
                        }
                        break;
                    case Config.TEXT_MANAGER_ACTION_VM_EXPORT:
                        {
                            if (this.SelectedBorder == null || this.selectedMachineIndex == -1)
                                break;
                            ManagedVirtualMachine selectedMachine = (ManagedVirtualMachine)this.machines[this.selectedMachineIndex];
                            this.SelectedBorder.Background = new SolidColorBrush(Colors.Transparent);
                            SaveFileDialog saveFileDialog = new();
                            saveFileDialog.CheckPathExists = true;
                            Boolean? dialogResult = saveFileDialog.ShowDialog();
                            if (dialogResult != null && (Boolean)dialogResult)
                                JSONManager.SerializeToFile<EmulatorState>(selectedMachine.EmulatorState, saveFileDialog.FileName);
                        }
                        break;
                    case Config.TEXT_MANAGER_ACTION_VM_BOOT:
                        {
                            if (this.SelectedBorder == null || this.selectedMachineIndex == -1)
                                break;
                            ManagedVirtualMachine selectedMachine = (ManagedVirtualMachine)this.machines[this.selectedMachineIndex];
                            if (selectedMachine.CoreVMWindow != null)
                                selectedMachine.CoreVMWindow.Close();
                            selectedMachine.CoreVMWindow = LaunchVirtualMachine(selectedMachine.EmulatorState);
                        }
                        break;
                    case Config.TEXT_MANAGER_ACTION_VM_STOP:
                        {
                            if (this.SelectedBorder == null || this.selectedMachineIndex == -1)
                                break;
                            ManagedVirtualMachine selectedMachine = (ManagedVirtualMachine)this.machines[this.selectedMachineIndex];
                            if (selectedMachine.CoreVMWindow != null)
                                selectedMachine.CoreVMWindow.Close();
                        }
                        break;
                    case Config.TEXT_MANAGER_ACTION_VM_SETTINGS:
                        {
                            if (this.SelectedBorder == null || this.selectedMachineIndex == -1)
                                break;
                            ManagedVirtualMachine selectedMachine = (ManagedVirtualMachine)this.machines[this.selectedMachineIndex];
                            NewVMWindow newVMWindow = new NewVMWindow(selectedMachine.EmulatorState);
                            newVMWindow.Closed += (sender, e) =>
                            {
                                if (newVMWindow.IsConfirmed)
                                {
                                    if (selectedMachine.CoreVMWindow != null)
                                        selectedMachine.CoreVMWindow.Close();
                                }
                                BuildVirtualMachineEntries();
                            };
                        }
                        break;
                }
                this.selectedMachineIndex = -1;
                BuildVirtualMachineEntries();
            };
        }

        private void BuildVirtualMachineEntries()
        {
            this.emulatorsPanel.Children.Clear();
            for (Int32 i = 0; i < this.machines.Count; i++)
                AddEmulatorManageWindow(i);
        }

        private void DisplaySelectedEmulatorState(EmulatorState emulatorState)
        {
            this.detailsPanel.Children.Clear();

            GetEditableDetail(Config.TEXT_NEWVM_NAME, emulatorState.Name);
            GetEditableDetail(Config.TEXT_NEWVM_DELAY_TIMER_DURATION, emulatorState.DelayDuration.ToString());
            GetEditableDetail(Config.TEXT_NEWVM_ROM, emulatorState.RomName.ToString());
            GetEditableDetail(Config.TEXT_NEWVM_COLORFG, emulatorState.ForegroundColor.ToString());
            GetEditableDetail(Config.TEXT_NEWVM_COLORBG, emulatorState.BackgroundColor.ToString());
        }

        private void GetEditableDetail(String elementName, String currentValue)
        {
            StackPanel elementValuePanel = new StackPanel
            {
                Orientation = Orientation.Vertical
            };

            TextBlock propertyBlock = new TextBlock
            {
                Text = elementName,
                FontWeight = FontWeights.Bold
            };
            elementValuePanel.Children.Add(propertyBlock);

            TextBlock valueBox = new TextBlock
            {
                Text = currentValue
            };
            elementValuePanel.Children.Add(valueBox);
            this.detailsPanel.Children.Add(elementValuePanel);
        }

        private void AddEmulatorManageWindow(Int32 index)
        {
            StackPanel horizontalPanel = new();
            horizontalPanel.Orientation = Orientation.Horizontal;

            Border border = new();
            border.Child = horizontalPanel;
            border.BorderBrush = new SolidColorBrush(Colors.Black);
            border.BorderThickness = new Thickness(Config.VALUE_MANAGER_BORDER_THICKNESS, this.emulatorsPanel.Children.Count == 0 ? Config.VALUE_MANAGER_BORDER_THICKNESS : 0, Config.VALUE_MANAGER_BORDER_THICKNESS, Config.VALUE_MANAGER_BORDER_THICKNESS);
            this.emulatorsPanel.Children.Add(border);

            Image emulatorImage = new();
            emulatorImage.Source = Utilities.ImageUtilities.GetBitmapImage(Images.Chip8, Extensions.Png);
            emulatorImage.Width = 45;
            emulatorImage.Height = 45;
            horizontalPanel.Children.Add(emulatorImage);

            TextBlock emulatorName = new();
            emulatorName.Text = this.machines[index].EmulatorState.Name;
            emulatorName.FontWeight = FontWeights.Bold;
            emulatorName.Margin = new Thickness(Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN, Config.VALUE_MANAGER_WINDOW_MARGIN);
            emulatorName.VerticalAlignment = VerticalAlignment.Center;
            horizontalPanel.Children.Add(emulatorName);

            border.MouseDown += (sender, e) =>
            {
                if (this.SelectedBorder != null)
                    this.SelectedBorder.Background = new SolidColorBrush(Colors.Transparent);
                border.Background = new SolidColorBrush(Colors.LightBlue);
                this.selectedMachineIndex = index;
                this.SelectedBorder = border;
                DisplaySelectedEmulatorState(this.machines[index].EmulatorState);
            };
        }

        private CoreVMWindow LaunchVirtualMachine(EmulatorState emulatorState)
        {
            Chip8Core coreInstance = Chip8Core.Chip8CoreInstance(emulatorState);
            CoreVMWindow coreVMWindow = new CoreVMWindow(coreInstance, coreInstance.EMGetEmulatorSettings());
            return coreVMWindow;
        }
    }
}
