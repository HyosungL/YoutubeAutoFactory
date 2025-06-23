using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YoutubeAutoFactory
{
    public class Program
    {
        //public static async Task Main(string[] args)
        //{
        //    // ✅ [1] config.json에서 API 키 로딩
        //    string configText = File.ReadAllText("config.json");
        //    var config = JsonConvert.DeserializeObject<Config>(configText);

        //    // ✅ [2] GPTService 생성자에 apiKey 전달
        //    var gptService = new GPTService(config.OpenAIApiKey);

        //    // ✅ [3] 주제 입력
        //    Console.Write("📝 주제 입력: ");
        //    string topic = Console.ReadLine();

        //    // ✅ [4] GPT 호출 및 결과 출력
        //    var script = await gptService.GenerateScript(topic);
        //    Console.WriteLine("\n🧾 생성된 대본:\n");
        //    Console.WriteLine(script);
        //}

        //static async Task Main(string[] args)
        //{
        //    Console.WriteLine("🧪 [Mock 테스트 시작] 주제를 입력하세요:");
        //    var topic = Console.ReadLine();

        //    // 실제 GPT 대신 MockGPTService 사용
        //    var gptService = new MockGPTService();
        //    var script = await gptService.GenerateScript(topic ?? "기본 테스트 주제");

        //    Console.WriteLine("\n🎬 생성된 대본:");
        //    Console.WriteLine("----------------------------------");
        //    Console.WriteLine(script);
        //    Console.WriteLine("----------------------------------");
        //}
            static async Task Main(string[] args)
            {
                var vrewRunner = new VrewFlowRunner();
                await vrewRunner.Run();

                Console.WriteLine("⏹ 종료 - 아무 키나 누르세요.");
                Console.ReadKey();
            }
    }
}
