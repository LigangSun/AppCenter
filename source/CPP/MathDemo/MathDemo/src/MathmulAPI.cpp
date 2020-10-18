
#include "_mathcalc_algo.h"
//#include "..\common\Puzzle3x2API.h"
#include <windows.h>

//1
// Initialization
void MMInit(
	void *g_c
)
{
}

//2 
// Select one combination by random
void MMSelectRandom(
	double *A, // First 
	double *B, // Second
	double *C, // Result of A * B
	int iTLimitA,
	int iTLimitB,
	int iRandom
)
{
	int j = 0;
	time_t t;
	int range_start = 1;
	int range_end = iTLimitA;
	int range_start2 = 1;
	int range_end2 = iTLimitB;
		
	static int lop = 500;
	static int lop_step = 50;

	if ( iTLimitA <= 0 || iTLimitB <= 0 || iTLimitA > 100000 || iTLimitB > 100000)
	{
		return ;
	}

	srand((unsigned) time(&t)); 

	if ( lop > 3000 )
	{
		lop = 500;
	}

	//while ( 1 )
	{
		for ( j=0; j<lop; j++ )
		{
			*A = rand() % (range_end - range_start + 1) + range_start;
		}
		lop += lop_step;

		for ( j=0; j<lop; j++ )
		{
			*B = rand() % (range_end2 - range_start2 + 1) + range_start2;
		}
		lop += lop_step;

		*C = *A * (*B);
	}
}

//4
// Release
void MMDestroy()
{
}