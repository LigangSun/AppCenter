using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadgetCenter;
using SoonLearning.AppCenter.Interfaces;

namespace Gadget.ReadCount
{
    public class ReadCountControl : IGadgetControl
    {
        public event ControlPanelVisible ControlPanelVisibleEvent;

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
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
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
            return true;
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
    }
}
