using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace YoutubeAutoFactory
{
    public static class ScriptInputService
    {
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// 지정한 스크립트 파일을 읽어 Vrew 창에 붙여 넣습니다.
        /// </summary>
        /// <param name="vrewHandle">Vrew 창의 핸들</param>
        /// <param name="scriptPath">텍스트 스크립트 파일 경로</param>
        public static void InputScript(IntPtr vrewHandle, string scriptPath)
        {
            if (vrewHandle == IntPtr.Zero)
            {
                Console.WriteLine("❌ Vrew 창 핸들을 찾을 수 없습니다.");
                return;
            }

            if (!File.Exists(scriptPath))
            {
                Console.WriteLine($"❌ 스크립트 파일을 찾을 수 없습니다: {scriptPath}");
                return;
            }

            string scriptText = File.ReadAllText(scriptPath, System.Text.Encoding.UTF8);

            var staThread = new Thread(() => Clipboard.SetText(scriptText));
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();

            SetForegroundWindow(vrewHandle);
            SendKeys.SendWait("^v");

            Console.WriteLine("✅ 대본 입력 완료");
        }
    }
}
