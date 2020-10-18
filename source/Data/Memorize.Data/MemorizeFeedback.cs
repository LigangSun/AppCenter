using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SoonLearning.Memorize.Data
{
    public class MemorizeFeedback
    {
        private string correctMusic = string.Empty;
        private string incorrectMusic = string.Empty;
        private string correctImage = string.Empty;
        private string incorrectImage = string.Empty;
        private string correctText = string.Empty;
        private string incorrectText = string.Empty;

        public string CorrectMusic
        {
            get { return this.correctMusic; }
            set { this.correctMusic = value; }
        }

        public string IncorrectMusic
        {
            get { return this.incorrectMusic; }
            set { this.incorrectMusic = value; }
        }

        public string CorrectImage
        {
            get { return this.correctImage; }
            set { this.correctImage = value; }
        }

        public string IncorrectImage
        {
            get { return this.incorrectImage; }
            set { this.incorrectImage = value; }
        }

        public string CorrectText
        {
            get { return this.correctText; }
            set { this.correctText = value; }
        }

        public string IncorrectText
        {
            get { return this.incorrectText; }
            set { this.incorrectText = value; }
        }

        public void Save(string path)
        {
            this.correctImage = MemorizeEntry.copyFile(this.correctImage, path);
            this.correctMusic = MemorizeEntry.copyFile(this.correctMusic, path);
            this.incorrectImage = MemorizeEntry.copyFile(this.incorrectImage, path);
            this.incorrectMusic = MemorizeEntry.copyFile(this.incorrectMusic, path);
        }

        internal void getFullPath(string path)
        {
            this.CorrectImage = MemorizeEntry.getFileFullPath(path, this.CorrectImage);
            this.CorrectMusic = MemorizeEntry.getFileFullPath(path, this.CorrectMusic);
            this.IncorrectImage = MemorizeEntry.getFileFullPath(path, this.IncorrectImage);
            this.IncorrectMusic = MemorizeEntry.getFileFullPath(path, this.IncorrectMusic);
        }
    }
}
