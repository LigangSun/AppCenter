//
// Created

#include "_mathcalc_algo.h"

// A, B, Result <= iNumber
int mc_exhaust_add(int iNumber, int *pDstDataA, int *pDstDataB, int *pDstDataABResult)
{
	int i=0, j=0, m=0, n=0;
	int ret = 0;

	if ( iNumber < 1 )
	{
		return ret;
	}
	
	// Exhaust add
	for( i=1; i<iNumber; i++ )
	{
		for ( j=1; j<iNumber; j++ )
		{
			int a = i;
			int b = j;
			int result = a + b;

			if ( result <= iNumber )
			{
				pDstDataA[m] = a;
				pDstDataB[m] = b;
				pDstDataABResult[m] = result;
				m++;
			}
		}
	}
	
	return m;
}

// A, B, Result <= iNumber
int mc_exhaust_sub(int iNumber, int *pDstDataA, int *pDstDataB, int *pDstDataABResult)
{
	int i=0, j=0, m=0, n=0;
	int ret = -1;

	if ( iNumber < 1 )
	{
		return ret;
	}
	
	// Exhaust sub
	for( i=1; i<iNumber; i++ )
	{
		for ( j=1; j<iNumber; j++ )
		{
			int a = i;
			int b = j;
			int result = a - b;

			if ( result > 0 )
			{
				pDstDataA[m] = a;
				pDstDataB[m] = b;
				pDstDataABResult[m] = result;
				m++;
			}
		}
	}
	
	return m;
}