using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Assignment
{
    class GameAudio
    {
        //Audio content.
        private AudioEngine audioEngine;
        private WaveBank waveBank;
        private SoundBank soundBank;
        private Cue background;
        private Cue warn;

        public GameAudio(String audioEngineName, String waveBankName, String soundBankName,String backgroundName, String setVarName, float varValue)
        {

            //Load the audio engine, wave bank and sound bank.
            audioEngine = new AudioEngine(audioEngineName);
            waveBank = new WaveBank(audioEngine,waveBankName);
            soundBank = new SoundBank(audioEngine,soundBankName);

            background = soundBank.GetCue(backgroundName);
            background.SetVariable(setVarName, varValue);
            background.Play();

        }

        public void playSound(String sound)
        {
            soundBank.PlayCue(sound);
        }

        public void backGroundVol(int num)
        {
            float vol;
            if (num <= 0)
            {
                vol = 0;
            }
            else
            {
                vol = 70 + (2.0f * num);
            }
            background.SetVariable("Volume", vol);
        }
    }
}
