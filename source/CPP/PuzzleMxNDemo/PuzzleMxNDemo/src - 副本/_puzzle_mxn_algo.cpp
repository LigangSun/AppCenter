//
// Created

#include "_puzzle_mxn_algo.h"


typedef struct tagPOINT
{
    int  x;
    int  y;
} POINT, *PPOINT;

CPuzzleMxN::CPuzzleMxN()
{
	m_pPathList = NULL;
	m_pPlaceList = NULL;
	m_pScanbuf = NULL;
	m_iPathsize = 0;
	m_iStepCount = 0;

	m_bAutoRun = false;

	const int MAXSIZE = 362880;
	m_pScanbuf = new Scanbuf[MAXSIZE];

	Reset();
}

CPuzzleMxN::~CPuzzleMxN()
{
	if (m_pPathList != NULL)
	{
		delete[] m_pPathList;
	}

	if (m_pScanbuf != NULL)
	{
		delete[] m_pScanbuf;
	}
}

//void CPuzzleMxN::Compress(unsigned char *array , long &data)
//{
//	data = 0;
//	data = (long)((long)array[0] << 15 | (long)array[1] << 12 | 
//					 (long)array[2] << 9  | (long)array[3] << 6  | 
//					 (long)array[4] << 3  | (long)array[5]);
//}
//
//void CPuzzleMxN::Decompress(long data , unsigned char *array)
//{
//	unsigned char chtem;
//	for ( int i = 0 ; i < MATRIX_SIZE ; i ++)
//	{
//		chtem = (unsigned char)(data >> (15 - (i) * 3) & 0x00000007);
//		array[i] = chtem;
//	}
//}

bool CPuzzleMxN::AddTree(unsigned char *place , PlaceList*& parent)
{
	if (parent == NULL)
	{
		parent = new PlaceList();
		parent->Left = parent->Right = NULL;
		memcpy(parent->Place, place, sizeof(unsigned char) * MAX_PUZZLE_SIZE);
		return true;
	}
	if (parent->Place == place)
	{
		return false;
	}
	if (parent->Place > place)
	{
		return AddTree(place , parent->Right);
	}
	return AddTree(place , parent->Left);
}

void CPuzzleMxN::FreeTree(PlaceList*& parent)
{
	if (parent != NULL)
	{
		if ( parent->Left != NULL)
			FreeTree(parent->Left);
		if ( parent->Right != NULL)
			FreeTree(parent->Right);
		delete parent;
		parent = NULL;
	}
}

bool CPuzzleMxN::Move(unsigned char *array , int way, int M, int N)
{
	int zero , chang;
	bool moveok = false;
	
	for ( zero = 0 ; zero < MAX_PUZZLE_SIZE_W * MAX_PUZZLE_SIZE_H ; zero ++)
	{
		if (array[zero] == 0)
			break;
	}

	POINT pnt;
	pnt.x = zero % MAX_PUZZLE_SIZE_W;
	pnt.y = int(zero / MAX_PUZZLE_SIZE_W);

	switch(way)
	{
	case 0 : //down
		if (pnt.y + 1 < N)
		{
			chang = (pnt.y + 1) * MAX_PUZZLE_SIZE_W + pnt.x ;
			array[zero] = array[chang];
			array[chang] = 0;
			moveok = true;
		}
		break;
	case 1 : //up
		if (pnt.y - 1 > -1)
		{
			chang = (pnt.y - 1) * MAX_PUZZLE_SIZE_W + pnt.x ;
			array[zero] = array[chang];
			array[chang] = 0;
			moveok = true;
		}
		break;
	case 2 : //right
		if (pnt.x + 1 < M)
		{
			chang = pnt.y * MAX_PUZZLE_SIZE_W + pnt.x + 1;
			array[zero] = array[chang];
			array[chang] = 0;
			moveok = true;
		}
		break;
	case 3 : //left
		if (pnt.x - 1 > -1)
		{
			chang = pnt.y * MAX_PUZZLE_SIZE_W + pnt.x - 1;
			array[zero] = array[chang];
			array[chang] = 0;
			moveok = true;
		}
		break;
	}

	if (moveok && !m_bAutoRun)
	{
		m_iStepCount ++ ;
		
		//long temp1 ,temp2;
		//Compress(array , temp1);
		//Compress(m_iTargetPuzzle , temp2);
		//if (temp1 == temp2)
		//{
		//	//MessageBox(NULL , "你真聪明这么快就搞定了!" , "^_^" , 0);		
		//}
	}

	return moveok;
}

//bool CPuzzleMxN::EstimateUncoil(unsigned char *array)
//{
//	int sun = 0;
//	for ( int i = 0 ; i < 5 ; i ++)
//	{
//		for ( int j = 0 ; j < MATRIX_SIZE ; j ++)
//		{
//			if (array[j] != 0)
//			{
//				if (array[j] == i +1 )
//					break;
//				if (array[j] < i + 1)
//					sun++;
//			}
//		}
//	}
//	if (sun % 2 == 0)
//		return true;
//	else
//		return false;
//}

void CPuzzleMxN::GetPath(unsigned int depth)
{
	int now = 0 , maxpos = 100;
	unsigned int parentid;

	if (m_pPathList != NULL)
	{
		delete[] m_pPathList;
	}

	m_pPathList = new PathList[maxpos];
	parentid = m_pScanbuf[depth].ScanID;
	
	//Decompress(m_pScanbuf[depth].Place , m_pPathList[++now].Path);
	memcpy(m_pPathList[++now].Path, m_pScanbuf[depth].Place, sizeof(unsigned char) * MAX_PUZZLE_SIZE);
	
	while(parentid != -1)
	{
		if (now == maxpos)
		{
			maxpos += 10;
			PathList * temlist = new PathList[maxpos];
			memcpy(temlist , m_pPathList , sizeof(PathList) * (maxpos - 10));
			delete[] m_pPathList;
			m_pPathList = temlist;
		}
		//Decompress(m_pScanbuf[parentid].Place , m_pPathList[++now].Path);
    	memcpy(m_pPathList[++now].Path, m_pScanbuf[parentid].Place, sizeof(unsigned char) * MAX_PUZZLE_SIZE);
		parentid = m_pScanbuf[parentid].ScanID;
	}

	m_iPathsize = now;
}

void CPuzzleMxN::Reset()
{
	//if(m_bAutoRun) return;
	//vector<int> vs;
	//int i;
	//for (i = 1 ; i < MATRIX_SIZE ; i ++)
	//	vs.push_back(i);
	//vs.push_back(0);
	//random_shuffle(vs.begin(), vs.end()); 
	//random_shuffle(vs.begin(), vs.end()); 
	//for ( i = 0 ; i < MATRIX_SIZE ; i ++)
	//{
	//	m_iPuzzle[i] = vs[i];
	//}
	//
	//if (!EstimateUncoil(m_iPuzzle))
	//{
	//	unsigned char array[MATRIX_SIZE] = {1,2,3,8,0,4};
	//	memcpy(m_iTargetPuzzle , array , MATRIX_SIZE);
	//}
	//else
	//{
	//	unsigned char array[MATRIX_SIZE] = {1,2,3,4,5,6,7,8,0};
	//	memcpy(m_iTargetPuzzle , array , MATRIX_SIZE);
	//}

	//m_iStepCount = 0;
}

