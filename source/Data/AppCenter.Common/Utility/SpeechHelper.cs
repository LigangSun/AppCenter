using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Globalization;

namespace SoonLearning.AppCenter.Utility
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
        private SpeechRecognitionEngine speechRecognizer;
        private EventHandler<RecognizeCompletedEventArgs> recognizerCallback;

        private SpeechSynthesizer SpeechSynthesizer
        {
            get
            {
                if (this.speechSynthesizer == null)
                    this.speechSynthesizer = new SpeechSynthesizer();

                return this.speechSynthesizer;
            }
        }

        private SpeechRecognitionEngine SpeechRecognizer
        {
            get
            {
                if (this.speechRecognizer == null)
                {
                    foreach (RecognizerInfo info in SpeechRecognitionEngine.InstalledRecognizers())
                    {
                        if (info.Culture == CultureInfo.CurrentCulture)
                        {
                            this.speechRecognizer = new SpeechRecognitionEngine(info);
                            break;
                        }
                    }

                    if (this.speechRecognizer == null)
                        this.speechRecognizer = new SpeechRecognitionEngine();

                    this.speechRecognizer.SetInputToDefaultAudioDevice();
                }
                    
                return this.speechRecognizer;
            }
        }

        public bool RecognizerEnabled
        {
            get;
            set;
        }

        public void EnableRecognizer()
        {
            try
            {
                if (this.speechRecognizer == null)
                    return;

                if (this.RecognizerEnabled)
                {
                    this.SpeechRecognizer.RecognizeAsync(RecognizeMode.Single);
                }
                else
                {
                    this.SpeechRecognizer.RecognizeAsyncStop();
                }
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
                this.SpeechSynthesizer.Volume = 100;
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

        public void StartRecognizer(IEnumerable<string> textList, EventHandler<RecognizeCompletedEventArgs> callback)
        {
            Choices choices = new Choices();
            choices.Add(Enumerable.ToArray<string>(textList));

            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(choices);

            this.recognizerCallback = callback;

            this.SpeechRecognizer.UnloadAllGrammars();

            Grammar g = new Grammar(gb);
            this.SpeechRecognizer.LoadGrammar(g);
            this.SpeechRecognizer.RecognizeCompleted += new EventHandler<RecognizeCompletedEventArgs>(SpeechRecognizer_RecognizeCompleted);

            if (!this.RecognizerEnabled)
                return;

            this.SpeechRecognizer.RecognizeAsync(RecognizeMode.Single);
        }

        public void EndRecognizer()
        {
            if (this.SpeechRecognizer == null)
                return;

            this.SpeechRecognizer.RecognizeCompleted -= SpeechRecognizer_RecognizeCompleted;
            this.recognizerCallback = null;
            this.SpeechRecognizer.UnloadAllGrammars();
            this.SpeechRecognizer.RecognizeAsyncStop();
        }

        private void SpeechRecognizer_RecognizeCompleted(object sender, RecognizeCompletedEventArgs e)
        {
            if (this.recognizerCallback != null)
                this.recognizerCallback(this, e);

            if (!this.RecognizerEnabled)
                return;

            ((SpeechRecognitionEngine)sender).RecognizeAsyncStop();
            ((SpeechRecognitionEngine)sender).RecognizeAsync(RecognizeMode.Single);
        }

        private void SpeechRecognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
        //    if (this.recognizerCallback != null)
        //        this.recognizerCallback(this, e);

            ((SpeechRecognitionEngine)sender).RecognizeAsync(RecognizeMode.Single);
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
