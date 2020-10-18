using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using SoonLearning.AppCenter;
using System.Reflection;
using SoonLearning.Math.Data;
using Math.Basic.Data;
using Math.Basic.Entry;
using SoonLearning.AppCenter.Interfaces;

namespace Math.Basic.UserControls
{
    internal class ControlMgr
    {
        private static ControlMgr instance;

        public static ControlMgr Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ControlMgr();
                }

                return instance;
            }
        }

        private ControlMgr()
        {
            this.idDictionary.Add(MathBasicType.Arithmetic, "{50CAA420-0F4F-411C-A3BA-BDEF14B657A1}");
            this.idDictionary.Add(MathBasicType.ArithmeticLaws, "{460041C5-A975-455C-A5E9-4AECCA9A0C3C}");
            this.idDictionary.Add(MathBasicType.Decimal, "{575F8C6A-9C3C-486F-AF9F-DB337892DC35}");
            this.idDictionary.Add(MathBasicType.Equation, "{F438B509-566D-4150-AF9C-BBCB7035DA73}");
            this.idDictionary.Add(MathBasicType.Fraction, "{262D221D-FA43-473E-8785-26C3FDD0A910}");
            this.idDictionary.Add(MathBasicType.Geometry, "{60203C99-4F47-49D7-BB28-6EAB8FAF3513}");
            this.idDictionary.Add(MathBasicType.Integer, "{5E1C4E65-57F6-48AD-805E-CDB462EAF8C7}");
            this.idDictionary.Add(MathBasicType.RelationalExpression, "{91D04D1B-0A99-4943-8EB6-49FECFFF90F8}");
            this.idDictionary.Add(MathBasicType.Units, "{AF0DDE19-8701-4CEC-8F94-2751DD18C757}");
        }

        private Dictionary<string, UIElement> controlDictionary = new Dictionary<string, UIElement>();
        private IGadgetEntry entry;
        private MathBasicControl mathBasicControl = new MathBasicControl();

        private Dictionary<MathBasicType, string> idDictionary = new Dictionary<MathBasicType, string>();

        internal StartupUserControl StartupUserControl
        {
            get
            {
                return this.GetControl(DataMgr.Instance.ActiveMathBasicType.ToString() + "_StartupUserControl", 
                    typeof(StartupUserControl).FullName) as StartupUserControl;
            }
        }

        internal MathBasicControl MathBasicControl
        {
            get { return this.mathBasicControl; }
        }

        internal IGadgetEntry Entry
        {
            get { return this.entry; }
            set { this.entry = value; }
        }

        private UIElement GetControl(string key, string type)
        {
            if (!this.controlDictionary.ContainsKey(key))
            {
                Assembly assembly = Assembly.GetCallingAssembly();
                this.controlDictionary.Add(key, assembly.CreateInstance(type) as UIElement);
            }

            return this.controlDictionary[key];
        }
    }
}
