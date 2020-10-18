
// PuzzleMxNDemo.h : main header file for the PROJECT_NAME application
//

#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"		// main symbols


// CPuzzleMxNDemoApp:
// See PuzzleMxNDemo.cpp for the implementation of this class
//

class CPuzzleMxNDemoApp : public CWinApp
{
public:
	CPuzzleMxNDemoApp();

// Overrides
public:
	virtual BOOL InitInstance();

// Implementation

	DECLARE_MESSAGE_MAP()
};

extern CPuzzleMxNDemoApp theApp;