using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game3
{
    public class SFXGenerator
    {
        private Random rand;

        public SFXGenerator()
        {
            rand = new Random();
        }

        public SFXGenerator(int seed)
        {
            rand = new Random(seed);
        }

        

        private int rnd(int n)
        {
            return rand.Next(n + 1);
        }

        private float frnd(float range)
        {
            return (float)rnd(10000) / 10000 * range;

        }

        public SoundParams RandomCoin()
        {
            SoundParams retVal = new SoundParams();
            
            retVal.BaseFreq = 0.4f + frnd(0.5f);
            retVal.EnvAttack = 0.0f;
            retVal.EnvSustain = frnd(0.1f);
            retVal.EnvDecay = 0.1f + frnd(0.4f);
            retVal.EnvPunch = 0.3f + frnd(0.3f);
            if (rnd(1) > 0)
            {
                retVal.ARP_Speed = 0.5f + frnd(0.2f);
                retVal.ARP_Mod = 0.2f + frnd(0.4f);
            }

            return retVal;
        }

        public SoundParams Laser()
        {
            SoundParams retVal = new SoundParams();

            retVal.WaveType = rnd(2);
            if (retVal.WaveType == 2 && rnd(1) > 0)
                retVal.WaveType = rnd(1);
            retVal.BaseFreq = 0.5f + frnd(0.5f);
            retVal.BaseFreqLimit = retVal.BaseFreq - 0.2f - frnd(0.6f);
            if (retVal.BaseFreqLimit < 0.2f) retVal.BaseFreqLimit = 0.2f;
            retVal.FreqRamp = -0.15f - frnd(0.2f);
            if (rnd(2) == 0)
            {
                retVal.BaseFreq = 0.3f + frnd(0.6f);
                retVal.BaseFreqLimit = frnd(0.1f);
                retVal.FreqRamp = -0.35f - frnd(0.3f);
            }
            if (rnd(1) > 0)
            {
                retVal.Duty = frnd(0.5f);
                retVal.DutyRamp = frnd(0.2f);
            }
            else
            {
                retVal.Duty = 0.4f + frnd(0.5f);
                retVal.DutyRamp = -frnd(0.7f);
            }
            retVal.EnvAttack = 0.0f;
            retVal.EnvSustain = 0.1f + frnd(0.2f);
            retVal.EnvDecay = frnd(0.4f);
            if (rnd(1) > 0)
                retVal.EnvPunch = frnd(0.3f);
            if (rnd(2) == 0)
            {
                retVal.PHA_Offset = frnd(0.2f);
                retVal.PHA_Ramp = -frnd(0.2f);
            }
            if (rnd(1) > 0)
                retVal.HPF_Freq = frnd(0.3f);

            return retVal;
        }

        public SoundParams Explosion()
        {
            SoundParams retVal = new SoundParams();

            retVal.WaveType = 3;
            if (rnd(1) > 0)
            {
                retVal.BaseFreq = 0.1f + frnd(0.4f);
                retVal.FreqRamp = -0.1f + frnd(0.4f);
            }
            else
            {
                retVal.BaseFreq = 0.2f + frnd(0.7f);
                retVal.FreqRamp = -0.2f - frnd(0.2f);
            }
            retVal.BaseFreq *= retVal.BaseFreq;
            if (rnd(4) == 0)
                retVal.FreqRamp = 0.0f;
            if (rnd(2) == 0)
                retVal.RepeatSpeed = 0.3f + frnd(0.5f);
            retVal.EnvAttack = 0.0f;
            retVal.EnvSustain = 0.1f + frnd(0.3f);
            retVal.EnvDecay = frnd(0.5f);
            if (rnd(1) == 0)
            {
                retVal.PHA_Offset = -0.3f + frnd(0.9f);
                retVal.PHA_Ramp = -frnd(0.3f);
            }
            retVal.EnvPunch = 0.2f + frnd(0.6f);
            if (rnd(1) > 0)
            {
                retVal.VibStrength = frnd(0.7f);
                retVal.VibSpeed = frnd(0.6f);
            }
            if (rnd(2) == 0)
            {
                retVal.ARP_Speed = 0.6f + frnd(0.3f);
                retVal.ARP_Mod = 0.8f - frnd(1.6f);
            }

            
            return retVal;
        }

        public SoundParams Powerup()
        {
            SoundParams retVal = new SoundParams();

            if (rnd(1) > 0)
                retVal.WaveType = 1;
            else
                retVal.Duty = frnd(0.6f);
            if (rnd(1) > 0)
            {
                retVal.BaseFreq = 0.2f + frnd(0.3f);
                retVal.FreqRamp = 0.1f + frnd(0.4f);
                retVal.RepeatSpeed = 0.4f + frnd(0.4f);
            }
            else
            {
                retVal.BaseFreq = 0.2f + frnd(0.3f);
                retVal.FreqRamp = 0.05f + frnd(0.2f);
                if (rnd(1) > 0)
                {
                    retVal.VibStrength = frnd(0.7f);
                    retVal.VibSpeed = frnd(0.6f);
                }
            }
            retVal.EnvAttack = 0.0f;
            retVal.EnvSustain = frnd(0.4f);
            retVal.EnvDecay = 0.1f + frnd(0.4f);

            return retVal;
        }

        public SoundParams HitHurt()
        {
            SoundParams retVal = new SoundParams();
            retVal.WaveType = rnd(2);
            if (retVal.WaveType == 2)
                retVal.WaveType = 3;
            if (retVal.WaveType == 0)
                retVal.Duty = frnd(0.6f);
            retVal.BaseFreq = 0.2f + frnd(0.6f);
            retVal.FreqRamp = -0.3f - frnd(0.4f);
            retVal.EnvAttack = 0.0f;
            retVal.EnvSustain = frnd(0.1f);
            retVal.EnvDecay = 0.1f + frnd(0.2f);
            if (rnd(1) > 0)
                retVal.HPF_Freq = frnd(0.3f);

            return retVal;
        }

        public SoundParams Jump()
        {
            SoundParams retVal = new SoundParams();
            retVal.WaveType = 0;
            retVal.Duty = frnd(0.6f);
            retVal.BaseFreq = 0.3f + frnd(0.3f);
            retVal.FreqRamp = 0.1f + frnd(0.2f);
            retVal.EnvAttack = 0.0f;
            retVal.EnvSustain = 0.1f + frnd(0.3f);
            retVal.EnvDecay = 0.1f + frnd(0.2f);
            if (rnd(1) > 0)
                retVal.HPF_Freq = frnd(0.3f);
            if (rnd(1) > 0)
                retVal.LPF_Freq = 1.0f - frnd(0.6f);

            return retVal;
        }

        public SoundParams BlipSelect()
        {
            SoundParams retVal = new SoundParams();
            retVal.WaveType = rnd(1);
            if (retVal.WaveType == 0)
                retVal.Duty = frnd(0.6f);
            retVal.BaseFreq = 0.2f + frnd(0.4f);
            retVal.EnvAttack = 0.0f;
            retVal.EnvSustain = 0.1f + frnd(0.1f);
            retVal.EnvDecay = frnd(0.2f);
            retVal.HPF_Freq = 0.1f;

            return retVal;
        }
    }
}
