using CHIP8Manager.Windows;
using System.Threading;

namespace CHIP8Manager.Models
{
    public class ManagedVirtualMachine
    {
        public EmulatorState EmulatorState { get; set; }
        public CoreVMWindow? CoreVMWindow { get; set; }

        public ManagedVirtualMachine(EmulatorState emulatorState)
        {
            this.EmulatorState = emulatorState;
        }
    }
}
