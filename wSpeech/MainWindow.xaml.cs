using System;
using System.Speech.Recognition;
using System.Windows;

namespace wSpeech
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static bool done = false;
        static bool speechOn = true;
        public MainWindow()
        {
            InitializeComponent();
            init();
        }

        void log(string m, params string[] args)
        {
            Console.WriteLine(m, args);
            tLog.AppendText(string.Format(m + "\n", args));
        }

        void speak(string msg, params object[] args)
        {
            cSpeech.speech.Speak(string.Format(msg, args));
        }
        void ac_fareWell()
        {
            speak("Farewell");
        }
        void init()
        {
            cSpeech.cmn.actions.Add("klatu barada nikto", ac_fareWell);

            addSummGrammar();

            cSpeech.cmn.init();

            log("\n(Speaking: I am awake)");
            speak("I am awake");

            cSpeech.log.Logged += Log_Logged;
            cSpeech.cmn.SpeechRecognizedCustom += Cmn_SpeechRecognizedCustom;

        }

        private void Cmn_SpeechRecognizedCustom(object sender, SpeechRecognizedEventArgs e)
        {
            var txt = e.Result.Text;
            if (txt.IndexOf("What") >= 0 && txt.IndexOf("plus") >= 0) // what is 2 plus 3
            {
                string[] words = txt.Split(' ');     // or use e.Result.Words
                int num1 = int.Parse(words[2]);
                int num2 = int.Parse(words[4]);
                int sum = num1 + num2;
                log("(Speaking: " + words[2] + " plus " + words[4] + " equals " + sum + ")");
                speak("{0} plus {1} equals {2}", words[2], words[4], sum);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            cSpeech.log.Logged -= Log_Logged;
            base.OnClosed(e);
        }

        private void Log_Logged(object sender, cSpeech.log.LogEventArgs e)
        {
            log("\n" + e.GetMessage());
        }

        private static void addSummGrammar()
        {
            string[] numbers = new string[100];
            for (int i = 0; i < 100; ++i)
                numbers[i] = i.ToString();
            Choices ch_Numbers = new Choices(numbers);

            GrammarBuilder gb_WhatIsXplusY = new GrammarBuilder();
            gb_WhatIsXplusY.Append("What is");
            gb_WhatIsXplusY.Append(ch_Numbers);
            gb_WhatIsXplusY.Append("plus");
            gb_WhatIsXplusY.Append(ch_Numbers);
            Grammar g_WhatIsXplusY = new Grammar(gb_WhatIsXplusY);

            cSpeech.cmn.AddGrammar(g_WhatIsXplusY);
        }
        
    }
}
