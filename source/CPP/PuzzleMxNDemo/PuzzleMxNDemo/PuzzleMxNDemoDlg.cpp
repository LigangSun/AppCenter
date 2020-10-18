
// PuzzleMxNDemoDlg.cpp : implementation file
//

#include "stdafx.h"
#include "PuzzleMxNDemo.h"
#include "PuzzleMxNDemoDlg.h"
#include "afxdialogex.h"

#include <math.h>

#define SLEEP_NUM			400 //300

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CAboutDlg dialog used for App About

class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();

// Dialog Data
	enum { IDD = IDD_ABOUTBOX };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

// Implementation
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialogEx(CAboutDlg::IDD)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialogEx)
END_MESSAGE_MAP()


// CPuzzleMxNDemoDlg dialog




CPuzzleMxNDemoDlg::CPuzzleMxNDemoDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CPuzzleMxNDemoDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CPuzzleMxNDemoDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);

	// Added
	DDX_Control(pDX, IDC_BUTTON1, m_btn[0]);
	DDX_Control(pDX, IDC_BUTTON2, m_btn[1]);
	DDX_Control(pDX, IDC_BUTTON3, m_btn[2]);
	DDX_Control(pDX, IDC_BUTTON4, m_btn[3]);
	DDX_Control(pDX, IDC_BUTTON5, m_btn[4]);
	DDX_Control(pDX, IDC_BUTTON6, m_btn[5]);
	DDX_Control(pDX, IDC_BUTTON7, m_btn[6]);
	DDX_Control(pDX, IDC_BUTTON8, m_btn[7]);
	DDX_Control(pDX, IDC_BUTTON9, m_btn[8]);
	DDX_Control(pDX, IDC_BUTTON10,m_btn[9]);
	DDX_Control(pDX, IDC_BUTTON11,m_btn[10]);
	DDX_Control(pDX, IDC_BUTTON12,m_btn[11]);
	DDX_Control(pDX, IDC_BUTTON13,m_btn[12]);
	DDX_Control(pDX, IDC_BUTTON14,m_btn[13]);
	DDX_Control(pDX, IDC_BUTTON15,m_btn[14]);
	DDX_Control(pDX, IDC_BUTTON16,m_btn[15]);
	DDX_Control(pDX, IDC_BUTTON17,m_btn[16]);
	DDX_Control(pDX, IDC_BUTTON18,m_btn[17]);
	DDX_Control(pDX, IDC_BUTTON19,m_btn[18]);
	DDX_Control(pDX, IDC_BUTTON20,m_btn[19]);
	DDX_Control(pDX, IDC_BUTTON21,m_btn[20]);
	DDX_Control(pDX, IDC_BUTTON22,m_btn[21]);
	DDX_Control(pDX, IDC_BUTTON23,m_btn[22]);
	DDX_Control(pDX, IDC_BUTTON24,m_btn[23]);
	DDX_Control(pDX, IDC_BUTTON25,m_btn[24]);
	DDX_Control(pDX, IDC_BUTTON26,m_btn[25]);
	DDX_Control(pDX, IDC_BUTTON27,m_btn[26]);
	DDX_Control(pDX, IDC_BUTTON28,m_btn[27]);
	DDX_Control(pDX, IDC_BUTTON29,m_btn[28]);
	DDX_Control(pDX, IDC_BUTTON30,m_btn[29]);
	DDX_Control(pDX, IDC_BUTTON31,m_btn[30]);
	DDX_Control(pDX, IDC_BUTTON32,m_btn[31]);
	DDX_Control(pDX, IDC_BUTTON33,m_btn[32]);
	DDX_Control(pDX, IDC_BUTTON34,m_btn[33]);
	DDX_Control(pDX, IDC_BUTTON35,m_btn[34]);
	DDX_Control(pDX, IDC_BUTTON36,m_btn[35]);
	DDX_Control(pDX, IDC_BUTTON37,m_btn[36]);
	DDX_Control(pDX, IDC_BUTTON38,m_btn[37]);
	DDX_Control(pDX, IDC_BUTTON39,m_btn[38]);
	DDX_Control(pDX, IDC_BUTTON40,m_btn[39]);
	DDX_Control(pDX, IDC_BUTTON41,m_btn[40]);
	DDX_Control(pDX, IDC_BUTTON42,m_btn[41]);
	DDX_Control(pDX, IDC_BUTTON43,m_btn[42]);
	DDX_Control(pDX, IDC_BUTTON44,m_btn[43]);
	DDX_Control(pDX, IDC_BUTTON45,m_btn[44]);
	DDX_Control(pDX, IDC_BUTTON46,m_btn[45]);
	DDX_Control(pDX, IDC_BUTTON47,m_btn[46]);
	DDX_Control(pDX, IDC_BUTTON48,m_btn[47]);
	DDX_Control(pDX, IDC_BUTTON49,m_btn[48]);
	DDX_Control(pDX, IDC_BUTTON50,m_btn[49]);
	DDX_Control(pDX, IDC_BUTTON51,m_btn[50]);
	DDX_Control(pDX, IDC_BUTTON52,m_btn[51]);
	DDX_Control(pDX, IDC_BUTTON53,m_btn[52]);
	DDX_Control(pDX, IDC_BUTTON54,m_btn[53]);
	DDX_Control(pDX, IDC_BUTTON55,m_btn[54]);
	DDX_Control(pDX, IDC_BUTTON56,m_btn[55]);
	DDX_Control(pDX, IDC_BUTTON57,m_btn[56]);
	DDX_Control(pDX, IDC_BUTTON58,m_btn[57]);
	DDX_Control(pDX, IDC_BUTTON59,m_btn[58]);
	DDX_Control(pDX, IDC_BUTTON60,m_btn[59]);
	DDX_Control(pDX, IDC_BUTTON61,m_btn[60]);
	DDX_Control(pDX, IDC_BUTTON62,m_btn[61]);
	DDX_Control(pDX, IDC_BUTTON63,m_btn[62]);
	DDX_Control(pDX, IDC_BUTTON64,m_btn[63]);

	DDX_Control(pDX, IDC_BUTTON66,m_btn_count);
	//							 
}
								 
