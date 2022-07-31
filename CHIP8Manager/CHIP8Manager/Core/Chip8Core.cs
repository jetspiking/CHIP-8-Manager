using CHIP8Manager.Core.Emulation;
using CHIP8Manager.Core.Hardware;
using CHIP8Manager.Core.Software;
using CHIP8Manager.Misc;
using CHIP8Manager.Models;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace CHIP8Manager.Core
{
    /// <summary>
    /// Provides an implementation for the CHIP-8 core and emulation extensions.
    /// This includes handling the video, keyboard, speaker and CoreData.
    /// </summary>
    public class Chip8Core : IKeyboardListener, IEmulationOperator
    {
        private CoreData coreData;

        private VirtualDisplay virtualDisplay;
        private VirtualSpeaker virtualSpeaker;

        private PhysicalKeyboardMapper keyboardMapper;

        private Thread displayUpdateThread;
        private Thread executionUpdateThread;

        private Boolean runDisplayThread = true;
        private Boolean runExecutionThread = true;

        private Image videoIn;

        private EmulatorSettings emulatorSettings;

        private Chip8Core(EmulatorSettings emulatorSettings)
        {
            this.emulatorSettings = emulatorSettings;

            this.coreData = new();
            this.coreData.Memory = new Memory(emulatorSettings.chip8MemorySize);
            this.coreData.Registry = new Registry(emulatorSettings.chip8DataRegisters, emulatorSettings.chip8StackDepth);
            this.coreData.Keyboard = new Keyboard(emulatorSettings.chip8KeyAmount);
            this.coreData.Display = new Display(emulatorSettings.chip8PixelWidth, emulatorSettings.chip8PixelHeight);
            this.virtualDisplay = new VirtualDisplay(emulatorSettings.emulatorForegroundColor, emulatorSettings.emulatorBackgroundColor, emulatorSettings.emulatorResolutionScale, emulatorSettings.chip8PixelWidth, emulatorSettings.chip8PixelHeight);
            this.virtualSpeaker = new VirtualSpeaker();

            LoadCharacterSet(emulatorSettings.emulatorCharacterSet);
            LoadReadOnlyMemory(emulatorSettings.emulatorRom);

            StartDisplayThread();
            StartExecutionThread();
        }

        public static Chip8Core Chip8CoreInstance(EmulatorState emulatorState)
        {
            EmulatorSettings emulatorSettings = new();
            if (emulatorState.Name != null)
                emulatorSettings.emulatorWindowTitle = emulatorState.Name;
            if (emulatorState.DelayDuration != null)
                emulatorSettings.emulatorDelayDuration = (UInt16)emulatorState.DelayDuration;
            if (emulatorState.EmulatorKeyMap != null)
                emulatorSettings.emulatorKeyMap = SelectableKeyLayout.GetSelectableKeyLayout((KeyLayouts)emulatorState.EmulatorKeyMap);
            if (emulatorState.ForegroundColor != null)
                emulatorSettings.emulatorForegroundColor = SelectableTheme.GetSelectableTheme((Themes)emulatorState.ForegroundColor);
            if (emulatorState.BackgroundColor != null)
                emulatorSettings.emulatorBackgroundColor = SelectableTheme.GetSelectableTheme((Themes)emulatorState.BackgroundColor);
            if (emulatorState.Rom != null)
                emulatorSettings.emulatorRom = emulatorState.Rom;
            return new Chip8Core(emulatorSettings);
        }

        private void StartDisplayThread()
        {
            this.runDisplayThread = true;
            this.displayUpdateThread = new Thread(() =>
            {
                while (runDisplayThread)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        DisplayUpdate();
                    }));
                    System.Threading.Thread.Sleep(25);
                }
            });
            this.displayUpdateThread.Start();
        }

        private void StopDisplayThread()
        {
            this.runDisplayThread = false;
        }

        private void StartExecutionThread()
        {
            this.coreData.CanWaitForKeyInput = true;
            this.executionUpdateThread = new Thread(() =>
            {
                while (this.runExecutionThread)
                    Step();
            });
            this.runExecutionThread = true;
            this.executionUpdateThread.Start();
        }

        private void StopExecutionThread()
        {
            this.runExecutionThread = false;
            this.coreData.CanWaitForKeyInput = false;
        }

        private void DisplayUpdate()
        {
            this.virtualDisplay.Clear();
            for (Byte x = 0; x < this.emulatorSettings.chip8PixelWidth; x++)
                for (Byte y = 0; y < this.emulatorSettings.chip8PixelHeight; y++)
                    if (this.coreData.Display.IsSet(x, y))
                        this.virtualDisplay.DrawPixel(x, y);
            this.videoIn.Source = this.virtualDisplay.Render();
        }

        private void SoundTimerUpdate()
        {
            if (this.coreData.Registry.ST > 0)
            {
                this.virtualSpeaker.Play(1500, this.emulatorSettings.emulatorDelayDuration * this.coreData.Registry.ST);
                this.coreData.Registry.ST = 0;
            }
        }

        private void DelayTimerUpdate()
        {
            if (this.coreData.Registry.DT > 0)
            {
                System.Threading.Thread.Sleep(this.emulatorSettings.emulatorDelayDuration);
                this.coreData.Registry.DT--;
            }
        }

        public void Step()
        {
            SoundTimerUpdate();
            DelayTimerUpdate();
            this.coreData.OpCodeUpdate();
        }

        private void LoadCharacterSet(Byte[] characterSet)
        {
            Array.Copy(characterSet, 0, this.coreData.Memory.RAM, 0, characterSet.Length);
        }

        private void LoadReadOnlyMemory(Byte[] buffer)
        {
            Buffer.BlockCopy(buffer, 0, this.coreData.Memory.RAM, this.emulatorSettings.chip8ProgramLoadAddress, buffer.Length);
            this.coreData.Registry.PC = this.emulatorSettings.chip8ProgramLoadAddress;
        }

        public void Pressed(Byte key)
        {
            this.coreData.Keyboard.Press(key);
        }

        public void Released(Byte key)
        {
            this.coreData.Keyboard.Release(key);
        }

        public Boolean[] EMGetKeypadKeys()
        {
            return this.coreData.Keyboard.Keys;
        }

        public Byte[] EMGetProgramByteCode()
        {
            return this.emulatorSettings.emulatorRom;
        }

        public Byte[] EMGetRegisterV()
        {
            return this.coreData.Registry.V;
        }

        public UInt16 EMGetRegisterI()
        {
            return this.coreData.Registry.I;
        }

        public UInt16[] EMGetRegisterSS()
        {
            return this.coreData.Registry.SS;
        }

        public Byte EMGetRegisterDT()
        {
            return this.coreData.Registry.DT;
        }

        public Byte EMGetRegisterST()
        {
            return this.coreData.Registry.ST;
        }

        public UInt16 EMGetRegisterPC()
        {
            return this.coreData.Registry.PC;
        }

        public Byte EMGetRegisterSP()
        {
            return this.coreData.Registry.SP;
        }

        public Byte[] EMGetMemory()
        {
            return this.coreData.Memory.RAM;
        }

        public UInt16 EMGetOpcode()
        {
            return this.coreData.Memory.RAM[this.coreData.Registry.PC];
        }

        public EmulatorSettings EMGetEmulatorSettings()
        {
            return this.emulatorSettings;
        }

        public void EMSetDisplayImage(ref Image image)
        {
            this.videoIn = image;
        }

        public void EMStepForward()
        {
            Step();
        }

        public void EMStop()
        {
            StopDisplayThread();
            StopExecutionThread();
        }

        public void EMSetKeyListener(Window window)
        {
            this.keyboardMapper = new PhysicalKeyboardMapper(window, emulatorSettings.emulatorKeyMap, this as IKeyboardListener);
        }
    }
}
