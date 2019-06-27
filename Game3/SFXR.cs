using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Game3
{


    public class SFXR
    {
        public const int DefaultWaveBits = 16;
        private int waveBits = DefaultWaveBits;
        public int WaveBits
        {
            get { return waveBits; }
            set { waveBits = ValidateWaveBits(value); }
        }

        private int ValidateWaveBits(int toSet)
        {
            if(toSet == 16 || toSet == 8)
            {
                return toSet;
            }
            return DefaultWaveBits;
        }

        private float masterVolume = 0.5f;
        public float MasterVolume
        {
            get { return masterVolume; }
            set { masterVolume = Bound(0f, 1f, value); }
        }

        private float soundVolume = 0.5f;
        public float SoundVolume
        {
            get { return soundVolume; }
            set { soundVolume = Bound(0f, 1f, value); }
        }


        Random rand = new Random();

        #region Internal State
        bool playing_sample = false;
        int phase;
        double fperiod;
        double fmaxperiod;
        double fslide;
        double fdslide;
        int period;
        float square_duty;
        float square_slide;
        int env_stage;
        int env_time;
        int[] env_length = new int[3];
        float env_vol;
        float fphase;
        float fdphase;
        int iphase;
        float[] phaser_buffer = new float[1024];
        int ipp;
        float[] noise_buffer = new float[32];
        float fltp;
        float fltdp;
        float fltw;
        float fltw_d;
        float fltdmp;
        float fltphp;
        float flthp;
        float flthp_d;
        float vib_phase;
        float vib_speed;
        float vib_amp;
        int rep_time;
        int rep_limit;
        int arp_time;
        int arp_limit;
        double arp_mod;
        #endregion

        int wav_freq = 44100;

        int file_sampleswritten;
        float filesample = 0.0f;
        int fileacc = 0;


        int rnd(int n)
        {
            return rand.Next(n + 1);
        }

        float frnd(float range)
        {
            return (float)rnd(10000) / 10000 * range;

        }

        private float Bound(float lower, float upper, float actual)
        {
            if(actual < lower)
            {
                return lower;
            }
            else if(actual > upper)
            {
                return upper;
            }
            else
            {
                return actual;
            }
        }

        public SFXR() {
            /*
            effect = new DynamicSoundEffectInstance(44100, AudioChannels.Mono);
            effect.BufferNeeded += onNeedBuffer;
            ResetSample(false);
            playing_sample = true;
            byte[] temp = SynthSample(4096 * 2, 0, "yes");
            cache.AddRange(temp);
            effect.SubmitBuffer(temp);
            //playing_sample = true;
            */
        }


        void PlaySample(SoundParams parms)
        {
            ResetSample(parms, false);
            playing_sample = true;
        }

        public byte[] GenerateFullSound(SoundParams parms, int stepLength)
        {
            ResetSample(parms, false);
            MemoryStream ms = new MemoryStream();
            PlaySample(parms);
            while (playing_sample)
            {
                SynthSample(parms, stepLength, ms);
            }
            return ms.ToArray();
        }
        

        public void ResetSample(SoundParams parms, bool restart)
        {
            if (!restart)
                phase = 0;
            fperiod = 100.0f / (parms.BaseFreq * parms.BaseFreq + 0.001f);
            period = (int)fperiod;
            fmaxperiod = 100.0f / (parms.BaseFreqLimit * parms.BaseFreqLimit + 0.001f);
            fslide = 1.0f - Math.Pow(parms.FreqRamp, 3.0) * 0.01;
            fdslide = -Math.Pow(parms.FreqDRamp, 3.0) * 000001;
            square_duty = 0.5f - parms.Duty * 0.5f;
            square_slide = -parms.DutyRamp * 0.00005f;
            if (parms.ARP_Mod >= 0.0f)
                arp_mod = 1.0 - (float)Math.Pow((double)parms.ARP_Mod, 2.0) * 0.9;
            else
                arp_mod = 1.0 + (float)Math.Pow((double)parms.ARP_Mod, 2.0) * 10.0;
            arp_time = 0;
            arp_limit = (int)((float)Math.Pow(1.0f - parms.ARP_Speed, 2.0f) * 20000 + 32);
            if (parms.ARP_Speed == 1.0f)
                arp_limit = 0;
            if (!restart)
            {
                // reset filter
                fltp = 0.0f;
                fltdp = 0.0f;
                fltw = (float)Math.Pow(parms.LPF_Freq, 3.0f) * 0.1f;
                fltw_d = 1.0f + parms.LPF_Ramp * 0.0001f;
                fltdmp = 5.0f / (1.0f + (float)Math.Pow(parms.LPF_Resonance, 2.0f) * 20.0f) * (0.01f + fltw);
                if (fltdmp > 0.8f) fltdmp = 0.8f;
                fltphp = 0.0f;
                flthp = (float)Math.Pow(parms.HPF_Freq, 2.0f) * 0.1f;
                flthp_d = (float)1.0 + parms.HPF_Ramp * 0.0003f;
                // reset vibrato
                vib_phase = 0.0f;
                vib_speed = (float)Math.Pow(parms.VibSpeed, 2.0f) * 0.01f;
                vib_amp = parms.VibStrength * 0.5f;
                // reset envelope
                env_vol = 0.0f;
                env_stage = 0;
                env_time = 0;
                env_length[0] = (int)(parms.EnvAttack * parms.EnvAttack * 100000.0f);
                env_length[1] = (int)(parms.EnvSustain * parms.EnvSustain * 100000.0f);
                env_length[2] = (int)(parms.EnvDecay * parms.EnvDecay * 100000.0f);

                fphase = (float)Math.Pow(parms.PHA_Offset, 2.0f) * 1020.0f;
                if (parms.PHA_Offset < 0.0f) fphase = -fphase;
                fdphase = (float)Math.Pow(parms.PHA_Ramp, 2.0f) * 1.0f;
                if (parms.PHA_Ramp < 0.0f) fdphase = -fdphase;
                iphase = Math.Abs((int)fphase);
                ipp = 0;
                for (int k = 0; k < 1024; k++)
                    phaser_buffer[k] = 0.0f;

                for (int k = 0; k < 32; k++)
                    noise_buffer[k] = frnd(2.0f) - 1.0f;

                rep_time = 0;
                rep_limit = (int)((float)Math.Pow(1.0f - parms.RepeatSpeed, 2.0f) * 20000 + 32);
                if (parms.RepeatSpeed == 0.0f)
                    rep_limit = 0;
            }
        }
        
        void SynthSample(SoundParams parms, int length, MemoryStream msBuffer)
        {
            file_sampleswritten = 0;
            for (int i = 0; i < length - 1; i++)
            {
                if (!playing_sample)
                    break;

                rep_time++;
                if (rep_limit != 0 && rep_time >= rep_limit)
                {
                    rep_time = 0;
                    ResetSample(parms, true);
                }

                // frequency envelopes/arpeggios
                arp_time++;
                if (arp_limit != 0 && arp_time >= arp_limit)
                {
                    arp_limit = 0;
                    fperiod *= arp_mod;
                }
                fslide += fdslide;
                fperiod *= fslide;

                if (fperiod > fmaxperiod)
                {
                    fperiod = fmaxperiod;
                    if (parms.BaseFreqLimit > 0.0f)
                    {
                        playing_sample = false;
                    }
                }
                float rfperiod = (float)fperiod;
                if (vib_amp > 0.0f)
                {
                    vib_phase += vib_speed;
                    rfperiod = (float)fperiod * (1.0f + (float)Math.Sin(vib_phase) * vib_amp);
                }
                period = (int)rfperiod;
                if (period < 8) period = 8;
                square_duty += square_slide;
                if (square_duty < 0.0f) square_duty = 0.0f;
                if (square_duty > 0.5f) square_duty = 0.5f;
                // volume envelope
                env_time++;
                if (env_time > env_length[env_stage])
                {
                    env_time = 0;
                    env_stage++;
                    if (env_stage == 3)
                    {
                        playing_sample = false;
                    }
                }
                if (env_stage == 0)
                    env_vol = (float)env_time / env_length[0];
                if (env_stage == 1)
                    env_vol = 1.0f + (float)Math.Pow(1.0f - (float)env_time / env_length[1], 1.0f) * 2.0f * parms.EnvPunch;
                if (env_stage == 2)
                    env_vol = 1.0f - (float)env_time / env_length[2];

                // phaser step
                fphase += fdphase;
                iphase = Math.Abs((int)fphase);
                if (iphase > 1023) iphase = 1023;

                if (flthp_d != 0.0f)
                {
                    flthp *= flthp_d;
                    if (flthp < 0.00001f) flthp = 0.00001f;
                    if (flthp > 0.1f) flthp = 0.1f;
                }

                float ssample = 0.0f;
                for (int si = 0; si < 8; si++) // 8x supersampling
                {
                    float sample = 0.0f;
                    phase++;
                    if (phase >= period)
                    {
                        //				phase=0;
                        phase %= period;
                        if (parms.WaveType == 3)
                            for (int j = 0; j < 32; j++)
                                noise_buffer[j] = frnd(2.0f) - 1.0f;
                    }
                    // base waveform
                    float fp = (float)phase / period;
                    switch (parms.WaveType)
                    {
                        case 0: // square
                            if (fp < square_duty)
                                sample = 0.5f;
                            else
                                sample = -0.5f;
                            break;
                        case 1: // sawtooth
                            sample = 1.0f - fp * 2;
                            break;
                        case 2: // sine
                            sample = (float)Math.Sin(fp * 2 * Math.PI);
                            break;
                        case 3: // noise
                            sample = noise_buffer[phase * 32 / period];
                            break;
                    }
                    // lp filter
                    float pp = fltp;
                    fltw *= fltw_d;
                    if (fltw < 0.0f) fltw = 0.0f;
                    if (fltw > 0.1f) fltw = 0.1f;
                    if (parms.LPF_Freq != 1.0f)
                    {
                        fltdp += (sample - fltp) * fltw;
                        fltdp -= fltdp * fltdmp;
                    }
                    else
                    {
                        fltp = sample;
                        fltdp = 0.0f;
                    }
                    fltp += fltdp;
                    // hp filter
                    fltphp += fltp - pp;
                    fltphp -= fltphp * flthp;
                    sample = fltphp;
                    // phaser
                    phaser_buffer[ipp & 1023] = sample;
                    sample += phaser_buffer[(ipp - iphase + 1024) & 1023];
                    ipp = (ipp + 1) & 1023;
                    // final accumulation and envelope application
                    ssample += sample * env_vol;
                }
                ssample = ssample / 8 * MasterVolume;

                ssample *= 2.0f * SoundVolume;
                        
                if (msBuffer != null)
                {

                    // quantize depending on format
                    // accumulate/count to accomodate variable sample rate?
                    ssample *= 4.0f; // arbitrary gain to get reasonable output volume...
                    if (ssample > 1.0f) ssample = 1.0f;
                    if (ssample < -1.0f) ssample = -1.0f;
                    filesample += ssample;
                    //Console.WriteLine(ssample);
                    fileacc++;
                    if (wav_freq == 44100 || fileacc == 2)
                    {
                        filesample /= fileacc;
                        fileacc = 0;
                        if (WaveBits == 16)
                        {
                            short isample = (short)(filesample * 32000);
                            byte[] buf = BitConverter.GetBytes(isample);
                            msBuffer.Write(buf, 0, buf.Length);
                        }
                        else
                        {
                            byte isample = (byte)(filesample * 127 + 128);
                            byte[] buf = BitConverter.GetBytes(isample);
                            msBuffer.Write(buf, 0, buf.Length);
                        }
                        filesample = 0.0f;

                    }
                    file_sampleswritten++;

                }

            }
        }
        /*
        
        private void onNeedBuffer(object sender, EventArgs e)
        {
            if (playing_sample == false & cached == false)
            {
                //playing_sample = true;
                //file_sampleswritten = 0;
                //filesample = 0;
                cached = true;
                playoneaftercache = false;
                effect.Stop();
                return;

            }
            else if (cached == true && playoneaftercache)
            {
                effect.SubmitBuffer(cache.ToArray());
                playoneaftercache = false;
                return;
            }
            else if (playoneaftercache == false) {
                effect.Stop();
            }
            else
            {
                byte[] temp = null;//SynthSample(4096*2, 0, "yes");
                cache.AddRange(temp);
                effect.SubmitBuffer(temp);

            }
        }
        */
    }
}
