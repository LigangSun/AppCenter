using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Math.Fast.RapidCalculation;
using SoonLearning.AppCenter.Interfaces;

namespace SoonLearning.Math.Fast
{
    public class RapidGadgetControl : IGadgetControl
    {
        private static RapidGadgetControl rapidGadgetControl;

        public static RapidGadgetControl Instance
        {
            get
            {
                if (rapidGadgetControl == null)
                    rapidGadgetControl = new RapidGadgetControl();

                return rapidGadgetControl;
            }
        }

        public event ControlPanelVisible ControlPanelVisibleEvent;
        private ControlAbility controlAbility = ControlAbility.None;

        public int SelectedStage
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int TotalStage
        {
            get { throw new NotImplementedException(); }
        }

        public ControlAbility ControlAbility
        {
            get
            {
                return this.controlAbility; ;
            }
            set
            {
                this.controlAbility = value;
                this.OnPropertyChanged("ControlAbility");
            }
        }

        public System.Windows.Controls.Page Help_Request
        {
            get { throw new NotImplementedException(); }
        }

        public System.Windows.Controls.Page Help_Goal
        {
            get { throw new NotImplementedException(); }
        }

        public System.Windows.Controls.Page Help_Operation
        {
            get { throw new NotImplementedException(); }
        }

        public bool GoBack()
        {
            return ControlMgr.Instance.MathStartupControl.GoBack();
        }

        public void Restart()
        {
            throw new NotImplementedException();
        }

        public void NextStage()
        {
            throw new NotImplementedException();
        }

        public void PreStage()
        {
            throw new NotImplementedException();
        }

        public void ShowStagePage()
        {
            throw new NotImplementedException();
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(name));
        }
    }
}
