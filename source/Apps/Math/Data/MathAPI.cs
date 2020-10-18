using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SoonLearning.Math.Data
{
    public class MathAPI
    {
        [DllImport("MathCalcDll.dll")]
        public extern static void MCInit(IntPtr g_c);

        [DllImport("MathCalcDll.dll")]
        public extern static void MCSelectRandom(
            ref int A, // First 
            ref int B, // Second
            ref int C, // Result of A + B or A - B
            int iTLimitNumber,
            int iRandom);

        [DllImport("MathCalcDll.dll")]
        public extern static void MCSelectRule(
            ref int A, // First 
            ref int B, // Second
            ref int C, // Result of A + B or A - B
            int iTLimitNumber,
            int iRule);

        //4
        // Release
        [DllImport("MathCalcDll.dll")]
        public extern static void MCDestroy();

        [DllImport("MathMul.dll")]
        public extern static void MMInit(IntPtr g_c);

        [DllImport("MathMul.dll")]
        public extern static void MMSelectRandom(ref double A, // First 
            ref double B, // Second
            ref double C, // Result of A * B
            int iTLimitA,
            int iTLimitB,
            int iRandom);

        [DllImport("MathMul.dll")]
        public extern static void MMDestroy();
    }
}
