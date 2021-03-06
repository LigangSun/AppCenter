﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Math.Data;
using System.Collections.ObjectModel;
using SoonLearning.Assessment.Data.Decimal;
using SoonLearning.Assessment.Data.Fraction;
using SoonLearning.Assessment.Data.Arithmetic;
using SoonLearning.Assessment.Data.ArithmeticLaws;
using SoonLearning.Assessment.Data.RelationalExpression;
using SoonLearning.Assessment.Data.Equation;
using SoonLearning.Assessment.Data.Units;
using SoonLearning.Assessment.Data.Geometry;

namespace SoonLearning.Assessment.Data
{
    public class DataMgr
    {
        private static DataMgr instance;

        public static DataMgr Instance
        {
            get
            {
                if (instance == null)
                    instance = new DataMgr();

                return instance;
            }
        }

        private MathBasicType mathBasicType;
        private Dictionary<MathBasicType, ObservableCollection<MathSubTypeItem>> mathSubTypeItemDictionary = new Dictionary<MathBasicType, ObservableCollection<MathSubTypeItem>>();
        private Dictionary<MathSubType, DataCreator> dataCreatorDictionary = new Dictionary<MathSubType, DataCreator>();

        public MathBasicType ActiveMathBasicType
        {
            set { this.mathBasicType = value; }
            get { return this.mathBasicType; }
        }

        public MathSubTypeItem ActiveMathSubTypeItem
        {
            get;
            set;
        }

        public ObservableCollection<MathSubTypeItem> MathSubItemCollection
        {
            get
            {
                if (!this.mathSubTypeItemDictionary.ContainsKey(this.mathBasicType))
                {
                    switch (this.mathBasicType)
                    {
                        case MathBasicType.Integer:
                            this.mathSubTypeItemDictionary.Add(this.mathBasicType, new IntegerTypeCollection());
                            break;
                        case MathBasicType.Decimal:
                            this.mathSubTypeItemDictionary.Add(this.mathBasicType, new DecimalTypeCollection());
                            break;
                        case MathBasicType.Fraction:
                            this.mathSubTypeItemDictionary.Add(this.mathBasicType, new FractionTypeCollection());
                            break;
                        case MathBasicType.Arithmetic:
                            this.mathSubTypeItemDictionary.Add(this.mathBasicType, new ArithmeticTypeCollection());
                            break;
                        case MathBasicType.ArithmeticLaws:
                            this.mathSubTypeItemDictionary.Add(this.mathBasicType, new ArithmeticLawsTypeCollection());
                            break;
                        case MathBasicType.RelationalExpression:
                            this.mathSubTypeItemDictionary.Add(this.mathBasicType, new RelationalExpressionTypeCollection());
                            break;
                        case MathBasicType.Equation:
                            this.mathSubTypeItemDictionary.Add(this.mathBasicType, new EquationTypeCollection());
                            break;
                        case MathBasicType.Units:
                            this.mathSubTypeItemDictionary.Add(this.mathBasicType, new UnitsTypeCollection());
                            break;
                        case MathBasicType.Geometry:
                            this.mathSubTypeItemDictionary.Add(this.mathBasicType, new GeometryTypeCollection());
                            break;
                        case MathBasicType.PowerExponent:
                            this.mathSubTypeItemDictionary.Add(this.mathBasicType, new PowerExponentTypeCollection());
                            break;
                    }
                }

                return this.mathSubTypeItemDictionary[this.mathBasicType];
            }
        }

        public DataCreator DataCreator
        {
            get
            {
                return this.dataCreatorDictionary[this.ActiveMathSubTypeItem.Type];
            }
        }

        public bool IsCreatorExist
        {
            get
            {
                this.CreateDataCreator();
                return this.dataCreatorDictionary.ContainsKey(this.ActiveMathSubTypeItem.Type);
            }
        }

