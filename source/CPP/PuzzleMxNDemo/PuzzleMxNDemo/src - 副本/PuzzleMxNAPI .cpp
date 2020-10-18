
#include "_puzzle_mxn_algo.h"

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
	unsigned char *pSrcData,
	unsigned char *pDstData,
	int iPuzzleSizeW,
	int iPuzzleSizeH
)
{
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

	unsigned char *array = p_Cpuzzle->m_iPuzzle;
	unsigned int i, j;
	const int MAXSIZE = 362880;
	unsigned char temparray[MAX_PUZZLE_SIZE];

	long target , fountain , parent , parentID = 0 , child = 1;
	int sum_total[2];
	int sum_target;

	for ( i=0; i<

	//if (p_Cpuzzle->m_bAutoRun) return;

	// Check place 
	sum_total[0] = 0;
	sum_total[1] = 0;
	for( i = 0; i < MATRIX_SIZE; i++ )
	{
		sum_total[0] += pSrcData[i];
		sum_total[1] += pDstData[i];
	}

	if( sum_total[0] != sum_total[1] )
	{
		// Initial place error!
		return num_movepaths;
	}

	if( sum_total[1] != T_PUZZLE3X2_CHECK )
	{
		// Target place error!
		return num_movepaths;
	}

	memcpy(p_Cpuzzle->m_iPuzzle, pSrcData, sizeof(unsigned char) * MATRIX_SIZE);
	memcpy(p_Cpuzzle->m_iTargetPuzzle, pDstData, sizeof(unsigned char) * MATRIX_SIZE);

	p_Cpuzzle->m_bAutoRun = true;

	p_Cpuzzle->Compress(p_Cpuzzle->m_iTargetPuzzle , target);
	p_Cpuzzle->Compress(array , fountain);

	if (fountain == target)
	{
		num_movepaths = 0;
		return num_movepaths;
	}

	p_Cpuzzle->AddTree(fountain ,p_Cpuzzle->m_pPlaceList);
	p_Cpuzzle->m_pScanbuf[ 0 ].Place = fountain;
	p_Cpuzzle->m_pScanbuf[ 0 ].ScanID = -1;

	while( parentID < MAXSIZE && child < MAXSIZE && parentID < child )
	{
		parent = p_Cpuzzle->m_pScanbuf[parentID].Place;
		for ( i = 0 ; i < 4 ; i ++)
		{
			p_Cpuzzle->Decompress(parent, temparray);
			flag = p_Cpuzzle->Move(temparray,i);

			if ( flag )
			{
				p_Cpuzzle->Compress(temparray , fountain);
				if (p_Cpuzzle->AddTree(fountain, p_Cpuzzle->m_pPlaceList))
				{
					p_Cpuzzle->m_pScanbuf[ child ].Place = fountain;
					p_Cpuzzle->m_pScanbuf[ child ].ScanID = parentID;
					if (fountain == target)
					{
						p_Cpuzzle->GetPath(child);

						num_movepaths = p_Cpuzzle->m_iPathsize;
						return num_movepaths ;
					}
					child++;
				}
			}
		} // for i
		parentID++;
	}

	p_Cpuzzle->m_bAutoRun = false;

	return num_movepaths;
}

//3 
void PGGetNextMovePath(
	int cur_path,
	unsigned char *pDstData //Return destination data
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
			memcpy(pDstData, p_Cpuzzle->m_pPathList[cur_path].Path, MAX_PUZZLE_SIZE);
	//	}
	//}

	//if ( i == 0 )
	//{
	//	MessageBox(L"Done!", L"Step Show Path" , 0);		
	//}
}; 

//4
void PGGetPrevMovePath(
	int cur_path,
	unsigned char *pDstData //Return destination data
)
{
}

//5
void PGGetAllMovePaths(
	unsigned char *pDstData //Return destination data
)
{
	//unsigned char array[MATRIX_NUM];
	//CString csText;
	//int i, k;

	//if ( IsAnswer == TRUE )
	//{
	//	g_antivirusalgo.m_iStepCount = 0;

	//	for ( i = g_antivirusalgo.m_iPathsize; i > 0 ; i-- )
	//	//for ( i = 0; i < g_antivirusalgo.m_iPathsize; i++ )
	//	{
	//		g_antivirusalgo.Decompress(g_antivirusalgo.m_pPathList[i].Path, m_nData_NA, g_antivirusalgo.m_pCell, array);
	//	
	//		for ( k = 0; k < MATRIX_WIDTH*MATRIX_WIDTH; k++ )
	//		{
	//			csText.Format(L"%d", array[k]);
	//			m_btnData[k]->SetWindowText(csText);
	//		}
	//		csText.Format(L"%d", array[52]);
	//		m_btnData[49]->SetWindowText(csText);

	//		// Display the step counts
	//		csText.Format(L"%d", g_antivirusalgo.m_iStepCount++);
	//		m_btnData2[0]->SetWindowText(csText);

	//		Sleep(SLEEP_NUM);
	//	}
	//}

	//MessageBox(L"Done!", L"Show Path" , 0);		
};