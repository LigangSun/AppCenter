using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    public enum MathBasicType
    {
        Integer = 0x0000,
        Decimal = 0x1000,
        Fraction = 0x2000,
        Arithmetic = 0x3000,
        ArithmeticLaws = 0x4000,
        RelationalExpression = 0x5000,
        Equation = 0x6000,
        Units = 0x7000,
        Geometry = 0x8000,

        PowerExponent = 0x9000
    }

    public enum MathSubType
    {
        // Integer
        ExactDivision = 0x0001,
        Submultiple,
        Multiple,
        OddNumber,
        EvenNumber,
        PrimeNumber,
        CompositeNumber,
        PrimeFactor,
        DecompoundPrimeFactor,
        CommonDivisor,
        CommonMultiple,
        Coprimenumbers,

        // Decimal
        DefinitionOfDecimal = 0x1001,
        FormOfDecimal,
        PureDecimal,
        WithDecimal,
        FiniteDecimal,
        InfiniteDecimal,
        InfiniteNonRepeatingDecimal,
        RepeatingDecimal,
        PureRepeatingDecimal,
        MixedRepeatingDecimal,

        // Fraction
        DefinitionOfFraction = 0x2001,
        UnitOfFraction,
        DenominatorAndNumerator,
        ProperFraction,
        ImproperFraction,
        WithFraction,
        FractionInLowestTerm,
        ReductionOfFraction,
        ReductionToCommonDenomiator,
        RelationshipInFractionAndDivision,
        RelationshipInFractionAndDecimal,
        RelationshipInFractionAndRatio,
        RelationshipInFractionAndFiniteDecimal,
        CharacterOfFraction,
        Percent,

        // Arithmetic
        Addition = 0x3001,
        Subtraction,
        Multiplication,
        Division,

        // ArithmeticLaws
        CommutativeLawOfAddition = 0x4001,
        CommutativeLawOfMultiplication,
        AssociativeLawOfAddition,
        AssociativeLawOfMultiplication,
        DistributiveLawOfMultiplication,
        CharacterOfSubtraction,
        CharacterOfDivision,

        // RelationalExpression
        ProblemsOfSpeed = 0x5001,
        ProblemsOfWork,
        ProblemsOfPrice,

        // Equation
        DefinitionOfEquation = 0x6001,
        SolutionOfEquation,
        SolveEquation,

        // Units
        UnitsOfLength = 0x7001,
        UnitsOfArea,
        UnitsOfVolume,
        UnitsOfWeight,
        UnitsOfTime,
        Month,
        Year,
        ConcreteNumber,

        // Geometry
        Line = 0x8001,
        Angle,
        SortOfAngle,
        VerticalLine,
        ParallelLine,
        Triangle,
        SortOfTriangle,
        Quadrangle,
        Circle,
        SymmetricFigure,
        Girth,
        Area,
        SurfaceArea,
        Volume,
        CuboidAndCube,
        Column,
        CircumferenceRatio,
        HeightOfTaper,

        // Power Exponent
        MulOfSamePowerExponent = 0x9001
    }
}
