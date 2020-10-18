using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.Memorize.Data
{
    public class MemorizeItem
    {
        private string id = string.Empty;
        private MemorizeObject objectA;
        private MemorizeObject objectB;

        public string Id
        {
            get { return this.id; }
            set
            { 
                this.id = value;
                this.setItemId();
            }
        }

        public MemorizeObject ObjectA
        {
            get { return this.objectA; }
            set 
            {
                this.objectA = value;
                this.setItemId();
            }
        }

        public MemorizeObject ObjectB
        {
            get { return this.objectB; }
            set 
            { 
                this.objectB = value;
                this.setItemId();
            }
        }

        public MemorizeItem()
        {
            this.id = Guid.NewGuid().ToString("N");
        }

        public override string ToString()
        {
            return string.Format("{0} -- {1}", this.objectA.ToString(), this.objectB.ToString());
        }

        private void setItemId()
        {
            if (this.objectA != null)
                this.objectA.setItemId(this.Id);

            if (this.objectB != null)
                this.objectB.setItemId(this.id);
        }
    }
}
