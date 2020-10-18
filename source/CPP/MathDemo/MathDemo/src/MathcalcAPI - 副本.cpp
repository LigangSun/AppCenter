
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
	int lop = 0;
	
	static int lop2 = iTLimitNumber;

	if ( iTLimitNumber <= 0 )
	{
		return ;
	}

	srand((unsigned) time(&t)); 

	//if ( iTLimitNumber > 10000 )
	//{
	//	lop = 10000;
	//}
	//else if ( iTLimitNumber < 2000 )
	//{
	//	lop = 1000;
	//}
	//else 
	//{
	//	lop = iTLimitNumber / 2;
	//}

	if ( iTLimitNumber > 1000 )
	{
		lop = 1000;
	}
	else if ( iTLimitNumber < 400 )
	{
		lop = 300;
	}
	else 
	{
		lop = iTLimitNumber / 2;
	}

	// 
	//lop = 1;
	//lop2 = iTLimitNumber;

	if ( lop2 > 10000 )
	{
		lop2 = 500;
	}

	while ( 1 )
	{
		srand((unsigned) time(NULL)); 
		for ( j=0; j<lop2; j++ )
		{
			*A = rand() % (range_end - range_start + 1) + range_start;

			//// Avoid *A == *C
			//if ( *A > (range_end / 2) )
			//{
			//	range_start2 = range_start;
			//	range_end2 = *A - 1;
			//}
			//else
			//{
			//	range_start2 = *A + 1;
			//	range_end2 = range_end;
			//}
			//srand((unsigned) time(NULL)); 
			//*C = rand() % (range_end2 - range_start2 + 1) + range_start2;
		}
		lop2++;

		//srand((unsigned) time(NULL)); 
		//for ( j=0; j<lop; j++ )
		//{
		//	*C = rand() % (range_end2 - range_start2 + 1) + range_start2;
		//}


		//Sleep(1000);

		//srand((unsigned) time(NULL)); 
		for ( j=0; j<lop2; j++ )
		{
			*C = rand() % (range_end2 - range_start2 + 1) + range_start2;
		}
		lop2++;

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