using CHIP8Manager.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace CHIP8Manager.Core.Software
{
    /// <summary>
    /// Provides an interface for the core actions.
    /// </summary>
    public interface IEmulationOperator : IKeyboardListener
    {
        Boolean[] EMGetKeypadKeys();
        Byte[] EMGetProgramByteCode();
        Byte[] EMGetRegisterV();
        UInt16 EMGetRegisterI();
        UInt16[] EMGetRegisterSS();
        Byte EMGetRegisterDT();
        Byte EMGetRegisterST();
        UInt16 EMGetRegisterPC();
        Byte EMGetRegisterSP();
        Byte[] EMGetMemory();
        UInt16 EMGetOpcode();

        EmulatorSettings EMGetEmulatorSettings();
        void EMSetDisplayImage(ref Image image);
        void EMSetKeyListener(Window window);
        void EMStepForward();
        void EMStop();
    }
}
