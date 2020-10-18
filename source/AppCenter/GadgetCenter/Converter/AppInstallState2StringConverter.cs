using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using GadgetCenter.Data;
using GadgetCenter.Resources;
using SoonLearning.AppCenter.Data;

namespace GadgetCenter.Converter
{
    public class AppInstallState2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            InstallState state = (InstallState)value;
            switch (state)
            {
                case InstallState.NotStart:
                    return Strings.NotStart;
                case InstallState.Done:
                    return Strings.Done;
                case InstallState.DownloadFail:
                    return Strings.DownloadFail;
                case InstallState.Downloading:
                    return Strings.Downloading;
                case InstallState.InstallFail:
                    return Strings.InstallFail;
                case InstallState.Installing:
                    return Strings.Installing;
                case InstallState.UserCancelled:
                    return Strings.UserCancelled;
                case InstallState.UserCancelling:
                    return Strings.UserCancelling;
            }
            
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
