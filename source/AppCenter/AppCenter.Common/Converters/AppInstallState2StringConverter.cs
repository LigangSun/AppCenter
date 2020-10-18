using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Properties;

namespace SoonLearning.AppCenter.Converter
{
    public class AppInstallState2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            InstallState state = (InstallState)value;
            switch (state)
            {
                case InstallState.NotStart:
                    return Resources.NotStart;
                case InstallState.Done:
                    return Resources.InstallDone;
                case InstallState.DownloadFail:
                    return Resources.DownloadFail;
                case InstallState.Downloading:
                    return Resources.Downloading;
                case InstallState.InstallFail:
                    return Resources.InstallFail;
                case InstallState.InstallFail_FileInUse:
                    return Resources.InstallFail;
                case InstallState.InstallFail_PathTooLongException:
                    return Resources.InstallFail;
                case InstallState.InstallFail_RequirePermission:
                    return Resources.InstallFail;
                case InstallState.InstallFail_UnauthorizedAccess:
                    return Resources.InstallFail;
                case InstallState.Installing:
                    return Resources.Installing;
                case InstallState.UserCancelled:
                    return Resources.UserCancelled;
                case InstallState.UserCancelling:
                    return Resources.UserCancelling;
            }
            
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
