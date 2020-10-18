using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading;

namespace SoonLearning.Memorize.Data
{
    public class MemorizeMathStage : MemorizeStage
    {
        private bool plus = true;
        private bool minus;
        private bool multiplication;
        private bool division;

        private int minValue;
        private int maxValue;

        private int itemCount;

        private SortedSet<int> selectedValueSet = new SortedSet<int>();

        private List<MemorizeItem> memorizeItemCollection = new List<MemorizeItem>();

        public List<MemorizeItem> Items
        {
            get { return this.memorizeItemCollection; }
        }

        public int ItemCount
        {
            get { return this.itemCount; }
            set
            {
                this.itemCount = value;
                base.OnPropertyChanged("ItemCount");
            }
        }

        public bool Plus
        {
            get { return this.plus; }
            set
            {
                this.plus = value;
                base.OnPropertyChanged("Plus");
            }
        }

        public bool Minus
        {
            get { return this.minus; }
            set
            {
                this.minus = value;
                base.OnPropertyChanged("Minus");
            }
        }

        public bool Multiplication
        {
            get { return this.multiplication; }
            set
            {
                this.multiplication = value;
                base.OnPropertyChanged("Multiplication");
            }
        }

        public bool Division
        {
            get { return this.division; }
            set
            {
                this.division = value;
                base.OnPropertyChanged("Division");
            }
        }

        public int MinValue
        {
            get { return this.minValue; }
            set
            {
                this.minValue = value;
                base.OnPropertyChanged("MinValue");
            }
        }

        public int MaxValue
        {
            get { return this.maxValue; }
            set
            {
                this.maxValue = value;
                base.OnPropertyChanged("MaxValue");
            }
        }

        public void GenerateItems()
        {
            this.Items.Clear();
            this.selectedValueSet.Clear();

            int partCount = 0;
            if (this.plus)
                partCount++;
            if (this.minus)
                partCount++;
            if (this.multiplication)
                partCount++;
            if (this.division)
                partCount++;

            if (partCount == 0)
                return;

            int count = this.itemCount / partCount + this.itemCount % partCount;

            if (this.Plus)
                this.generatePlusExpression(this.MinValue, this.MaxValue, count);

            if (this.Minus)
                this.generateMinusExpression(this.MinValue, this.MaxValue, count);

            if (this.Multiplication)
                this.generateMultiplyExpression(this.MinValue, this.MaxValue, count);

            if (this.Division)
                this.generateDivisionExpression(this.MinValue, this.MaxValue, count);

            if (this.Items.Count > this.itemCount)
            {
                this.shuffleItems();
                this.Items.RemoveRange(this.itemCount, this.Items.Count - this.itemCount);
            }
        }

        private void shuffleItems()
        {
            Random rand = new Random(Environment.TickCount);
            int randCount = this.Items.Count;
            while (true)
            {
                if (randCount <= 1)
                    break;

                int randIndex = rand.Next(randCount - 1);
                var temp = this.Items[randCount - 1];
                this.Items[randCount - 1] = this.Items[randIndex];
                this.Items[randIndex] = temp;

                randCount--;
            }
        }

        private void generatePlusExpression(int minValue, int maxValue, int count)
        {
            Random rand = new Random(Environment.TickCount);
            foreach (int value in this.randomValue(minValue, maxValue, count))
            {
                int a = rand.Next(minValue, value+1);
                int b = value - a;
                this.createMemorizeItem(string.Format("{0}+{1}", a, b), value.ToString());
            }
        }

        private void generateMinusExpression(int minValue, int maxValue, int count)
        {
            Random rand = new Random(Environment.TickCount);
            foreach (int value in this.randomValue(minValue, maxValue, count))
            {
                while (true)
                {
                    int a = rand.Next(value, maxValue + 1);
                    if (value == maxValue)
                        a = maxValue;
                    int b = a - value;
                    if (b < 0)
                    {
                        Thread.Sleep(20);
                        continue;
                    }
                    this.createMemorizeItem(string.Format("{0}-{1}", a, b), value.ToString());
                    break;
                }
            }
        }

        private IEnumerable<int> randomValue(int minValue, int maxValue, int count)
        {
            List<int> indexList = new List<int>();
            int max = maxValue - minValue + 1;
            if (minValue == 0)
                max--;
            for (int i = minValue; i <= max; i++)
            {
                indexList.Add(i);
            }

            Random rand = new Random(Environment.TickCount);
            for (int i = 0; i < count; i++)
            {
                int randIndex = rand.Next(0, indexList.Count);
                if (indexList.Count == 0)
                {
                    yield return rand.Next(minValue, maxValue);
                    continue;
                }

                int value = indexList[randIndex];
                indexList.RemoveAt(randIndex);

                var queryValue = from temp in this.selectedValueSet
                                 where temp == value
                                 select temp;
                if (queryValue.Count() > 0)
                {
                    i--;
                    continue;
                }

                this.selectedValueSet.Add(value);

                yield return value;
            }
        }

        private void generateMultiplyExpression(int minValue, int maxValue, int count)
        {
            Random rand = new Random(Environment.TickCount);
            foreach (int value in this.randomResult(minValue, maxValue, count))
            {
                while (true)
                {
                    int a = rand.Next(minValue, maxValue + 1);
                    int b = value / a;
                    if (value % a == 0 && (b >= minValue && b <= maxValue))
                    {
                        this.createMemorizeItem(string.Format("{0}×{1}", a, b), value.ToString());
                        break;
                    }
                }
            }
        }

        private void generateDivisionExpression(int minValue, int maxValue, int count)
        {
            Random rand = new Random(Environment.TickCount);
            foreach (int value in this.randomValue(minValue, maxValue, count))
            {
                while (true)
                {
                    int a = rand.Next(minValue, maxValue + 1);
                    int b = value * a;
               //     if (value % a == 0)
                    {
                        this.createMemorizeItem(string.Format("{0}÷{1}", b, a), value.ToString());
                        break;
                    }
                }
            }
        }

        private IEnumerable<int> randomResult(int minValue, int maxValue, int count)
        {
            Dictionary<int, string> allResultDictionary = new Dictionary<int, string>();
            for (int i = minValue; i <= maxValue; i++)
            {
                for (int j = minValue; j <= maxValue; j++)
                {
                    int a = i;
                    int b = j;
                    int c = a*b;
                    if (!allResultDictionary.ContainsKey(c))
                    {
                        allResultDictionary.Add(c, string.Format("{0}x{1}", a, b));
                    }
                }
            }

            List<int> indexList = new List<int>();
            foreach (int key in allResultDictionary.Keys)
            {
                indexList.Add(key);
            }

            Random rand = new Random(Environment.TickCount);
            for (int i = 0; i < count; i++)
            {
                int randIndex = rand.Next(0, indexList.Count);
                int value = indexList[randIndex];
                indexList.RemoveAt(randIndex);

                var queryValue = from temp in this.selectedValueSet
                                 where temp == value
                                 select temp;
                if (queryValue.Count() > 0)
                {
                    i--;
                    continue;
                }

                this.selectedValueSet.Add(value);

                yield return value;
            }
        }

        private void createMemorizeItem(string left, string right)
        {
            MemorizeItem item = new MemorizeItem();

            MemorizeText leftText = new MemorizeText(left);
            MemorizeText rightText = new MemorizeText(right);
            item.ObjectA = leftText;
            item.ObjectB = rightText;

            memorizeItemCollection.Add(item);
        }

        public override string ToString()
        {
            return string.Format("数学算式关卡 - 最小值:{0} 最大值:{1} 数量:{2}", this.minValue, this.maxValue, this.itemCount);
        }
    }
}
