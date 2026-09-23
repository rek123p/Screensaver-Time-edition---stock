using System;
using System.Runtime.InteropServices;

namespace Screensaver
{
    public static class LowLevelInputHook
    {
        private const int WH_MOUSE_LL = 14;
        private const int WH_KEYBOARD_LL = 13;

        private static IntPtr mouseHookId = IntPtr.Zero;
        private static IntPtr keyboardHookId = IntPtr.Zero;
        private static readonly LowLevelProc mouseProc = MouseHookCallback;
        private static readonly LowLevelProc keyboardProc = KeyboardHookCallback;

        private static POINT lastMousePos;
        private static bool hasLastMousePos = false;

        public static event Action? ActivityDetected;

        public static void Start()
        {
            mouseHookId = SetHook(WH_MOUSE_LL, mouseProc);
            keyboardHookId = SetHook(WH_KEYBOARD_LL, keyboardProc);
        }

        public static void Stop()
        {
            if (mouseHookId != IntPtr.Zero) UnhookWindowsHookEx(mouseHookId);
            if (keyboardHookId != IntPtr.Zero) UnhookWindowsHookEx(keyboardHookId);
            mouseHookId = IntPtr.Zero;
            keyboardHookId = IntPtr.Zero;
        }

        private static IntPtr SetHook(int hookType, LowLevelProc proc)
        {
            using var curProcess = System.Diagnostics.Process.GetCurrentProcess();
            using var curModule = curProcess.MainModule;
            return SetWindowsHookEx(hookType, proc, GetModuleHandle(curModule!.ModuleName), 0);
        }

        private delegate IntPtr LowLevelProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var hookStruct = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
                if (!hasLastMousePos)
                {
                    lastMousePos = hookStruct.pt;
                    hasLastMousePos = true;
                }
                else if (hookStruct.pt.x != lastMousePos.x || hookStruct.pt.y != lastMousePos.y)
                {
                    ActivityDetected?.Invoke();
                }
            }
            return CallNextHookEx(mouseHookId, nCode, wParam, lParam);
        }

        private static IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                ActivityDetected?.Invoke();
            }
            return CallNextHookEx(keyboardHookId, nCode, wParam, lParam);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT { public int x; public int y; }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
