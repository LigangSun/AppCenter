using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.AppCenter;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;

namespace Gadget.ColorExplore
{
    public class ColorClickGadgetControl: IGadgetControl
    {
        private static ColorClickGadgetControl gadgetControl;

        public static ColorClickGadgetControl Instance
        {
            get
            {
                if (gadgetControl == null)
                    gadgetControl = new ColorClickGadgetControl();

                return gadgetControl;
            }
        }

        private VerticalAlignment verticalAlignment = VerticalAlignment.Bottom;
        private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left;

        public System.Collections.ObjectModel.ObservableCollection<StageItem> StageItems
        {
            get { throw new NotImplementedException(); }
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
            ColorClickUserControl.Instance.Restart();
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

        private ColorClickGadgetControl()
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
