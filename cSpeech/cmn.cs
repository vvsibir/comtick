using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Recognition;
using System.Text;

namespace cSpeech
{
    public static class cmn
    {
        public static Dictionary<string, Action> actions = new Dictionary<string, Action>();

        public static void init()
        {
            actions.Add("speech on", speechOn);
            actions.Add("speech off", speechOff);

            actions.Add("hello volume up", soundManager.VolumeUp);
            actions.Add("hello volume down", soundManager.VolumeDown);
            actions.Add("hello volume mute", soundManager.VolumeMute);

            speech.init();
            initSpeech();
        }
        static SpeechRecognitionEngine sre;

        public static void initSpeech()
        {
            CultureInfo ci = new CultureInfo("en-us");
            sre = new SpeechRecognitionEngine(ci);
            sre.SetInputToDefaultAudioDevice();
            sre.SpeechRecognized += Sre_SpeechRecognized;

            Choices ch_Static = new Choices();
            foreach (string k in actions.Keys) ch_Static.Add(k);
            GrammarBuilder gb_Static = new GrammarBuilder();
            gb_Static.Append(ch_Static);
            Grammar g_StartStop = new Grammar(gb_Static);

            sre.LoadGrammarAsync(g_StartStop);
            grammars.ForEach(g => sre.LoadGrammarAsync(g));

            sre.RecognizeAsync(RecognizeMode.Multiple); // multiple grammars
        }

        static List<Grammar> grammars = new List<Grammar>();
        public static void AddGrammar(Grammar gr)
        {
            grammars.Add(gr);
        }

        static bool enabled = true;
        private static void Sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string txt = e.Result.Text;
            float confidence = e.Result.Confidence; // consider implicit cast to double
            log.Write("\nRecognized: " + txt);

            if (confidence < 0.60) return;

            if (txt.Contains("speech on"))
            {
                speechOn();
                return;
            }
            if (txt.Contains("speech off"))
            {
                speechOff();
                return;
            }

            if (!enabled) return;
            if (actions.ContainsKey(txt))
            {
                actions[txt].Invoke();
                return;
            }

            SpeechRecognizedCustom?.Invoke(sender, e);
        }

        public static event EventHandler<SpeechRecognizedEventArgs> SpeechRecognizedCustom;

        private static void speechOff()
        {
            enabled = false;
            log.Write("Speech is now OFF");
            speech.Speak("bye");
        }

        private static void speechOn()
        {
            enabled = true;
            log.Write("Speech is now ON");
            speech.Speak("hello");
        }
    }
    
}
