using System;
using System.Runtime.InteropServices;
using UnityEngine;

public enum WindowsCursor
{
    StandardArrow = 32512,
    Hand = 32649, 
    OpenHand = 32652,
}

public class NativeCursorManager : MonoBehaviour
{
    public static NativeCursorManager Instance { get; private set; }

    private WindowsCursor currentCursor;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void SetCursor(WindowsCursor cursor)
    {
        currentCursor = cursor;
    }

    private void Update()
    {
        SetCursor(LoadCursor(IntPtr.Zero, (int)currentCursor));
    }

    [DllImport("user32.dll", EntryPoint = "SetCursor")]
    private static extern IntPtr SetCursor(IntPtr hCursor);

    [DllImport("user32.dll", EntryPoint = "LoadCursor")]
    private static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);
}
