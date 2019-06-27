using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game3
{
    public class SoundParams
    {
        public int WaveType { get; set; }

        public float BaseFreq { get; set; }
        public float BaseFreqLimit { get; set; }
        public float FreqRamp { get; set; }
        public float FreqDRamp { get; set; }
        public float Duty { get; set; }
        public float DutyRamp { get; set; }

        public float VibStrength { get; set; }
        public float VibSpeed { get; set; }
        public float VibDelay { get; set; }

        public float EnvAttack { get; set; }
        public float EnvSustain { get; set; }
        public float EnvDecay { get; set; }
        public float EnvPunch { get; set; }

        public bool FilterOn { get; set; }
        public float LPF_Resonance { get; set; }
        public float LPF_Freq { get; set; }
        public float LPF_Ramp { get; set; }
        public float HPF_Freq { get; set; }
        public float HPF_Ramp { get; set; }

        public float PHA_Offset { get; set; }
        public float PHA_Ramp { get; set; }

        public float RepeatSpeed { get; set; }

        public float ARP_Speed { get; set; }
        public float ARP_Mod { get; set; }

        public SoundParams()
        {
            Reset();
        }

        public void Reset()
        {
            WaveType = 0;

            BaseFreq = 0.3f;
            BaseFreqLimit = 0.0f;
            FreqRamp = 0.0f;
            FreqDRamp = 0.0f;
            Duty = 0.0f;
            DutyRamp = 0.0f;

            VibStrength = 0.0f;
            VibSpeed = 0.0f;
            VibDelay = 0.0f;

            EnvAttack = 0.0f;
            EnvSustain = 0.3f;
            EnvDecay = 0.4f;
            EnvPunch = 0.0f;

            FilterOn = false;
            LPF_Resonance = 0.0f;
            LPF_Freq = 1.0f;
            LPF_Ramp = 0.0f;
            HPF_Freq = 0.0f;
            HPF_Ramp = 0.0f;

            PHA_Offset = 0.0f;
            PHA_Ramp = 0.0f;

            RepeatSpeed = 0.0f;

            ARP_Speed = 0.0f;
            ARP_Mod = 0.0f;
        }

    }
}
