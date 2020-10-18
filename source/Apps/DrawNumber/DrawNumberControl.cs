using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.AppCenter;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using SoonLearning.AppCenter.Interfaces;

namespace SoonLearning.ConnectNumber
{
    public class DrawNumberControl : IGadgetControl
    {
        private static DrawNumberControl gadgetControl;

        public static DrawNumberControl Instance
        {
            get
            {
                if (gadgetControl == null)
                    gadgetControl = new DrawNumberControl();

                return gadgetControl;
            }
        }

        public ControlAbility ControlAbility
        {
            get { return ControlAbility.CanRestart | 
                ControlAbility.HasHelpContent | 
                ControlAbility.CanSwitchStage | 
                ControlAbility.CanShowStageList; }
            set
            {
            }
        }

        public bool GoBack()
        {
            return true;
        }

        public void Restart()
        {
            
        }

        public void NextStage()
        {
            ControlMgr.Instance.DrawNumberPage.Next();
        }

        public void PreStage()
        {
            ControlMgr.Instance.DrawNumberPage.Previous();
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private DrawNumberControl()
        {
        }

        public Page Help_Request
        {
            get { return new Page(); }
        }

        public Page Help_Goal
        {
            get { return new Page(); }
        }

        public Page Help_Operation
        {
            get { return new Page(); }
        }

        public int SelectedStage
        {
            get
            {
                return ControlMgr.Instance.DataMgr.CurrentIndex;
            }
            set
            {
                ControlMgr.Instance.DataMgr.CurrentIndex = value;
                this.OnPropertyChanged("SelectedStage");
            }
        }

        public event ControlPanelVisible ControlPanelVisibleEvent;

        public int TotalStage
        {
            get { return Enumerable.Count<DrawNumberItem>(ControlMgr.Instance.DataMgr.DrawNumberItems.Cast<DrawNumberItem>()); }
        }

        public void ShowStagePage()
        {
            ControlMgr.Instance.DrawNumberStartupPage.ShowListPage();
        }

        internal void ControlPanlVisible(bool visible)
        {
            if (ControlPanelVisibleEvent != null)
                ControlPanelVisibleEvent(visible);
        }
    }
}
