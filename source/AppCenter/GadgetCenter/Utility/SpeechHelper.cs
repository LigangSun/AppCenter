using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;
using System.Speech.Recognition;

namespace GadgetCenter.Utility
{
    public class SpeechHelper : IDisposable
    {
        private static SpeechHelper helper;

        public static SpeechHelper Instance
        {
            get
            {
                if (helper == null)
                {
                    helper = new SpeechHelper();
                }

                return helper;
            }
        }

        private SpeechSynthesizer speechSynthesizer;
        private SpeechRecognizer speechRecognizer;
        private EventHandler<SpeechRecognizedEventArgs> recognizerCallback; 

        private SpeechSynthesizer SpeechSynthesizer
        {
            get
            {
                if (this.speechSynthesizer == null)
                    this.speechSynthesizer = new SpeechSynthesizer();

                return this.speechSynthesizer;
            }
        }

        private SpeechRecognizer SpeechRecognizer
        {
            get
            {
                if (this.speechRecognizer == null)
                    this.speechRecognizer = new SpeechRecognizer();

                return this.speechRecognizer;
            }
        }

        internal void EnableRecognizer()
        {
            try
            {
                if (this.speechRecognizer == null &&
                    !App.StyleSetting.SpeechRecognizer)
                    return;

                this.SpeechRecognizer.Enabled = App.StyleSetting.SpeechRecognizer;
            }
            catch
            {
            }
        }

        public void Speak(string text)
        {
            try
            {
                Prompt prompt = new Prompt(text);
                this.SpeechSynthesizer.Speak(prompt);
            }
            catch
            {
            }
        }

        public void SpeakAsync(string text)
        {
            try
            {
                Prompt prompt = new Prompt(text);
                this.SpeechSynthesizer.SpeakAsync(prompt);
            }
            catch
            {
            }
        }

        public void StartRecognizer(IEnumerable<string> textList, EventHandler<SpeechRecognizedEventArgs> callback)
        {
            if (!App.StyleSetting.SpeechRecognizer)
                return;

            Choices choices = new Choices();
            choices.Add(Enumerable.ToArray<string>(textList));

            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(choices);

            this.recognizerCallback = callback;

            this.SpeechRecognizer.UnloadAllGrammars();

            Grammar g = new Grammar(gb);
            this.SpeechRecognizer.LoadGrammar(g);
            this.SpeechRecognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognizer_SpeechRecognized);
        }

        public void EndRecognizer()
        {
            if (!App.StyleSetting.SpeechRecognizer)
                return;

            this.SpeechRecognizer.SpeechRecognized -= SpeechRecognizer_SpeechRecognized;
            this.recognizerCallback = null;
            this.SpeechRecognizer.UnloadAllGrammars();
        }

        private void SpeechRecognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (this.recognizerCallback != null)
                this.recognizerCallback(this, e);
        }

        public void Dispose()
        {
            if (this.speechRecognizer != null)
            {
                this.speechRecognizer.Dispose();
                this.speechRecognizer = null;
            }

            if (this.speechSynthesizer != null)
            {
                this.speechSynthesizer.Dispose();
                this.speechSynthesizer = null;
            }

            this.recognizerCallback = null;
        }
    }
}
