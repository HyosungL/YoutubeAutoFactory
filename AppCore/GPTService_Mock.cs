using System.Threading.Tasks;

namespace YoutubeAutoFactory
{
    public class MockGPTService
    {
        public async Task<string> GenerateScript(string topic)
        {
            await Task.Delay(500);
            return $"[Mock 대본] 주제: {topic}에 대한 테스트용 대본입니다.";
        }
    }
}
