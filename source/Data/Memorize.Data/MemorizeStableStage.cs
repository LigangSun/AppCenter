using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.Memorize.Data
{
    public class MemorizeStableStage : MemorizeStage
    {
        private List<string> memorizeItemIdList = new List<string>();

        public List<string> MemorizeItemIds
        {
            get { return this.memorizeItemIdList; }
        }

        public override string ToString()
        {
            return "静态关卡";
        }
    }
}
