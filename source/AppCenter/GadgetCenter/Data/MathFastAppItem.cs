using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.AppCenter.Data
{
    public class MathFastAppItem : AppItem
    {
        public MathFastAppItem()
            : base()
        {
        }

        public MathFastAppItem(string id, 
            string title, 
            string description, 
            DateTime createDate,
            string thumbnailUri,
            int type,
            int subType, 
            string fullName,
            string creatorName,
            string creatorWebSite,
            string creatorLogo)
            : base(id, title, description, createDate, thumbnailUri, type, subType, fullName, creatorName, creatorWebSite, creatorLogo)
        {
            
        }
    }
}
