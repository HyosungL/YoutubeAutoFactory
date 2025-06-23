using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace YoutubeAutoFactory
{
    public static class ScriptInputService
    {
        public static void InputScript(IntPtr vrewHandle, string scriptPath)
        {
            if (!File.Exists(scriptPath))
            {
                Console.WriteLine($"❌ 파일을 찾을 수 없습니다: {scriptPath}");
                return;
            }

            string text = File.ReadAllText(scriptPath);
            Clipboard.SetText(text);

            SetForegroundWindow(vrewHandle);
            Thread.Sleep(200);
            SendKeys.SendWait("^v");
        }

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
