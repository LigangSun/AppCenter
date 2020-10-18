using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Collections.ObjectModel;
using SoonLearning.AppCenter.Data;

namespace SoonLearning.AppCenter.Interfaces
{
    [Flags]
    public enum AppCapability
    {
        None = 0x00,
        SpeechRecognization = 0x01,
        BackgroundMusic = 0x02,
    }

    public interface IGadgetEntry// : MarshalByRefObject
    {
        string Id
        {
            get;
        }

        DateTime CreateDate
        {
            get;
        }

        string Title
        {
            get;
        }

        string Description
        {
            get;
        }

        string Thumbnail
        {
            get;
        }

        int Tag
        {
            get;
        }

        int SubTag
        {
            get;
        }

        AppCapability Capability
        {
            get;
        }

        UIElement GetStartupPage();

        void Uninstall();
    }
}
