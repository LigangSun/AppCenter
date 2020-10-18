using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.AppManagementTool
{
    public class ProjectData
    {
        private List<string> additionalFileList = new List<string>();

        public string MainFile
        {
            get;
            set;
        }

        public List<string> AdditionalFiles
        {
            get { return this.additionalFileList; }
        }
    }
}
