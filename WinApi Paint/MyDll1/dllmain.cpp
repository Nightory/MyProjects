#include "pch.h"
#include <Windows.h>
BOOL APIENTRY DllMain(HMODULE hModule,
    DWORD  ul_reason_for_call,
    LPVOID lpReserved
)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}
extern "C" _declspec(dllexport) COLORREF setColorR(COLORREF  lineColor)
{
    lineColor = RGB(255, 0, 0);
    return lineColor;
}
extern "C" _declspec(dllexport) COLORREF setColorG(COLORREF  lineColor)
{
    lineColor = RGB(0, 255, 0);
    return lineColor;
}
extern "C" _declspec(dllexport) COLORREF setColorB(COLORREF  lineColor)
{
    lineColor = RGB(0, 0, 255);
    return lineColor;
}


