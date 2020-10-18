//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Proprietary & Confidential
// All Rights Reserved,  2009-2011(c)
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef _MATHCALCALGO_INTERFACE_H_
#define _MATHCALCALGO_INTERFACE_H_

// Macro-definition

//For math calculate algorithm interface
//1
// Initialization
void MCInit(
	void *g_c
);

//2
// Select one combination by random
void MCSelectRandom(
	int *A, // First 
	int *B, // Second
	int *C, // Result of A + B or A - B
	int iTLimitNumber,
	int iRandom
);

//3 
// Select one combination according to rule
void MCSelectRule(
	int *A, // First 
	int *B, // Second
	int *C, // Result of A + B or A - B
	int iTLimitNumber,
	int iRule
); 

//4
// Release
void MCDestroy();

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#endif
