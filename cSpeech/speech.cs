using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;

namespace cSpeech
{
    public static class speech
    {
        public static SpeechSynthesizer ss = new SpeechSynthesizer();

        public static void init()
        {
            ss.SetOutputToDefaultAudioDevice();
        }
        public static void Speak(string s)
        {
            ss.Speak(s);
        }

        public static void SpeakAsync(string s)
        {
            ss.SpeakAsync(s);
        }
    }
}
