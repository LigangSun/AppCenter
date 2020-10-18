//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Proprietary & Confidential
// All Rights Reserved, 2011(c)
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef _PUZZLEMXNALGO_INTERFACE_H_
#define _PUZZLEMXNALGO_INTERFACE_H_

// Macro-definition
#define MAX_PUZZLE_SIZE_W			(8)
#define MAX_PUZZLE_SIZE_H			(8)
#define MAX_PUZZLE_SIZE				(MAX_PUZZLE_SIZE_W * MAX_PUZZLE_SIZE_H)

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//These functionS must implementate even if it is empty.
//The nEnableAlg parameter is pass by framework and could config from config.ini file,
//  if nEnableAlg =0,each algorithm do nothing but copy input buffer data to output buffer.
//  if nEnableAlg =1,each algorithm do its processing routine.
//  if nEnableAlg >1,each algorithm define itself behavior.
  
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Each algorithm Lib module's header interface must conform with below convention:
//These three function must implementate even if it is empty.
//The nEnableAlg parameter is pass by framework and could config from config.ini file,
//  if nEnableAlg =0,each algorithm do nothing but copy input buffer data to output buffer.
//  if nEnableAlg =1,each algorithm do its processing routine.
//  if nEnableAlg >1,each algorithm define itself behavior.
  
//For puzzle3x2 algorithm interface
//1
// Initialization
void PGInit(
	void *g_c
);

//2 
// Return -1 is no answer, n is answer and total move paths number
int PGGetAnswer(
	unsigned char pSrcData[][MAX_PUZZLE_SIZE_W],
	unsigned char pDstData[][MAX_PUZZLE_SIZE_W],
	int iPuzzleSizeW,
	int iPuzzleSizeH
); 

//3 
// Show next step move path
void PGGetNextMovePath(
	int iPuzzleSizeW,
	int iPuzzleSizeH,
	int cur_path,
	unsigned char pDstData[][MAX_PUZZLE_SIZE_W] //Return destination data
); 


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#endif

