// AppCore/VrewFlowRunner.cs
using System;
using System.Threading.Tasks;

namespace YoutubeAutoFactory
{
    public class VrewFlowRunner
    {
        private readonly VrewImageClickService vrewService = new();

        public async Task Run()
        {
            vrewService.LaunchVrew();
            Console.WriteLine("📦 Vrew 실행 중...");

            await Task.Delay(5000);

            IntPtr hWnd = vrewService.GetWindowHandle("영상 (Vrew 3.1.0)");
            await vrewService.ClickOnImage(hWnd, @"Images\새로만들기_기본.png");
            await Task.Delay(1000);
            await vrewService.ClickOnImage(hWnd, @"Images\텍스트로비디오만들기.png");
            await Task.Delay(1000);
            await vrewService.ClickOnImage(hWnd, @"Images\다음.png");

            Console.WriteLine("✅ 초기 흐름 완료");
        }
    }
}