BEGIN_MESSAGE_MAP(CPuzzleMxNDemoDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_EN_CHANGE(IDC_EDIT1, &CPuzzleMxNDemoDlg::OnEnChangeEdit1)
	ON_BN_CLICKED(IDC_BUTTON1, &CPuzzleMxNDemoDlg::OnBnClickedButton1)
	ON_BN_CLICKED(IDC_BUTTON2, &CPuzzleMxNDemoDlg::OnBnClickedButton2)
	ON_BN_CLICKED(IDC_BUTTON3, &CPuzzleMxNDemoDlg::OnBnClickedButton3)
	ON_BN_CLICKED(IDC_BUTTON4, &CPuzzleMxNDemoDlg::OnBnClickedButton4)
	ON_BN_CLICKED(IDC_BUTTON5, &CPuzzleMxNDemoDlg::OnBnClickedButton5)
	ON_BN_CLICKED(IDC_BUTTON6, &CPuzzleMxNDemoDlg::OnBnClickedButton6)
	ON_BN_CLICKED(IDC_BUTTON7, &CPuzzleMxNDemoDlg::OnBnClickedButton7)
	ON_BN_CLICKED(IDC_BUTTON8, &CPuzzleMxNDemoDlg::OnBnClickedButton8)
	ON_BN_CLICKED(IDC_BUTTON9, &CPuzzleMxNDemoDlg::OnBnClickedButton9)
	ON_BN_CLICKED(IDC_BUTTON10, &CPuzzleMxNDemoDlg::OnBnClickedButton10)
	ON_BN_CLICKED(IDC_BUTTON11, &CPuzzleMxNDemoDlg::OnBnClickedButton11)
	ON_BN_CLICKED(IDC_BUTTON12, &CPuzzleMxNDemoDlg::OnBnClickedButton12)
	ON_BN_CLICKED(IDC_BUTTON13, &CPuzzleMxNDemoDlg::OnBnClickedButton13)
	ON_BN_CLICKED(IDC_BUTTON14, &CPuzzleMxNDemoDlg::OnBnClickedButton14)
	ON_BN_CLICKED(IDC_BUTTON15, &CPuzzleMxNDemoDlg::OnBnClickedButton15)
	ON_BN_CLICKED(IDC_BUTTON16, &CPuzzleMxNDemoDlg::OnBnClickedButton16)
	ON_BN_CLICKED(IDC_BUTTON17, &CPuzzleMxNDemoDlg::OnBnClickedButton17)
	ON_BN_CLICKED(IDC_BUTTON18, &CPuzzleMxNDemoDlg::OnBnClickedButton18)
	ON_BN_CLICKED(IDC_BUTTON19, &CPuzzleMxNDemoDlg::OnBnClickedButton19)
	ON_BN_CLICKED(IDC_BUTTON20, &CPuzzleMxNDemoDlg::OnBnClickedButton20)
	ON_BN_CLICKED(IDC_BUTTON21, &CPuzzleMxNDemoDlg::OnBnClickedButton21)
	ON_BN_CLICKED(IDC_BUTTON22, &CPuzzleMxNDemoDlg::OnBnClickedButton22)
	ON_BN_CLICKED(IDC_BUTTON23, &CPuzzleMxNDemoDlg::OnBnClickedButton23)
	ON_BN_CLICKED(IDC_BUTTON24, &CPuzzleMxNDemoDlg::OnBnClickedButton24)
	ON_BN_CLICKED(IDC_BUTTON25, &CPuzzleMxNDemoDlg::OnBnClickedButton25)
	ON_BN_CLICKED(IDC_BUTTON26, &CPuzzleMxNDemoDlg::OnBnClickedButton26)
	ON_BN_CLICKED(IDC_BUTTON27, &CPuzzleMxNDemoDlg::OnBnClickedButton27)
	ON_BN_CLICKED(IDC_BUTTON28, &CPuzzleMxNDemoDlg::OnBnClickedButton28)
	ON_BN_CLICKED(IDC_BUTTON29, &CPuzzleMxNDemoDlg::OnBnClickedButton29)
	ON_BN_CLICKED(IDC_BUTTON30, &CPuzzleMxNDemoDlg::OnBnClickedButton30)
	ON_BN_CLICKED(IDC_BUTTON31, &CPuzzleMxNDemoDlg::OnBnClickedButton31)
	ON_BN_CLICKED(IDC_BUTTON32, &CPuzzleMxNDemoDlg::OnBnClickedButton32)
	ON_BN_CLICKED(IDC_BUTTON33, &CPuzzleMxNDemoDlg::OnBnClickedButton33)
	ON_BN_CLICKED(IDC_BUTTON34, &CPuzzleMxNDemoDlg::OnBnClickedButton34)
	ON_BN_CLICKED(IDC_BUTTON35, &CPuzzleMxNDemoDlg::OnBnClickedButton35)
	ON_BN_CLICKED(IDC_BUTTON36, &CPuzzleMxNDemoDlg::OnBnClickedButton36)
	ON_BN_CLICKED(IDC_BUTTON37, &CPuzzleMxNDemoDlg::OnBnClickedButton37)
	ON_BN_CLICKED(IDC_BUTTON38, &CPuzzleMxNDemoDlg::OnBnClickedButton38)
	ON_BN_CLICKED(IDC_BUTTON39, &CPuzzleMxNDemoDlg::OnBnClickedButton39)
	ON_BN_CLICKED(IDC_BUTTON40, &CPuzzleMxNDemoDlg::OnBnClickedButton40)
	ON_BN_CLICKED(IDC_BUTTON41, &CPuzzleMxNDemoDlg::OnBnClickedButton41)
	ON_BN_CLICKED(IDC_BUTTON42, &CPuzzleMxNDemoDlg::OnBnClickedButton42)
	ON_BN_CLICKED(IDC_BUTTON43, &CPuzzleMxNDemoDlg::OnBnClickedButton43)
	ON_BN_CLICKED(IDC_BUTTON44, &CPuzzleMxNDemoDlg::OnBnClickedButton44)
	ON_BN_CLICKED(IDC_BUTTON45, &CPuzzleMxNDemoDlg::OnBnClickedButton45)
	ON_BN_CLICKED(IDC_BUTTON46, &CPuzzleMxNDemoDlg::OnBnClickedButton46)
	ON_BN_CLICKED(IDC_BUTTON47, &CPuzzleMxNDemoDlg::OnBnClickedButton47)
	ON_BN_CLICKED(IDC_BUTTON48, &CPuzzleMxNDemoDlg::OnBnClickedButton48)
	ON_BN_CLICKED(IDC_BUTTON49, &CPuzzleMxNDemoDlg::OnBnClickedButton49)
	ON_BN_CLICKED(IDC_BUTTON50, &CPuzzleMxNDemoDlg::OnBnClickedButton50)
	ON_BN_CLICKED(IDC_BUTTON51, &CPuzzleMxNDemoDlg::OnBnClickedButton51)
	ON_BN_CLICKED(IDC_BUTTON52, &CPuzzleMxNDemoDlg::OnBnClickedButton52)
	ON_BN_CLICKED(IDC_BUTTON53, &CPuzzleMxNDemoDlg::OnBnClickedButton53)
	ON_BN_CLICKED(IDC_BUTTON54, &CPuzzleMxNDemoDlg::OnBnClickedButton54)
	ON_BN_CLICKED(IDC_BUTTON55, &CPuzzleMxNDemoDlg::OnBnClickedButton55)
	ON_BN_CLICKED(IDC_BUTTON56, &CPuzzleMxNDemoDlg::OnBnClickedButton56)
	ON_BN_CLICKED(IDC_BUTTON57, &CPuzzleMxNDemoDlg::OnBnClickedButton57)
	ON_BN_CLICKED(IDC_BUTTON58, &CPuzzleMxNDemoDlg::OnBnClickedButton58)
	ON_BN_CLICKED(IDC_BUTTON59, &CPuzzleMxNDemoDlg::OnBnClickedButton59)
	ON_BN_CLICKED(IDC_BUTTON60, &CPuzzleMxNDemoDlg::OnBnClickedButton60)
	ON_BN_CLICKED(IDC_BUTTON61, &CPuzzleMxNDemoDlg::OnBnClickedButton61)
	ON_BN_CLICKED(IDC_BUTTON62, &CPuzzleMxNDemoDlg::OnBnClickedButton62)
	ON_BN_CLICKED(IDC_BUTTON63, &CPuzzleMxNDemoDlg::OnBnClickedButton63)
	ON_BN_CLICKED(IDC_BUTTON64, &CPuzzleMxNDemoDlg::OnBnClickedButton64)
	ON_EN_CHANGE(IDC_EDIT2, &CPuzzleMxNDemoDlg::OnEnChangeEdit2)
	ON_BN_CLICKED(IDC_BUTTON65, &CPuzzleMxNDemoDlg::OnBnClickedButton65)
	ON_BN_CLICKED(IDC_BUTTON66, &CPuzzleMxNDemoDlg::OnBnClickedButton66)
	ON_BN_CLICKED(IDC_BUTTON67, &CPuzzleMxNDemoDlg::OnBnClickedButton67)
	ON_BN_CLICKED(IDC_BUTTON68, &CPuzzleMxNDemoDlg::OnBnClickedButton68)
END_MESSAGE_MAP()


// CPuzzleMxNDemoDlg message handlers

BOOL CPuzzleMxNDemoDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// Add "About..." menu item to system menu.

	// IDM_ABOUTBOX must be in the system command range.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		BOOL bNameValid;
		CString strAboutMenu;
		bNameValid = strAboutMenu.LoadString(IDS_ABOUTBOX);
		ASSERT(bNameValid);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	// TODO: Add extra initialization here
	// Added
	int i;
	for ( i = 0; i < 64; i++ )
	{
		m_btnData_src[i] = &m_btn[i];
	}
	
	m_btnData_dst3[0] = &m_btn_count;

	// Initial plane
	for ( i = 0; i < MAX_PUZZLE_SIZE_H; i++ )
	{
		memset(m_nData_src[i], 0xff, sizeof(unsigned char) * MAX_PUZZLE_SIZE_W);
		memset(m_nData_dst[i], 0xff, sizeof(unsigned char) * MAX_PUZZLE_SIZE_W);
	}

	// Initialization
	hInst = NULL;

	// Load DLL
	hInst = LoadLibrary(("PuzzleMxN.dll"));
	if ( !hInst ) 
	{
		printf("Dll failure!!\n");
		return FALSE;
	}

	ptrPGINT = (PPGINT)GetProcAddress(hInst, MAKEINTRESOURCE(1));
	if ( !ptrPGINT ) 
	{
		printf("Function 1 in Dll failure!!\n");
		return FALSE;
	}

	ptrPGANSWER = (PPGANSWER)GetProcAddress(hInst, MAKEINTRESOURCE(2));
	if ( !ptrPGANSWER ) 
	{
		printf("Function 2 in Dll failure!!\n");
		return FALSE;
	}

	ptrPGNEXTMOVE = (PPGNEXTMOVE)GetProcAddress(hInst, MAKEINTRESOURCE(3));
	if ( !ptrPGNEXTMOVE ) 
	{
		printf("Function 3 in Dll failure!!\n");
		return FALSE;
	}

	void *pTmp = NULL;
	ptrPGINT(pTmp);

	IsAnswer = false;
	cur_path = 0;

	return TRUE;  // return TRUE  unless you set the focus to a control
}

void CPuzzleMxNDemoDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialogEx::OnSysCommand(nID, lParam);
	}
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CPuzzleMxNDemoDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CPuzzleMxNDemoDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



void CPuzzleMxNDemoDlg::OnEnChangeEdit1()
{
	// TODO:  If this is a RICHEDIT control, the control will not
	// send this notification unless you override the CDialogEx::OnInitDialog()
	// function and call CRichEditCtrl().SetEventMask()
	// with the ENM_CHANGE flag ORed into the mask.

	// TODO:  Add your control notification handler code here

	CString strTemp;
	CEdit* edit1 = ((CEdit*)(GetDlgItem(IDC_EDIT1)));
	edit1->GetWindowText(strTemp);

	iPuzzleSizeW = atoi(strTemp);
}


void CPuzzleMxNDemoDlg::OnBnClickedButton1()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton2()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton3()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton4()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton5()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton6()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton7()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton8()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton9()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton10()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton11()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton12()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton13()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton14()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton15()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton16()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton17()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton18()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton19()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton20()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton21()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton22()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton23()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton24()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton25()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton26()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton27()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton28()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton29()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton30()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton31()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton32()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton33()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton34()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton35()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton36()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton37()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton38()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton39()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton40()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton41()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton42()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton43()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton44()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton45()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton46()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton47()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton48()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton49()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton50()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton51()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton52()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton53()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton54()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton55()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton56()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton57()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton58()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton59()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton60()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton61()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton62()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton63()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnBnClickedButton64()
{
	// TODO: Add your control notification handler code here
}