        private void CreateDataCreator()
        {
            if (!this.dataCreatorDictionary.ContainsKey(this.ActiveMathSubTypeItem.Type))
            {
                switch (this.ActiveMathSubTypeItem.Type)
                {
                    case MathSubType.ExactDivision:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new ExactDivisionDataCreator());
                        break;
                    case MathSubType.Submultiple:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new SubmultipleDataCreator());
                        break;
                    case MathSubType.Multiple:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new MultipleDataCreator());
                        break;
                    case MathSubType.OddNumber:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new OddNumberDataCreator());
                        break;
                    case MathSubType.EvenNumber:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new EvenNumberDataCreator());
                        break;
                    case MathSubType.PrimeNumber:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new PrimeNumberDataCreator());
                        break;
                    case MathSubType.CompositeNumber:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new CompositeNumberDataCreator());
                        break;
                    case MathSubType.PrimeFactor:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new PrimeFactorDataCreator());
                        break;
                    case MathSubType.DecompoundPrimeFactor:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new DecompoundPrimeFactorDataCreator());
                        break;
                    case MathSubType.CommonDivisor:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new CommonDivisorDataCreator());
                        break;
                    case MathSubType.CommonMultiple:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new CommonMultipleDataCreator());
                        break;
                    case MathSubType.Coprimenumbers:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new CoprimenumbersDataCreator());
                        break;

                    // Decimal
                    case MathSubType.DefinitionOfDecimal:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new DefinitionOfDecimalDataCreator());
                        break;
                    case MathSubType.FormOfDecimal:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new FormOfDecimalDataCreator());
                        break;
                    case MathSubType.PureDecimal:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new PureDecimalDataCreator());
                        break;
                    case MathSubType.WithDecimal:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new WithDecimalDataCreator());
                        break;
                    case MathSubType.FiniteDecimal:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new FiniteDecimalDataCreator());
                        break;
                    case MathSubType.InfiniteDecimal:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new InfiniteDecimalDataCreator());
                        break;

                    //Fraction
                    case MathSubType.DefinitionOfFraction:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new DefinitionOfFractionDataCreator());
                        break;
                    case MathSubType.UnitOfFraction:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new UnitOfFractionDataCreator());
                        break;
                    case MathSubType.DenominatorAndNumerator:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new DenominatorAndNumeratorDataCreator());
                        break;
                    case MathSubType.ProperFraction:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new ProperFractionDataCreator());
                        break;
                    case MathSubType.ImproperFraction:
                        break;
                    case MathSubType.WithFraction:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new WithFractionDataCreator());
                        break;

                    // Arithmetic
                    case MathSubType.Addition:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new AdditionDataCreator());
                        break;
                    case MathSubType.Subtraction:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new SubtractionDataCreator());
                        break;
                    case MathSubType.Multiplication:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new MultiplicationDataCreator());
                        break;
                    case MathSubType.Division:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new DivisionDataCreator());
                        break;

                    // Arithmetic Laws
                    case MathSubType.CommutativeLawOfAddition:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new CommutativeLawOfAdditionDataCreator());
                        break;
                    case MathSubType.CommutativeLawOfMultiplication:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new CommutativeLawOfMultiplicationDataCreator());
                        break;
                    case MathSubType.AssociativeLawOfAddition:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new AssociativeLawOfAdditionDataCreator());
                        break;
                    case MathSubType.AssociativeLawOfMultiplication:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new AssociativeLawOfMultiplicationDataCreator());
                        break;
                    case MathSubType.DistributiveLawOfMultiplication:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new DistributiveLawOfMultiplicationDataCreator());
                        break;
                    case MathSubType.CharacterOfSubtraction:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new CharacterOfSubtractionDataCreator());
                        break;
                    case MathSubType.CharacterOfDivision:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new CharacterOfDivisionDataCreator());
                        break;   

                    // Equation
                    case MathSubType.DefinitionOfEquation:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new DefinitionOfEquationDataCreator());
                        break;
                    case MathSubType.SolutionOfEquation:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new SolutionOfEquationDataCreator());
                        break;
                    case MathSubType.SolveEquation:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new SolveEquationDataCreator());
                        break;
                    
                    // PowerExponent
                    case MathSubType.MulOfSamePowerExponent:
                        this.dataCreatorDictionary.Add(this.ActiveMathSubTypeItem.Type, new MulOfSamePowerExponentDataCreator());
                        break;

                }
            }
        }

        private DataMgr()
        {
        }
    }
}
