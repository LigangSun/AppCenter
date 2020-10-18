
// MathDemoDlg.h : header file
//

#include "..\common\MathcalcAPI.h"
#include "..\common\MathmulAPI.h"

#pragma once


// CMathDemoDlg dialog
class CMathDemoDlg : public CDialogEx
{
// Construction
public:
	CMathDemoDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	enum { IDD = IDD_MATHDEMO_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support

// Implementation
protected:
	HICON m_hIcon;

	// Added
	int m_nData_dst[6];
	CButton* m_btnData_dst[6];

#ifdef _WIN32

	HINSTANCE hInst;

	typedef void (*PMCINT)(	void*);
	PMCINT ptrMCINT;

	typedef void (*PMCRANDOM)(int* ,int* ,int*, int, int);
	PMCRANDOM ptrMCRANDOM;

	typedef void (*PMCRULE)(int* ,int* ,int*, int, int);
	PMCRULE ptrMCRULE;

	typedef void (*PMCDESTROY)();
	PMCDESTROY ptrMCDESTROY;

	HINSTANCE hInst2;

	typedef void (*PMMINT)(	void*);
	PMMINT ptrMMINT;

	typedef void (*PMMRANDOM)(double* ,double* ,double*, int, int, int);
	PMMRANDOM ptrMMRANDOM;

	typedef void (*PMMDESTROY)();
	PMMDESTROY ptrMMDESTROY;
#endif //_WIN32

	CButton m_btn_1;
	CButton m_btn_2;
	CButton m_btn_3;
	CButton m_btn_4;
	CButton m_btn_5;
	CButton m_btn_6;
	CButton m_btn_7;
	CButton m_btn_8;
	CButton m_btn_9;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedButton7();
	afx_msg void OnBnClickedButton1();
	afx_msg void OnBnClickedButton2();
	afx_msg void OnBnClickedButton3();
	afx_msg void OnBnClickedButton4();
	afx_msg void OnBnClickedButton5();
	afx_msg void OnBnClickedButton6();
	afx_msg void OnBnClickedButton8();
	afx_msg void OnDestroy();
	afx_msg void OnEnChangeEdit1();
	int InputNumber;
	afx_msg void OnEnChangeEdit2();
	afx_msg void OnEnChangeEdit3();
	afx_msg void OnBnClickedButton9();
//	CString InputMulA;
	int InputMulB;
	int InputMulA;
};
