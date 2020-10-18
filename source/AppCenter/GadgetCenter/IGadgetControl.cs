using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using GadgetCenter.Data;
using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;

namespace GadgetCenter
{
    [Flags]
    public enum ControlAbility
    {
        None = 0x00,
        CanGoback = 0x01,
        CanRestart = 0x02,
        CanSwitchStage = 0x04,
        CanShowStageList = 0x08,
        HasHelpContent = 0x010,
    }

    public delegate void ControlPanelVisible(bool isVisible);

    public interface IGadgetControl : INotifyPropertyChanged
    {
        event ControlPanelVisible ControlPanelVisibleEvent;

        int SelectedStage
        {
            get;
            set;
        }

        int TotalStage
        {
            get;
        }

        ControlAbility ControlAbility
        {
            get;
            set;
        }

        Page Help_Request
        {
            get;
        }

        Page Help_Goal
        {
            get;
        }

        Page Help_Operation
        {
            get;
        }

        bool GoBack();
        void Restart();
        void NextStage();
        void PreStage();
        void ShowStagePage();
    }
}
