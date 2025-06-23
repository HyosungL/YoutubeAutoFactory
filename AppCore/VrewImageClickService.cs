// AppCore/VrewImageClickService.cs
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace YoutubeAutoFactory
{
    public class VrewImageClickService
    {
        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        private const uint WM_LBUTTONDOWN = 0x0201;
        private const uint WM_LBUTTONUP = 0x0202;

        public void LaunchVrew()
        {
            Process.Start(@"C:\Users\qqqq\AppData\Local\Programs\vrew\Vrew.exe");
        }

        public IntPtr GetWindowHandle(string windowTitle)
        {
            return FindWindow(null, windowTitle);
        }

        public async Task ClickOnImage(IntPtr hWnd, string imagePath, double threshold = 0.95)
        {
            await Task.Delay(1000);

            if (hWnd == IntPtr.Zero)
            {
                Console.WriteLine("❌ Vrew 창을 찾을 수 없습니다.");
                return;
            }

            using Bitmap screenshot = CaptureWindow(hWnd);                     // 스크린샷 캡처 (PrintWindow)
            using Mat source = BitmapConverter.ToMat(screenshot);              // Bitmap → Mat 변환
            using Mat template = Cv2.ImRead(imagePath, ImreadModes.Color);     // 이미지 불러오기
            using Mat result = new();                                          // 결과 저장용 Mat

            Cv2.MatchTemplate(source, template, result, TemplateMatchModes.CCoeffNormed); // 템플릿 매칭 실행

            Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out OpenCvSharp.Point maxLoc);

            if (maxVal >= threshold)
            {
                int clickX = maxLoc.X + template.Width / 2;
                int clickY = maxLoc.Y + template.Height / 2;

                NativeMethods.POINT pt = new() { X = clickX, Y = clickY };
                _ = NativeMethods.ClientToScreen(hWnd, ref pt);                // 좌표 변환
                int lParam = (pt.Y << 16) | (pt.X & 0xFFFF);

                PostMessage(hWnd, WM_LBUTTONDOWN, 0x00000001, lParam);
                PostMessage(hWnd, WM_LBUTTONUP, 0x00000001, lParam);           // 클릭 이벤트 전송
                Console.WriteLine($"✅ 클릭 완료: {imagePath}");
            }
            else
            {
                Console.WriteLine($"❌ 이미지 찾기 실패: {imagePath}");
            }
        }

        private Bitmap CaptureWindow(IntPtr hWnd)
        {
            Rectangle rect = new();
            _ = NativeMethods.GetWindowRect(hWnd, ref rect);
            var bmp = new Bitmap(rect.Width - rect.X, rect.Height - rect.Y);
            using Graphics g = Graphics.FromImage(bmp);
            IntPtr hDc = g.GetHdc();
            bool success = NativeMethods.PrintWindow(hWnd, hDc, 0);
            g.ReleaseHdc(hDc);

            if (!success)
            {
                g.CopyFromScreen(rect.X, rect.Y, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
            }

            return bmp;
        }

        private static class NativeMethods
        {
            [DllImport("user32.dll")]
            public static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle rect);

            [DllImport("user32.dll")]
            public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, uint nFlags);

            [DllImport("user32.dll")]
            public static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

            [StructLayout(LayoutKind.Sequential)]
            public struct POINT
            {
                public int X;
                public int Y;
            }
        }
    }
}
