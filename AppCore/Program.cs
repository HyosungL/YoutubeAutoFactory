using System;
using System.Threading.Tasks;

namespace YoutubeAutoFactory
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("🟦 브루를 먼저 켜고, '새로 만들기' 버튼이 화면에 보이게 해주세요.");
            Console.WriteLine("3초 뒤에 버튼 이미지를 찾아서 자동 클릭합니다...");
            await Task.Delay(3000);  // 3초 대기

            var service = new VrewImageClickService();
            await service.ClickOnImage("영상 (Vrew 3.1.0)", @"Images\NewProject.png");

            Console.WriteLine("✅ 클릭 시도 완료. 결과는 위 로그를 확인하세요.");
            Console.WriteLine("⏹ 아무 키나 누르면 종료됩니다.");
            Console.ReadKey();
        }
    }
}
