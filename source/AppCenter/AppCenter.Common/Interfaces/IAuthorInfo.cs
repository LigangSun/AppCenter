using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace SoonLearning.AppCenter.Interfaces
{
    public interface IAuthorInfo
    {
        string Name
        {
            get;
        }

        string WebSite
        {
            get;
        }

        string Logo
        {
            get;
        }

    }
}
