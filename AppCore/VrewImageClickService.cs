using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace YoutubeAutoFactory
{
    public class VrewImageClickService
    {
        public async Task ClickOnImage(string windowTitle, string imagePath)
        {
            await Task.Delay(1000);

            var hWnd = FindWindow(null, windowTitle);
            if (hWnd == IntPtr.Zero)
            {
                Console.WriteLine("❌ Vrew 창을 찾을 수 없습니다.");
                return;
            }

            Bitmap screenshot = CaptureWindow(hWnd);
            Mat source = BitmapConverter.ToMat(screenshot);
            Cv2.CvtColor(source, source, ColorConversionCodes.BGR2GRAY);

            Mat template = Cv2.ImRead(imagePath, ImreadModes.Grayscale);
            Mat result = new Mat();

            Cv2.MatchTemplate(source, template, result, TemplateMatchModes.CCoeffNormed);
            Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out OpenCvSharp.Point maxLoc);

            if (maxVal >= 0.95)
            {
                NativeMethods.RECT rect;
                NativeMethods.GetWindowRect(hWnd, out rect);
                int clickX = rect.Left + maxLoc.X + template.Width / 2;
                int clickY = rect.Top + maxLoc.Y + template.Height / 2;

                Console.WriteLine($"✅ 버튼 클릭 좌표: ({clickX}, {clickY})");

                NativeMethods.SetCursorPos(clickX, clickY);
                NativeMethods.mouse_event(NativeMethods.LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
                NativeMethods.mouse_event(NativeMethods.LEFTUP, 0, 0, 0, UIntPtr.Zero);
            }
            else
            {
                Console.WriteLine("❌ 이미지와 유사도가 낮아 클릭하지 않았습니다.");
            }
        }

        private Bitmap CaptureWindow(IntPtr hWnd)
        {
            NativeMethods.RECT rect;
            NativeMethods.GetWindowRect(hWnd, out rect);
            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;

            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(rect.Left, rect.Top, 0, 0, new System.Drawing.Size(width, height), CopyPixelOperation.SourceCopy);
            }

            return bmp;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    }

    public static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        public const uint LEFTDOWN = 0x0002;
        public const uint LEFTUP = 0x0004;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}
