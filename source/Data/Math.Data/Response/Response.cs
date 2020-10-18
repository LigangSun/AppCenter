using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    public class Response : BaseObject
    {
        private string objectId;

        public string ObjectId
        {
            get { return this.objectId; }
            set { this.objectId = value; }
        }

        public Response()
        {
        }

        public Response(string objectId)
        {
            this.objectId = objectId;
        }
    }
}
