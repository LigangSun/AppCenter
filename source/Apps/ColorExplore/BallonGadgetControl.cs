using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;

namespace Gadget.ColorExplore
{
    public class BallonGadgetControl : IGadgetControl
    {
        private static BallonGadgetControl gadgetControl;

        public static BallonGadgetControl Instance
        {
            get
            {
                if (gadgetControl == null)
                    gadgetControl = new BallonGadgetControl();

                return gadgetControl;
            }
        }

        private VerticalAlignment verticalAlignment = VerticalAlignment.Bottom;
        private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left;

        public System.Collections.ObjectModel.ObservableCollection<StageItem> StageItems
        {
            get { throw new NotImplementedException(); }
        }

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

        public ControlAbility ControlAbility
        {
            get { return ControlAbility.CanRestart | ControlAbility.HasHelpContent; }
            set
            {
            }
        }

        public System.Windows.VerticalAlignment VerticalAlignment
        {
            get
            {
                return this.verticalAlignment;
            }
            set
            {
                this.verticalAlignment = value;
                this.OnPropertyChanged("VerticalAlignment");
            }
        }

        public System.Windows.HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return this.horizontalAlignment;
            }
            set
            {
                this.horizontalAlignment = value;
                this.OnPropertyChanged("HorizontalAlignment");
            }
        }

        public bool GoBack()
        {
            return true;
        }

        public void Restart()
        {
            BallonUserControl.Instance.Restart();
        }

        public void NextStage()
        {
            throw new NotImplementedException();
        }

        public void PreStage()
        {
            throw new NotImplementedException();
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        private BallonGadgetControl()
        {
        }

        public Page Help_Request
        {
            get { throw new NotImplementedException(); }
        }

        public Page Help_Goal
        {
            get { throw new NotImplementedException(); }
        }

        public Page Help_Operation
        {
            get { throw new NotImplementedException(); }
        }

        public event ControlPanelVisible ControlPanelVisibleEvent;

        public int TotalStage
        {
            get { throw new NotImplementedException(); }
        }

        public void ShowStagePage()
        {
            throw new NotImplementedException();
        }
    }
}
