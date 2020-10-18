
// MathDemoDlg.cpp : implementation file
//

#include "stdafx.h"
#include "MathDemo.h"
#include "MathDemoDlg.h"
#include "afxdialogex.h"

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


// CMathDemoDlg dialog




CMathDemoDlg::CMathDemoDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CMathDemoDlg::IDD, pParent)
	, InputNumber(0)
	/*, InputMulA(_T(""))*/
	, InputMulB(0)
	, InputMulA(0)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CMathDemoDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);

	// Added
	DDX_Control(pDX, IDC_BUTTON1, m_btn_1);
	DDX_Control(pDX, IDC_BUTTON2, m_btn_2);
	DDX_Control(pDX, IDC_BUTTON3, m_btn_3);
	DDX_Control(pDX, IDC_BUTTON4, m_btn_4);
	DDX_Control(pDX, IDC_BUTTON5, m_btn_5);
	DDX_Control(pDX, IDC_BUTTON6, m_btn_6);
	DDX_Control(pDX, IDC_BUTTON7, m_btn_7);
	DDX_Control(pDX, IDC_BUTTON8, m_btn_8);
	DDX_Control(pDX, IDC_BUTTON9, m_btn_9);

	DDX_Text(pDX, IDC_EDIT1, InputNumber);
	DDV_MinMaxInt(pDX, InputNumber, 0, 1000000);
	//  DDX_Text(pDX, IDC_EDIT2, InputMulA);
	DDX_Text(pDX, IDC_EDIT3, InputMulB);
	DDV_MinMaxInt(pDX, InputMulB, 0, 10000000);
	DDX_Text(pDX, IDC_EDIT2, InputMulA);
	DDV_MinMaxInt(pDX, InputMulA, 0, 10000000);
}

BEGIN_MESSAGE_MAP(CMathDemoDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON7, &CMathDemoDlg::OnBnClickedButton7)
	ON_BN_CLICKED(IDC_BUTTON1, &CMathDemoDlg::OnBnClickedButton1)
	ON_BN_CLICKED(IDC_BUTTON2, &CMathDemoDlg::OnBnClickedButton2)
	ON_BN_CLICKED(IDC_BUTTON3, &CMathDemoDlg::OnBnClickedButton3)
	ON_BN_CLICKED(IDC_BUTTON4, &CMathDemoDlg::OnBnClickedButton4)
	ON_BN_CLICKED(IDC_BUTTON5, &CMathDemoDlg::OnBnClickedButton5)
	ON_BN_CLICKED(IDC_BUTTON6, &CMathDemoDlg::OnBnClickedButton6)
	ON_BN_CLICKED(IDC_BUTTON8, &CMathDemoDlg::OnBnClickedButton8)
	ON_WM_DESTROY()
	ON_EN_CHANGE(IDC_EDIT1, &CMathDemoDlg::OnEnChangeEdit1)
	ON_EN_CHANGE(IDC_EDIT2, &CMathDemoDlg::OnEnChangeEdit2)
	ON_EN_CHANGE(IDC_EDIT3, &CMathDemoDlg::OnEnChangeEdit3)
	ON_BN_CLICKED(IDC_BUTTON9, &CMathDemoDlg::OnBnClickedButton9)
END_MESSAGE_MAP()


// CMathDemoDlg message handlers

BOOL CMathDemoDlg::OnInitDialog()
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

	ShowWindow(SW_MAXIMIZE);

	// TODO: Add extra initialization here
	// Added
	m_btnData_dst[0] = &m_btn_1;
	m_btnData_dst[1] = &m_btn_3;
	m_btnData_dst[2] = &m_btn_5;
	m_btnData_dst[3] = &m_btn_7;
	m_btnData_dst[4] = &m_btn_9;

	memset(m_nData_dst, 0, sizeof(int) * 6);

	m_nData_dst[0] = 0;
	m_nData_dst[1] = 100;
	m_nData_dst[2] = 1000;
	m_nData_dst[3] = 10000;

	CString csText;

	for (int i = 0; i < 5; i++)
	{
		csText.Format("%d", m_nData_dst[i]);
		m_btnData_dst[i]->SetWindowText(csText);
	}

	// Initialization
	hInst = NULL;

	// Load DLL
	hInst = LoadLibrary(("MathCalcDll.dll"));
	if ( !hInst ) 
	{
		printf("Dll failure!!\n");
		return FALSE;
	}

	ptrMCINT = (PMCINT)GetProcAddress(hInst, MAKEINTRESOURCE(1));
	if ( !ptrMCINT ) 
	{
		printf("Function 1 in Dll failure!!\n");
		return FALSE;
	}

	ptrMCRANDOM = (PMCRANDOM)GetProcAddress(hInst, MAKEINTRESOURCE(2));
	if ( !ptrMCRANDOM ) 
	{
		printf("Function 4 in Dll failure!!\n");
		return FALSE;
	}

	ptrMCRULE = (PMCRULE)GetProcAddress(hInst, MAKEINTRESOURCE(3));
	if ( !ptrMCRULE ) 
	{
		printf("Function 3 in Dll failure!!\n");
		return FALSE;
	}

	ptrMCDESTROY = (PMCDESTROY)GetProcAddress(hInst, MAKEINTRESOURCE(4));
	if ( !ptrMCDESTROY ) 
	{
		printf("Function 5 in Dll failure!!\n");
		return FALSE;
	}	

	hInst2 = LoadLibrary(("MathMul.dll"));
	ptrMMINT = (PMMINT)GetProcAddress(hInst2, MAKEINTRESOURCE(1));
	if ( !ptrMMINT ) 
	{
		printf("Function 1 in Dll failure!!\n");
		return FALSE;
	}

	ptrMMRANDOM = (PMMRANDOM)GetProcAddress(hInst2, MAKEINTRESOURCE(2));
	if ( !ptrMMRANDOM ) 
	{
		printf("Function 2 in Dll failure!!\n");
		return FALSE;
	}

	ptrMMDESTROY = (PMMDESTROY)GetProcAddress(hInst2, MAKEINTRESOURCE(3));
	if ( !ptrMMDESTROY ) 
	{
		printf("Function 3 in Dll failure!!\n");
		return FALSE;
	}

	void *pTmp = NULL;
	ptrMCINT(pTmp);
	ptrMMINT(NULL);

	return TRUE;  // return TRUE  unless you set the focus to a control
}

