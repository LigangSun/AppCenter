using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.ColorExplorer
{
    public class ColorObject
    {
        private string file;
        private string crashMusic;
        private string text;

        public string File
        {
            get { return this.file; }
        }

        public string CrashMusic
        {
            get { return this.crashMusic; }
        }

        public string Text
        {
            get { return this.text; }
        }

        public ColorObject(string file, string crashMusic, string text)
        {
            this.file = file;
            this.crashMusic = crashMusic;
            this.text = text;
        }
    }
}
