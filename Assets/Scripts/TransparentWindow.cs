using UnityEngine;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;

public class TransparentWindow : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out POINT lpPoint);

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int X;
        public int Y;
    }

    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    private const int GWL_STYLE = -16;
    private const int GWL_EXSTYLE = -20;

    private const uint WS_POPUP = 0x80000000;
    private const uint WS_VISIBLE = 0x10000000;

    private const uint WS_EX_LAYERED = 0x00080000;
    private const uint WS_EX_TRANSPARENT = 0x00000020;

    private static readonly IntPtr HWND_TOPMOST = new(-1);
    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOMOVE = 0x0002;

    private IntPtr hWnd;
    private Camera mainCamera;
    private bool isClickThrough = false;

    private void Start()
    {
        mainCamera = Camera.main;
#if !UNITY_EDITOR
        hWnd = GetActiveWindow();

        MARGINS margins = new MARGINS { cxLeftWidth = -1, cxRightWidth = -1, cyTopHeight = -1, cyBottomHeight = -1 };
        DwmExtendFrameIntoClientArea(hWnd, ref margins);

        SetWindowLong(hWnd, GWL_STYLE, unchecked((int)(WS_POPUP | WS_VISIBLE)));
        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);

        SetClickThrough(true);
#endif
    }

    private void Update()
    {
#if !UNITY_EDITOR
        bool isOverObject = IsPointerOverGameObject();

        if (isOverObject && isClickThrough)
        {
            SetClickThrough(false); 
        }
        else if (!isOverObject && !isClickThrough)
        {
            SetClickThrough(true); 
        }
#endif
    }

    private void SetClickThrough(bool transparent)
    {
        isClickThrough = transparent;
        int currentExStyle = GetWindowLong(hWnd, GWL_EXSTYLE);

        if (transparent)
        {
            SetWindowLong(hWnd, GWL_EXSTYLE, unchecked((int)(currentExStyle | WS_EX_LAYERED | WS_EX_TRANSPARENT)));
        }
        else
        {
            SetWindowLong(hWnd, GWL_EXSTYLE, unchecked((int)(currentExStyle & ~WS_EX_TRANSPARENT)));
        }
    }

    private bool IsPointerOverGameObject()
    {
        GetCursorPos(out POINT point);
        Vector2 mousePos = new(point.X, Screen.height - point.Y);

        PointerEventData eventData = new (EventSystem.current) { position = mousePos };
        List<RaycastResult> results = new();
        if (EventSystem.current != null)
        {
            EventSystem.current.RaycastAll(eventData, results);
            if (results.Count > 0) return true;
        }

        Vector2 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
        RaycastHit2D hit2D = Physics2D.Raycast(worldPos, Vector2.zero);
        if (hit2D.collider != null) return true;

        return false;
    }
}