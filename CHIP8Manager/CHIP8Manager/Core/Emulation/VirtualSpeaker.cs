using System;

namespace CHIP8Manager.Core.Emulation
{
    /// <summary>
    /// Provides an implementation for producing a beep sound with a frequency and duration of choice.
    /// </summary>
    public class VirtualSpeaker
    {
        public VirtualSpeaker()
        {
        }

        public void Play(Int32 frequency, Int32 duration)
        {
            Console.Beep(frequency, duration);
        }
    }
}
