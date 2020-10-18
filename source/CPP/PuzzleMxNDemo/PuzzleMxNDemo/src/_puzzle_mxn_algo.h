//
// Created
#ifndef _PUZZLEMXNALGO_H_
#define _PUZZLEMXNALGO_H_

#include <time.h>
#include <vector>
#include <algorithm>
#include <string.h>

#include "..\common\PuzzleMxNAPI.h"

#define MAXSIZE			(3628800*3) //(362880)

//using namespace std;

class CPuzzleMxN
{
public:
	struct PlaceList
    {
		unsigned char Place[MAX_PUZZLE_SIZE_W][MAX_PUZZLE_SIZE_H];
		PlaceList*	  Left;
		PlaceList*	  Right;
    };

	struct Scanbuf
	{
		unsigned char Place[MAX_PUZZLE_SIZE_W][MAX_PUZZLE_SIZE_H];
		int ScanID;
	};

	struct PathList
	{
		unsigned char Path[MAX_PUZZLE_SIZE_W][MAX_PUZZLE_SIZE_H];
	};

//private:
	PlaceList *m_pPlaceList;
	Scanbuf *m_pScanbuf;

public:
	int m_iPathsize;
	unsigned int m_iStepCount;
	unsigned char m_iTargetPuzzle[MAX_PUZZLE_SIZE_W][MAX_PUZZLE_SIZE_H];
	unsigned char m_iPuzzle[MAX_PUZZLE_SIZE_W][MAX_PUZZLE_SIZE_H];
	PathList *m_pPathList;
	bool m_bAutoRun;

//private:
	bool AddTree(unsigned char place[][MAX_PUZZLE_SIZE_H] , PlaceList*& parent, int M, int N);
	void FreeTree(PlaceList*& parent);
	//void Compress(unsigned char *array , long & data);
	//void Decompress(long data , unsigned char *array);
	bool Move(unsigned char array[][MAX_PUZZLE_SIZE_H] , int way, int M, int N);
	//bool EstimateUncoil(unsigned char *array);
	void GetPath(unsigned int depth, int M, int N);
//	bool IsSame(unsigned char *pSrc[MAX_PUZZLE_SIZE_H], unsigned char *pDst[MAX_PUZZLE_SIZE_H], int M, int N);
//	bool IsSame(unsigned char **pSrc, unsigned char **pDst, int M, int N);
	bool IsSame(unsigned char pSrc[][MAX_PUZZLE_SIZE_W], unsigned char pDst[][MAX_PUZZLE_SIZE_W], int M, int N);
	
public:
	void Reset();
	
public:
	CPuzzleMxN();
	~CPuzzleMxN();
};

#endif