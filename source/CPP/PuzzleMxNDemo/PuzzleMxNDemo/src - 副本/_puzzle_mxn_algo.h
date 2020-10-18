//
// Created
#ifndef _PUZZLEMXNALGO_H_
#define _PUZZLEMXNALGO_H_

#include <time.h>
#include <vector>
#include <algorithm>
#include <string.h>

#include "..\common\PuzzleMxNAPI.h"

//using namespace std;

class CPuzzleMxN
{
public:
	struct PlaceList
    {
		unsigned char Place[MAX_PUZZLE_SIZE];
		PlaceList*	  Left;
		PlaceList*	  Right;
    };

	struct Scanbuf
	{
		unsigned char Place[MAX_PUZZLE_SIZE];
		int ScanID;
	};

	struct PathList
	{
		unsigned char Path[MAX_PUZZLE_SIZE];
	};

//private:
	PlaceList *m_pPlaceList;
	Scanbuf *m_pScanbuf;

public:
	int m_iPathsize;
	unsigned int m_iStepCount;
	unsigned char m_iTargetPuzzle[MAX_PUZZLE_SIZE];
	unsigned char m_iPuzzle[MAX_PUZZLE_SIZE];
	PathList *m_pPathList;
	bool m_bAutoRun;

//private:
	bool AddTree(unsigned char *place , PlaceList*& parent);
	void FreeTree(PlaceList*& parent);
	//void Compress(unsigned char *array , long & data);
	//void Decompress(long data , unsigned char *array);
	bool Move(unsigned char *array , int way, int M, int N);
	//bool EstimateUncoil(unsigned char *array);
	void GetPath(unsigned int depth);
	
public:
	void Reset();
	
public:
	CPuzzleMxN();
	~CPuzzleMxN();
};

#endif