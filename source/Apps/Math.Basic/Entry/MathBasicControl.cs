using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Math.Basic.UserControls;
using SoonLearning.AppCenter.Interfaces;

namespace Math.Basic.Entry
{
    internal class MathBasicControl : IGadgetControl
    {
        public event ControlPanelVisible ControlPanelVisibleEvent;
        private ControlAbility controlAbility = ControlAbility.CanGoback;

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
                return this.controlAbility;
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
            return ControlMgr.Instance.StartupUserControl.GoBack();
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
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
