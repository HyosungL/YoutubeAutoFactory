// AppCore/VrewFlowRunner.cs
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

            await vrewService.ClickOnImage("영상 (Vrew 3.1.0)", @"Images\새로만들기_기본.png");
            await Task.Delay(1000);
            await vrewService.ClickOnImage("영상 (Vrew 3.1.0)", @"Images\텍스트로비디오만들기.png");
            await Task.Delay(1000);
            await vrewService.ClickOnImage("영상 (Vrew 3.1.0)", @"Images\다음.png");

            Console.WriteLine("✅ 초기 흐름 완료");
        }
    }
}
