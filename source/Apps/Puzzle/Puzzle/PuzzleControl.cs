using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.AppCenter;
using System.Windows;
using System.ComponentModel;
using System.Windows.Documents;
using System.Windows.Controls;
using SoonLearning.BlockPuzzle.Help;
using SoonLearning.BlockPuzzle.Data;
using SoonLearning.AppCenter.Interfaces;

namespace SoonLearning.BlockPuzzle.Puzzle
{
    public class PuzzleControl : IGadgetControl
    {
        private static PuzzleControl gadgetControl;

        public static PuzzleControl Instance
        {
            get
            {
                if (gadgetControl == null)
                    gadgetControl = new PuzzleControl();

                return gadgetControl;
            }
        }

        private PuzzleControl()
        {
        }

        private VerticalAlignment verticalAlignment = VerticalAlignment.Bottom;
        private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left;
        private ControlAbility controlAbility = ControlAbility.None;

        public ControlAbility ControlAbility
        {
            get { return this.controlAbility; }
            set
            {
                this.controlAbility = value;
                this.OnPropertyChanged("ControlAbility");
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

        public Page Help_Request
        {
            get { return new RequestPage(); }
        }

        public Page Help_Goal
        {
            get { return new GoalPage(); }
        }

        public Page Help_Operation
        {
            get { return new OperationPage(); }
        }

        public bool GoBack()
        {
            return ControlMgr.Instance.PuzzleStartupPage.GoBack();
        }

        public void Restart()
        {
            ControlMgr.Instance.PuzzlePage.Restart();
        }

        public void NextStage()
        {
            ControlMgr.Instance.PuzzlePage.NextStage();
        }

        public void PreStage()
        {
            ControlMgr.Instance.PuzzlePage.PreStage();
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
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

        public void ShowStagePage()
        {
            ControlMgr.Instance.PuzzleStartupPage.ShowPuzzleList();
        }

        public int TotalStage
        {
            get { return Enumerable.Count<PuzzleItem>(ControlMgr.Instance.DataMgr.PuzzleItems.Cast<PuzzleItem>()); }
        }

        internal void ControlPanlVisible(bool visible)
        {
            if (ControlPanelVisibleEvent != null)
                ControlPanelVisibleEvent(visible);
        }
    }
}
