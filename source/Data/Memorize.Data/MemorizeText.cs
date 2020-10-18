using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.Memorize.Data
{
    public class MemorizeText : MemorizeObject
    {
        private string text = string.Empty;

        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }

        public MemorizeText()
        {
        }

        public MemorizeText(string text)
        {
            this.text = text;
        }

        public override void Save(string path)
        {
            
        }

        public override void getFullPath(string path)
        {
        }

        public override string ToString()
        {
            return this.Text;
        }
    }
}
