#include "framework.h"
#include "ЛР4.h"
#include <windows.h>
#include <math.h>
#include <string>
#include <sstream>
#define CM_LINE 1
#define CM_SQUARE 2
#define CM_CIRCLE 3
#define CM_RED 4
#define CM_BLUE 5
#define CM_GREEN 6 

LRESULT WINAPI WindowProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);
BOOL WINAPI Regmain(HINSTANCE);

int key = 0;
HMODULE hLib;
COLORREF lineColor = RGB(0, 0, 0);
HDC hDC;
HMENU hmenu, hmenuF, hmenuC;
static POINTS ptsBegin;
static POINTS ptsEnd;
static BOOL fPrevLine = FALSE;
static LPCTSTR szAppName = L"WinApi";

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
    HWND hWnd;
    MSG msg;
    if (!Regmain(hInstance)) return FALSE;

    hWnd = CreateWindow(szAppName, NULL, WS_OVERLAPPEDWINDOW, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, 0, 0, hInstance, 0);
    if (!hWnd) return FALSE;

    ShowWindow(hWnd, nCmdShow);
    UpdateWindow(hWnd);

    while (GetMessage(&msg, 0, 0, 0))
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    return static_cast<int>(msg.wParam);
}

BOOL WINAPI Regmain(HINSTANCE hinst)
{
    WNDCLASSEX wc;
    wc.cbSize = sizeof(WNDCLASSEX);
    wc.style = CS_HREDRAW | CS_VREDRAW;
    wc.lpfnWndProc = WindowProc;
    wc.cbClsExtra = 0;
    wc.cbWndExtra = 0;
    wc.hInstance = hinst;
    wc.hIcon = LoadIcon(0, IDI_APPLICATION);
    wc.hCursor = LoadCursor(0, IDC_ARROW);
    wc.hbrBackground = static_cast<HBRUSH>(GetStockObject(WHITENESS));
    wc.lpszMenuName = 0;
    wc.lpszClassName = szAppName;
    wc.hIconSm = 0;

    return(RegisterClassEx(&wc)!=0);
}

LRESULT WINAPI WindowProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{

    PAINTSTRUCT PaintSt;
    RECT aRect;
    switch (message)

    {
    case WM_CREATE:
    {
        hLib = LoadLibrary(L"MyDll1.dll");
        if (!hLib) MessageBox(hWnd, L"Error", L"Cant download dll", MB_OK);
        hDC = GetDC(hWnd);
        hmenu = CreateMenu();
        SetMenu(hWnd, hmenu);
        hmenuF = CreatePopupMenu();
        hmenuC = CreatePopupMenu();
        AppendMenu(hmenuF, MF_ENABLED | MF_STRING, CM_LINE, L"&Line");
        AppendMenu(hmenuF, MF_ENABLED | MF_STRING, CM_SQUARE, L"&Square");
        AppendMenu(hmenuF, MF_ENABLED | MF_STRING, CM_CIRCLE, L"&Circle");
        AppendMenu(hmenuC, MF_ENABLED | MF_STRING, CM_RED, L"&Red");
        AppendMenu(hmenuC, MF_ENABLED | MF_STRING, CM_BLUE, L"&Blue");
        AppendMenu(hmenuC, MF_ENABLED | MF_STRING, CM_GREEN, L"&Green");
        AppendMenu(hmenu, MF_ENABLED | MF_POPUP, (UINT_PTR)hmenuF, L"&Figures");
        AppendMenu(hmenu, MF_ENABLED | MF_POPUP, (UINT_PTR)hmenuC, L"&Color");
        DrawMenuBar(hWnd);
    }

    case WM_COMMAND:
    {
        switch (wParam)
        {
        case CM_LINE:
        {
            key = 1;
            break;
        }
        case CM_SQUARE:
        {
            key = 2;
            break;
        }
        case CM_CIRCLE:
        {
            key = 3;
            break;
        }
        case CM_RED:
        {
            typedef COLORREF(*MYFUNC)(COLORREF);
            MYFUNC setColorRed = (MYFUNC)GetProcAddress(hLib, "setColorR");
            lineColor = setColorRed(lineColor);
            break;
        }
        case CM_BLUE:
        {
            typedef COLORREF(*MYFUNC)(COLORREF);
            MYFUNC setColorBlue = (MYFUNC)GetProcAddress(hLib, "setColorB");
            lineColor = setColorBlue(lineColor);
            break;
        }
        case CM_GREEN:
        {
            typedef COLORREF(*MYFUNC)(COLORREF);
            MYFUNC setColorGreen = (MYFUNC)GetProcAddress(hLib, "setColorG");
            lineColor = setColorGreen(lineColor);
            break;
        }
        }
        break;
    }

    case WM_LBUTTONDOWN:
        fPrevLine = FALSE;
        ptsBegin = MAKEPOINTS(lParam);
        return 0;

    case WM_MOUSEMOVE:

        if (wParam & MK_LBUTTON)
        {
            hDC = GetDC(hWnd);
            SetROP2(hDC, R2_NOTXORPEN);
            HPEN linePen;
            linePen = CreatePen(PS_SOLID, 1, lineColor);
            HGDIOBJ prevObj = SelectObject(hDC, linePen);
            if (key == 1)
            {
                if (fPrevLine)
                {
                    MoveToEx(hDC, ptsBegin.x, ptsBegin.y, (LPPOINT)NULL);
                    LineTo(hDC, ptsEnd.x, ptsEnd.y);
                }
                ptsEnd = MAKEPOINTS(lParam);
                MoveToEx(hDC, ptsBegin.x, ptsBegin.y, (LPPOINT)NULL);
                LineTo(hDC, ptsEnd.x, ptsEnd.y);
                fPrevLine = TRUE;
            }
            if (key == 2)
            {
                if (fPrevLine)
                {

                    Rectangle(hDC, ptsBegin.x, ptsBegin.y, ptsEnd.x, ptsEnd.y);
                }
                ptsEnd = MAKEPOINTS(lParam);
                Rectangle(hDC, ptsBegin.x, ptsBegin.y, ptsEnd.x, ptsEnd.y);
                fPrevLine = TRUE;
            }
            if (key == 3)
            {
                if (fPrevLine)
                {

                    Ellipse(hDC, ptsBegin.x, ptsBegin.y, ptsEnd.x, ptsEnd.y);
                }
                ptsEnd = MAKEPOINTS(lParam);
                Ellipse(hDC, ptsBegin.x, ptsBegin.y, ptsEnd.x, ptsEnd.y);
                fPrevLine = TRUE;
            }
        }
        break;

    case WM_DESTROY:

        PostQuitMessage(0);
        return 0;

    default:
        return DefWindowProc(hWnd, message, wParam, lParam);
    }
}

