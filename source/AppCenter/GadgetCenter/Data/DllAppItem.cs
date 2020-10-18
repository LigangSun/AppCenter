using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.AppCenter.Interfaces;
using System.IO;

namespace SoonLearning.AppCenter.Data
{
    public class DllAppItem : AppItem
    {
         public DllAppItem()
            : base()
        {
        }

         public DllAppItem(string id, 
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