void CPuzzleMxNDemoDlg::OnEnChangeEdit2()
{
	// TODO:  If this is a RICHEDIT control, the control will not
	// send this notification unless you override the CDialogEx::OnInitDialog()
	// function and call CRichEditCtrl().SetEventMask()
	// with the ENM_CHANGE flag ORed into the mask.

	// TODO:  Add your control notification handler code here
	CString strTemp;
	CEdit* edit1 = ((CEdit*)(GetDlgItem(IDC_EDIT2)));
	edit1->GetWindowText(strTemp);

	iPuzzleSizeH = atoi(strTemp);

	if ( iPuzzleSizeW > 8 || iPuzzleSizeH > 8 )
	{
		return ;
	}

	// Initial src and target
	int i, j;
	int number = 0;

	for ( i = 0; i < MAX_PUZZLE_SIZE_H; i++ )
	{
		memset(m_nData_src[i], 0xff, sizeof(unsigned char) * MAX_PUZZLE_SIZE_W);
	}

	for ( i = 0; i < iPuzzleSizeH; i++ )
	{
		for ( j = 0; j < iPuzzleSizeW; j++ )
		{
			m_nData_src[i][j] = m_nData_dst[i][j] = number++;
		}
	}

/*
	// Test 8x8
	m_nData_src[7][7] = 61;
	m_nData_src[7][6] = 62;
	m_nData_src[7][5] = 60;
	m_nData_src[7][4] = 63;

	// Test 3x2
	m_nData_src[0][0] = 5;
	m_nData_src[0][1] = 2;
	m_nData_src[0][2] = 0;
	m_nData_src[1][0] = 4;
	m_nData_src[1][1] = 3;
	m_nData_src[1][2] = 1;

	m_nData_dst[0][0] = 0;
	m_nData_dst[0][1] = 1;
	m_nData_dst[0][2] = 2;
	m_nData_dst[1][3] = 3;
	m_nData_dst[1][4] = 4;
	m_nData_dst[1][5] = 5;

	// Test 2x2
	m_nData_src[0][0] = 0;
	m_nData_src[0][1] = 3;
	m_nData_src[1][0] = 2;
	m_nData_src[1][1] = 1;

	m_nData_dst[0][0] = 0;
	m_nData_dst[0][1] = 1;
	m_nData_dst[1][0] = 2;
	m_nData_dst[1][1] = 3;
*/

	CString csText;

	for ( i = 0; i < MAX_PUZZLE_SIZE_H; i++ )
	{
		for ( j = 0; j < MAX_PUZZLE_SIZE_W; j++ )
		{
			csText.Format("%d", m_nData_src[i][j]);
			m_btnData_src[i * MAX_PUZZLE_SIZE_W + j]->SetWindowText(csText);
		}
	}

}


