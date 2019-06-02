using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Game3
{


    class SFXR
    {
        DynamicSoundEffectInstance effect;
        Random rand = new Random();

        //DynamicSoundEffectInstance effectinstance;
        string path1 = "./Content/test.wav";
        byte[] test;

        //start vars here
        int wave_type;

        float p_base_freq;
        float p_freq_limit;
        float p_freq_ramp;
        float p_freq_dramp;
        float p_duty;
        float p_duty_ramp;

        float p_vib_strength;
        float p_vib_speed;
        float p_vib_delay;

        float p_env_attack;
        float p_env_sustain;
        float p_env_decay;
        float p_env_punch;

        bool filter_on;
        float p_lpf_resonance;
        float p_lpf_freq;
        float p_lpf_ramp;
        float p_hpf_freq;
        float p_hpf_ramp;

        float p_pha_offset;
        float p_pha_ramp;

        float p_repeat_speed;

        float p_arp_speed;
        float p_arp_mod;

        float master_vol = 0.5f;

        float sound_vol = 0.5f;


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

        //float* vselected = NULL;
        int vcurbutton = -1;

        int wav_bits = 16;
        int wav_freq = 44100;

        int file_sampleswritten;
        float filesample = 0.0f;
        int fileacc = 0;
        bool playoneaftercache = true;


        List<byte> cache = new List<byte>();
        bool cached = false;


        int rnd(int n)
        {
            return rand.Next(n + 1);
        }

        float frnd(float range)
        {
            return (float)rnd(10000) / 10000 * range;

        }

        public SFXR() {
            effect = new DynamicSoundEffectInstance(44100, AudioChannels.Mono);
            effect.BufferNeeded += onNeedBuffer;
            ResetSample(false);
            playing_sample = true;
            byte[] temp = SynthSample(4096 * 2, 0, "yes");
            cache.AddRange(temp);
            effect.SubmitBuffer(temp);
            //playing_sample = true;
        }

        public SoundState State() {
            return effect.State;
        }

        void PlaySample()
        {
            ResetSample(false);
            playing_sample = true;
            playoneaftercache = true;
        }


        public void Play() {
            PlaySample();
            effect.Play();
        }



        void ResetParams()
        {
            wave_type = 0;

            p_base_freq = 0.3f;
            p_freq_limit = 0.0f;
            p_freq_ramp = 0.0f;
            p_freq_dramp = 0.0f;
            p_duty = 0.0f;
            p_duty_ramp = 0.0f;

            p_vib_strength = 0.0f;
            p_vib_speed = 0.0f;
            p_vib_delay = 0.0f;

            p_env_attack = 0.0f;
            p_env_sustain = 0.3f;
            p_env_decay = 0.4f;
            p_env_punch = 0.0f;

            filter_on = false;
            p_lpf_resonance = 0.0f;
            p_lpf_freq = 1.0f;
            p_lpf_ramp = 0.0f;
            p_hpf_freq = 0.0f;
            p_hpf_ramp = 0.0f;

            p_pha_offset = 0.0f;
            p_pha_ramp = 0.0f;

            p_repeat_speed = 0.0f;

            p_arp_speed = 0.0f;
            p_arp_mod = 0.0f;
        }

        public void coin()
        {
            cache = new List<byte>();
            cached = false;
            ResetParams();
            p_base_freq = 0.4f + frnd(0.5f);
            p_env_attack = 0.0f;
            p_env_sustain = frnd(0.1f);
            p_env_decay = 0.1f + frnd(0.4f);
            p_env_punch = 0.3f + frnd(0.3f);
            if (rnd(1) > 0)
            {
                p_arp_speed = 0.5f + frnd(0.2f);
                p_arp_mod = 0.2f + frnd(0.4f);
            }
            //ResetSample(false);
            //playing_sample = true;
            //byte[] temp = SynthSample(4096 * 2, 0, "yes");
            //cache.AddRange(temp);
            //effect.SubmitBuffer(temp);

        }

        public void laser() {
            ResetParams();
            wave_type = rnd(2);
            if (wave_type == 2 && rnd(1) > 0)
                wave_type = rnd(1);
            p_base_freq = 0.5f + frnd(0.5f);
            p_freq_limit = p_base_freq - 0.2f - frnd(0.6f);
            if (p_freq_limit < 0.2f) p_freq_limit = 0.2f;
            p_freq_ramp = -0.15f - frnd(0.2f);
            if (rnd(2) == 0)
            {
                p_base_freq = 0.3f + frnd(0.6f);
                p_freq_limit = frnd(0.1f);
                p_freq_ramp = -0.35f - frnd(0.3f);
            }
            if (rnd(1)>0)
            {
                p_duty = frnd(0.5f);
                p_duty_ramp = frnd(0.2f);
            }
            else
            {
                p_duty = 0.4f + frnd(0.5f);
                p_duty_ramp = -frnd(0.7f);
            }
            p_env_attack = 0.0f;
            p_env_sustain = 0.1f + frnd(0.2f);
            p_env_decay = frnd(0.4f);
            if (rnd(1)>0)
                p_env_punch = frnd(0.3f);
            if (rnd(2) == 0)
            {
                p_pha_offset = frnd(0.2f);
                p_pha_ramp = -frnd(0.2f);
            }
            if (rnd(1) >0)
                p_hpf_freq = frnd(0.3f);
        }

        public void ResetSample(bool restart)
        {
            if (!restart)
                phase = 0;
            fperiod = 100.0f / (p_base_freq * p_base_freq + 0.001f);
            period = (int)fperiod;
            fmaxperiod = 100.0f / (p_freq_limit * p_freq_limit + 0.001f);
            fslide = 1.0f - Math.Pow(p_freq_ramp, 3.0) * 0.01;
            fdslide = -Math.Pow(p_freq_dramp, 3.0) * 000001;
            square_duty = 0.5f - p_duty * 0.5f;
            square_slide = -p_duty_ramp * 0.00005f;
            if (p_arp_mod >= 0.0f)
                arp_mod = 1.0 - (float)Math.Pow((double)p_arp_mod, 2.0) * 0.9;
            else
                arp_mod = 1.0 + (float)Math.Pow((double)p_arp_mod, 2.0) * 10.0;
            arp_time = 0;
            arp_limit = (int)((float)Math.Pow(1.0f - p_arp_speed, 2.0f) * 20000 + 32);
            if (p_arp_speed == 1.0f)
                arp_limit = 0;
            if (!restart)
            {
                // reset filter
                fltp = 0.0f;
                fltdp = 0.0f;
                fltw = (float)Math.Pow(p_lpf_freq, 3.0f) * 0.1f;
                fltw_d = 1.0f + p_lpf_ramp * 0.0001f;
                fltdmp = 5.0f / (1.0f + (float)Math.Pow(p_lpf_resonance, 2.0f) * 20.0f) * (0.01f + fltw);
                if (fltdmp > 0.8f) fltdmp = 0.8f;
                fltphp = 0.0f;
                flthp = (float)Math.Pow(p_hpf_freq, 2.0f) * 0.1f;
                flthp_d = (float)1.0 + p_hpf_ramp * 0.0003f;
                // reset vibrato
                vib_phase = 0.0f;
                vib_speed = (float)Math.Pow(p_vib_speed, 2.0f) * 0.01f;
                vib_amp = p_vib_strength * 0.5f;
                // reset envelope
                env_vol = 0.0f;
                env_stage = 0;
                env_time = 0;
                env_length[0] = (int)(p_env_attack * p_env_attack * 100000.0f);
                env_length[1] = (int)(p_env_sustain * p_env_sustain * 100000.0f);
                env_length[2] = (int)(p_env_decay * p_env_decay * 100000.0f);

                fphase = (float)Math.Pow(p_pha_offset, 2.0f) * 1020.0f;
                if (p_pha_offset < 0.0f) fphase = -fphase;
                fdphase = (float)Math.Pow(p_pha_ramp, 2.0f) * 1.0f;
                if (p_pha_ramp < 0.0f) fdphase = -fdphase;
                iphase = Math.Abs((int)fphase);
                ipp = 0;
                for (int k = 0; k < 1024; k++)
                    phaser_buffer[k] = 0.0f;

                for (int k = 0; k < 32; k++)
                    noise_buffer[k] = frnd(2.0f) - 1.0f;

                rep_time = 0;
                rep_limit = (int)((float)Math.Pow(1.0f - p_repeat_speed, 2.0f) * 20000 + 32);
                if (p_repeat_speed == 0.0f)
                    rep_limit = 0;
            }
        }


        byte[] SynthSample(int length, float buffer, string file)
        {
            byte[] tempbuffer = new byte[length];
            List<byte> tempbuffer16 = new List<byte>();
            file_sampleswritten = 0;
            {

                {
                    for (int i = 0; i < length - 1; i++)
                    {
                        if (!playing_sample)
                            break;

                        rep_time++;
                        if (rep_limit != 0 && rep_time >= rep_limit)
                        {
                            rep_time = 0;
                            ResetSample(true);
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
                        //Console.WriteLine("start");
                        //Console.WriteLine(fdslide);
                        //Console.WriteLine(fperiod);
                        //Console.WriteLine(fmaxperiod);
                        if (fperiod > fmaxperiod)
                        {
                            fperiod = fmaxperiod;
                            if (p_freq_limit > 0.0f)
                            {

                                //file_sampleswritten = 0;
                                //filesample = 0;
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

                                //file_sampleswritten = 0;
                                //filesample = 0;
                                playing_sample = false;
                                //env_stage = 0;
                            }
                        }
                        if (env_stage == 0)
                            env_vol = (float)env_time / env_length[0];
                        if (env_stage == 1)
                            env_vol = 1.0f + (float)Math.Pow(1.0f - (float)env_time / env_length[1], 1.0f) * 2.0f * p_env_punch;
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
                                if (wave_type == 3)
                                    for (int j = 0; j < 32; j++)
                                        noise_buffer[j] = frnd(2.0f) - 1.0f;
                            }
                            // base waveform
                            float fp = (float)phase / period;
                            switch (wave_type)
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
                            if (p_lpf_freq != 1.0f)
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
                        ssample = ssample / 8 * master_vol;

                        ssample *= 2.0f * sound_vol;

                        if (buffer != 0)
                        {
                            if (ssample > 1.0f) ssample = 1.0f;
                            if (ssample < -1.0f) ssample = -1.0f;
                            //not sure what this was about
                            buffer++;
                            buffer = ssample;
                        }
                        //byte returnbyte = 0;
                        if (file != null)
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
                                if (wav_bits == 16)
                                {
                                    short isample = (short)(filesample * 32000);
                                    byte[] buf = BitConverter.GetBytes(isample);
                                    foreach (byte k in buf) {
                                        tempbuffer16.Add(k);
                                    }

                                    //fwrite(&isample, 1, 2, file);
                                    //return returnbyte;
                                }
                                else
                                {
                                    byte isample = (byte)(filesample * 127 + 128);
                                    tempbuffer[i] = isample;
                                    //test[file_sampleswritten] = isample;
                                    //string hex = BitConverter.ToString(test);
                                    //Console.WriteLine(isample);
                                    //fwrite(&isample, 1, 1, file);
                                }
                                filesample = 0.0f;

                            }
                            file_sampleswritten++;

                        }

                    }
                }
            }
            if (wav_bits == 16)
            {
                return tempbuffer16.ToArray();
            }
            else {
                return tempbuffer;
            }
            
        }
        
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
                byte[] temp = SynthSample(4096*2, 0, "yes");
                cache.AddRange(temp);
                effect.SubmitBuffer(temp);

            }
        }
    }
}
