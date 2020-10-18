
#include "_mathcalc_algo.h"
//#include "..\common\Puzzle3x2API.h"
#include <windows.h>

//1
// Initialization
void MCInit(
	void *g_c
)
{
}

//2 
// Select one combination by random
void MCSelectRandom(
	int *A, // First 
	int *B, // Second
	int *C, // Result of A + B or A - B
	int iTLimitNumber,
	int iRandom
)
{
	int j = 0;
	time_t t;
	int range_start = 1;
	int range_end = iTLimitNumber;
	int range_start2 = 1;
	int range_end2 = iTLimitNumber;
		
	static int lop = 500;
	static int lop_step = 50;

	if ( iTLimitNumber <= 0 )
	{
		return ;
	}

	srand((unsigned) time(&t)); 

	if ( lop > 3000 )
	{
		lop = 500;
	}

	while ( 1 )
	{
		for ( j=0; j<lop; j++ )
		{
			*A = rand() % (range_end - range_start + 1) + range_start;
		}
		lop += lop_step;

		for ( j=0; j<lop; j++ )
		{
			*C = rand() % (range_end2 - range_start2 + 1) + range_start2;
		}
		lop += lop_step;

		if ( *A < *C ) //Add is selected
		{
			*B =  *C - *A;
			break;
		}
		else if ( *A > *C ) //Sub is selected
		{
			*B =  *A - *C;	
			break;
		}
	}
}

//3
// Select one combination according to rule
void MCSelectRule(
	int *A, // First 
	int *B, // Second
	int *C, // Result of A + B or A - B
	int iTLimitNumber,
	int iRule
)
{
}

//4
// Release
void MCDestroy()
{
}