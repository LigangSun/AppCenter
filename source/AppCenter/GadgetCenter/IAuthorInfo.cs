using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace GadgetCenter
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

        BitmapImage Logo
        {
            get;
        }

    }
}
