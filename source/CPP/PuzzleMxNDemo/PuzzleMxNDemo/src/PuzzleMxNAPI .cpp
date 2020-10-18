
#include "_puzzle_mxn_algo.h"

//#define DEBUG

#ifdef DEBUG
#include <stdlib.h>
#include <stdio.h>
#endif

CPuzzleMxN g_Cpuzzle;

//1 
void PGInit( void *g_c
)
{
	g_c = &g_Cpuzzle;
}

//2
// Return -1 is no answer, n is answer and total move paths number
int PGGetAnswer(
	unsigned char pSrcData[][MAX_PUZZLE_SIZE_W],
	unsigned char pDstData[][MAX_PUZZLE_SIZE_W],
	int iPuzzleSizeW,
	int iPuzzleSizeH
)
{
#ifdef DEBUG
	FILE *pfdebug = NULL;
	if ( !(pfdebug = fopen("moves.log", "wb")) )
	{
		return -2;
	}
#endif
	int num_movepaths = -1;
	bool flag = false;
	int M = iPuzzleSizeW;
	int N = iPuzzleSizeH;

	CPuzzleMxN *p_Cpuzzle = &g_Cpuzzle;

	if( p_Cpuzzle == NULL )
	{
		return num_movepaths;
	}

	if ( M > MAX_PUZZLE_SIZE_W || N > MAX_PUZZLE_SIZE_H )
	{
		return -1;
	}

	// Reset
	p_Cpuzzle->FreeTree(p_Cpuzzle->m_pPlaceList);

	//unsigned char *array = p_Cpuzzle->m_iPuzzle;
	int i, j;
	//const int MAXSIZE = 3628800;
	unsigned char temparray[MAX_PUZZLE_SIZE_W][MAX_PUZZLE_SIZE_H];
	
	for ( i = 0; i < MAX_PUZZLE_SIZE_H; i++ )
	{
		memset(temparray[i], 0xff, sizeof(unsigned char) * MAX_PUZZLE_SIZE_W);
	}

	//long target , fountain , parent , 
	//unsigned char *parent[MAX_PUZZLE_SIZE_H];
	long parentID = 0 , child = 1;
	int sum_total[2];
	int sum_MxN;

	//if (p_Cpuzzle->m_bAutoRun) return;

	// Check place 
	sum_total[0] = 0;
	sum_total[1] = 0;
	sum_MxN = 0;
	int number = 0;
	for( i = 0; i < N; i++ )
	{
		for ( j = 0; j < M; j++ )
		{
			sum_total[0] += pSrcData[i][j];
			sum_total[1] += pDstData[i][j];
			sum_MxN = number++;
		}
	}

	if( sum_total[0] != sum_MxN )
	{
		// Initial place error!
		return num_movepaths;
	}

	if( sum_total[1] != sum_MxN )
	{
		// Target place error!
		return num_movepaths;
	}

	for ( i = 0; i < N; i++ )
	{
		memcpy(p_Cpuzzle->m_iPuzzle[i], pSrcData[i], sizeof(unsigned char) * M);
		memcpy(p_Cpuzzle->m_iTargetPuzzle[i], pDstData[i], sizeof(unsigned char) * M);
	}

	p_Cpuzzle->m_bAutoRun = true;

	//p_Cpuzzle->Compress(p_Cpuzzle->m_iTargetPuzzle , target);
	//p_Cpuzzle->Compress(array , fountain);

	if ( p_Cpuzzle->IsSame(p_Cpuzzle->m_iPuzzle, p_Cpuzzle->m_iTargetPuzzle, M, N) )
	{
		num_movepaths = 0;
		return num_movepaths;
	}

	p_Cpuzzle->AddTree(p_Cpuzzle->m_iPuzzle ,p_Cpuzzle->m_pPlaceList, M, N);
	//p_Cpuzzle->m_pScanbuf[ 0 ].Place = fountain;
	for ( i = 0; i < N; i++ )
	{
		memcpy(p_Cpuzzle->m_pScanbuf[0].Place[i], p_Cpuzzle->m_iPuzzle[i], sizeof(unsigned char) * M);
	}
	p_Cpuzzle->m_pScanbuf[ 0 ].ScanID = -1;

	while( parentID < MAXSIZE && child < MAXSIZE && parentID < child )
	{
		//parent = p_Cpuzzle->m_pScanbuf[parentID].Place;
		for ( i = 0 ; i < 4 ; i ++)
		{
			//p_Cpuzzle->Decompress(parent, temparray);
			for ( j = 0; j < N; j++ )
			{
				memcpy(temparray[j], p_Cpuzzle->m_pScanbuf[parentID].Place[j], sizeof(unsigned char) * M); 
			}

			flag = p_Cpuzzle->Move(temparray, i, M, N);

			if ( flag )
			{
				//p_Cpuzzle->Compress(temparray , fountain);
				if ( p_Cpuzzle->AddTree(temparray, p_Cpuzzle->m_pPlaceList, M, N) )
				{
#ifdef DEBUG
					fprintf(pfdebug, "parentID=%d, child=%d\n", parentID, child);
					for ( int v = 0; v < N; v++ )
					{
						for ( int u = 0; u < M; u++ )
						{
							fprintf(pfdebug, "%d	", temparray[v][u]);
						}
						fprintf(pfdebug, "\n");
					}
					fflush(pfdebug);
#endif						
					for ( j = 0; j < N; j++ )
					{
						memcpy(p_Cpuzzle->m_pScanbuf[ child ].Place[j], temparray[j], sizeof(unsigned char) * M);
					}
					p_Cpuzzle->m_pScanbuf[ child ].ScanID = parentID;
					//if (fountain == target)
					if ( p_Cpuzzle->IsSame(temparray, p_Cpuzzle->m_iTargetPuzzle, M, N) )
					{
						p_Cpuzzle->GetPath(child, M, N);

						num_movepaths = p_Cpuzzle->m_iPathsize;
#ifdef DEBUG
	fclose(pfdebug);
#endif						return num_movepaths ;
					}
					child++;
				}
			}
		} // for i
		parentID++;
	}

	p_Cpuzzle->m_bAutoRun = false;

#ifdef DEBUG
	fclose(pfdebug);
#endif	

	return num_movepaths;
}

//3 
void PGGetNextMovePath(
	int iPuzzleSizeW,
	int iPuzzleSizeH,
	int cur_path,
	unsigned char pDstData[][MAX_PUZZLE_SIZE_W] //Return destination data
)
{
	//unsigned char array[MATRIX_SIZE];
	//int k;
	//static int n = 0;
	CPuzzleMxN *p_Cpuzzle = &g_Cpuzzle;

	//static int i = cur_path;

	//if ( IsAnswer == TRUE )
	//{
	//	if ( i > 0 )
	//	{
			//array = g_antivirusalgo.m_pPathList[i].Path;
			//p_Cpuzzle->Decompress(p_Cpuzzle->m_pPathList[cur_path].Path, pDstData);
	for ( int i=0; i<iPuzzleSizeH; i++ )
	{
		memcpy(pDstData[i], p_Cpuzzle->m_pPathList[cur_path].Path[i], sizeof(unsigned char) * iPuzzleSizeW);
	}
	//	}
	//}

	//if ( i == 0 )
	//{
	//	MessageBox(L"Done!", L"Step Show Path" , 0);		
	//}
}; 