// Get answer
void CPuzzleMxNDemoDlg::OnBnClickedButton65()
{
	// TODO: Add your control notification handler code here
	CString csText;

	total_moves =  ptrPGANSWER(m_nData_src, m_nData_dst, iPuzzleSizeW, iPuzzleSizeH);

	if ( -1 == total_moves ) // No answer
		MessageBox("No Answer!", "GetAnswer...", 0);	
	else 
	{
		IsAnswer = TRUE;
		MessageBox("Yes!", "GetAnswer...", 0);		
		csText.Format("%d", total_moves-1);

		m_btnData_dst3[0]->SetWindowText(csText);

		cur_path = total_moves;
	}
}


void CPuzzleMxNDemoDlg::OnBnClickedButton66()
{
	// TODO: Add your control notification handler code here
	CString csText;

	long stepcount = total_moves;

	if ( TRUE == IsAnswer )
	{
		csText.Format("%d", stepcount-1);
		m_btnData_dst3[0]->SetWindowText(csText);
	}
	else 
	{
		csText.Format("%d", 0);
		m_btnData_dst3[0]->SetWindowText(csText);
	}
}


void CPuzzleMxNDemoDlg::OnBnClickedButton67()
{
	// TODO: Add your control notification handler code here
	int k = 0, i, j;
	CString csText;
	static int cur_move = 0;

	if ( TRUE == IsAnswer )
	{
		if ( cur_path > 0 )
		{
			ptrPGNEXTMOVE(iPuzzleSizeW, iPuzzleSizeH, cur_path--, m_nData_src);
		}

		for ( i = 0; i < iPuzzleSizeH; i++ )
		{
			for ( j = 0; j < iPuzzleSizeW; j++ )
			{
				csText.Format("%d", m_nData_src[i][j]);
				m_btnData_src[i * MAX_PUZZLE_SIZE_W + j]->SetWindowText(csText);
			}
		}	

		// Display current step count
		csText.Format("%d", cur_move++);
		m_btnData_dst3[0]->SetWindowText(csText);

		Sleep(SLEEP_NUM);

		if ( cur_path == 0 )
		{
			cur_move = 0;
			MessageBox("Done!", "Show Path" , 0);		
		}
	}
}

