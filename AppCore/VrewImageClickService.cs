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

        public async Task ClickOnImage(string windowTitle, string imagePath)
        {
            await Task.Delay(1000);

            var hWnd = FindWindow(null, windowTitle);
            if (hWnd == IntPtr.Zero)
            {
                Console.WriteLine("❌ Vrew 창을 찾을 수 없습니다.");
                return;
            }

            Bitmap screenshot = CaptureWindow(hWnd);                           // 스크린샷 캡처
            Mat source = BitmapConverter.ToMat(screenshot);                    // Bitmap → Mat 변환
            Mat template = Cv2.ImRead(imagePath, ImreadModes.Color);           // 이미지 불러오기
            Mat result = new Mat();                                            // 결과 저장용 Mat

            Cv2.MatchTemplate(source, template, result, TemplateMatchModes.CCoeffNormed); // 템플릿 매칭 실행

            Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out OpenCvSharp.Point maxLoc);

            if (maxVal >= 0.95)
            {
                var clickX = maxLoc.X + template.Width / 2;
                var clickY = maxLoc.Y + template.Height / 2;
                int lParam = (clickY << 16) | (clickX & 0xFFFF);

                PostMessage(hWnd, WM_LBUTTONDOWN, 0x00000001, lParam);
                PostMessage(hWnd, WM_LBUTTONUP, 0x00000001, lParam);
                Console.WriteLine($"✅ 클릭 완료: {imagePath}");
            }
            else
            {
                Console.WriteLine($"❌ 이미지 찾기 실패: {imagePath}");
            }
        }

        private Bitmap CaptureWindow(IntPtr hWnd)
        {
            Rectangle rect = new Rectangle();
            _ = NativeMethods.GetWindowRect(hWnd, ref rect);
            var bmp = new Bitmap(rect.Width - rect.X, rect.Height - rect.Y);
            using var g = Graphics.FromImage(bmp);
            g.CopyFromScreen(rect.X, rect.Y, 0, 0, bmp.Size);
            return bmp;
        }

        private static class NativeMethods
        {
            [DllImport("user32.dll")]
            public static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle rect);
        }
    }
}
