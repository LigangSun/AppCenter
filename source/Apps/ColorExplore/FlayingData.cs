using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace SoonLearning.ColorExplorer
{
    public class FlyingData
    {
        private int index;
        private Image image;
        private Storyboard storyboard;

        public int Index
        {
            get { return this.index; }
        }

        public Image Image
        {
            get { return this.image; }
        }

        public Storyboard Storyboard
        {
            get
            {
                if (this.storyboard == null)
                    this.storyboard = new Storyboard();

                return this.storyboard;
            }
        }

        public FlyingData(int index, Image image)
        {
            this.index = index;
            this.image = image;
        }
    }
}