// Mix up
void CPuzzleMxNDemoDlg::OnBnClickedButton68()
{
	// TODO: Add your control notification handler code here
	int i, j, k;
	int	lop = 100;
	int range_start = 0, range_end = iPuzzleSizeW * iPuzzleSizeH;
	int tmp;

	int tmp_array[MAX_PUZZLE_SIZE_W * MAX_PUZZLE_SIZE_H];
	int number = 0;
	int remain = iPuzzleSizeW * iPuzzleSizeH;

	if ( iPuzzleSizeW > MAX_PUZZLE_SIZE_W || iPuzzleSizeH > MAX_PUZZLE_SIZE_H)
	{
		return ;
	}

	for ( i = 0; i < remain; i++ )
	{
		tmp_array[i] = number++;
	}

	srand((unsigned) time(NULL)); 
	
	for ( i = 0; i < iPuzzleSizeH; i++ )
	{
		for ( j = 0; j < iPuzzleSizeW; j++ )
		{
			if ( lop > 1000 )
			{
				lop = 100;
			}

			for ( k = 0; k < lop; k++ )
			{
				tmp = rand() % (range_end - range_start) + range_start;
			}
			lop++;

			range_end--;
			m_nData_src[i][j] = tmp_array[tmp];

			if ( tmp != remain - 1 )
			{
				for ( k = tmp; k < remain - 1; k++ )
				{
					tmp_array[k] = tmp_array[k+1];				
				}
			}
			remain--;
		}
	}
	
	// Debug
	// Test 2x2
	//m_nData_src[0][0] = 1;
	//m_nData_src[0][1] = 2;
	//m_nData_src[1][0] = 0;
	//m_nData_src[1][1] = 3;	
	
	CString csText;

	for ( i = 0; i < iPuzzleSizeH; i++ )
	{
		for ( j = 0; j < iPuzzleSizeW; j++ )
		{
			csText.Format("%d", m_nData_src[i][j]);
			m_btnData_src[i * MAX_PUZZLE_SIZE_W + j]->SetWindowText(csText);
		}
	}


}