void CMathDemoDlg::OnDestroy()
{
	CDialogEx::OnDestroy();

	// TODO: Add your message handler code here
}

void CMathDemoDlg::OnSysCommand(UINT nID, LPARAM lParam)
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

void CMathDemoDlg::OnPaint()
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
HCURSOR CMathDemoDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



// 20
void CMathDemoDlg::OnBnClickedButton1()
{
	// TODO: Add your control notification handler code here
	static int flag = 0;
	static int length_add = 0, length_sub = 0;
	CString csText;
	int A, B, C;
	int iLimitNum = InputNumber;

	
	if ( iLimitNum <= 0 )
	{
		return ;
	}

	// Rendom select
	ptrMCRANDOM(&A, &B, &C, iLimitNum, 0);

	if ( A + B == C )
	{
		csText.Format("%d + %d = %d", A, B, C);
	}
	else
	{
		csText.Format("%d - %d = %d", A, B, C);
	}

	m_btnData_dst[0]->SetWindowText(csText);
}

// 100
void CMathDemoDlg::OnBnClickedButton2()
{
	// TODO: Add your control notification handler code here
}

// 100
void CMathDemoDlg::OnBnClickedButton3()
{
	// TODO: Add your control notification handler code here

}


void CMathDemoDlg::OnBnClickedButton4()
{
	// TODO: Add your control notification handler code here
}

// 1000
void CMathDemoDlg::OnBnClickedButton5()
{
	// TODO: Add your control notification handler code here
}


void CMathDemoDlg::OnBnClickedButton6()
{
	// TODO: Add your control notification handler code here
}

// 10000
void CMathDemoDlg::OnBnClickedButton7()
{
	// TODO: Add your control notification handler code here
}

void CMathDemoDlg::OnBnClickedButton8()
{
	// TODO: Add your control notification handler code here
}


void CMathDemoDlg::OnEnChangeEdit1()
{
	// TODO:  If this is a RICHEDIT control, the control will not
	// send this notification unless you override the CDialogEx::OnInitDialog()
	// function and call CRichEditCtrl().SetEventMask()
	// with the ENM_CHANGE flag ORed into the mask.

	// TODO:  Add your control notification handler code here

	CString strTemp;
	CEdit* edit1 = ((CEdit*)(GetDlgItem(IDC_EDIT1)));
	edit1->GetWindowText(strTemp);

	InputNumber = atoi(strTemp);
}


void CMathDemoDlg::OnEnChangeEdit2()
{
	// TODO:  If this is a RICHEDIT control, the control will not
	// send this notification unless you override the CDialogEx::OnInitDialog()
	// function and call CRichEditCtrl().SetEventMask()
	// with the ENM_CHANGE flag ORed into the mask.

	// TODO:  Add your control notification handler code here
	CString strTemp;
	CEdit* edit1 = ((CEdit*)(GetDlgItem(IDC_EDIT2)));
	edit1->GetWindowText(strTemp);

	InputMulA = atoi(strTemp);
}


void CMathDemoDlg::OnEnChangeEdit3()
{
	// TODO:  If this is a RICHEDIT control, the control will not
	// send this notification unless you override the CDialogEx::OnInitDialog()
	// function and call CRichEditCtrl().SetEventMask()
	// with the ENM_CHANGE flag ORed into the mask.

	// TODO:  Add your control notification handler code here
	CString strTemp;
	CEdit* edit1 = ((CEdit*)(GetDlgItem(IDC_EDIT3)));
	edit1->GetWindowText(strTemp);

	InputMulB = atoi(strTemp);
}


void CMathDemoDlg::OnBnClickedButton9()
{
	// TODO: Add your control notification handler code here
	CString csText;
	double A, B, C;
	int iLimitA = InputMulA, iLimitB = InputMulB;
	
	// Debug
	//iLimitNum = 20;
	//int temp[20];
	//int i=0;

	//for ( i=0; i<iLimitNum; i++ )
	//{
	//	ptrMCRANDOM(&A, &B, &C, iLimitNum, 0);

	//	temp[i] = A;
	//}

	
	if ( iLimitA <= 0 || iLimitB <= 0 )
	{
		return ;
	}

	// Rendom select
	ptrMMRANDOM(&A, &B, &C, iLimitA, iLimitB, 0);

	//csText.Format("%0.1f x %0.1f = %0.1f", A, B, C);
	csText.Format("%d x %d = %d", (int)A, (int)B, (int)C);

	m_btnData_dst[4]->SetWindowText(csText);
}
