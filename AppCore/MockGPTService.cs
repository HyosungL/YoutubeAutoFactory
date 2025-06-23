using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore
{
    internal class MockGPTService
    {
        public async Task<string> GenerateScript(string topic)
        {
            await Task.Delay(500); // 일부러 약간 대기

            return $"[Mock 대본 생성]\n\n주제: {topic}\n\n이건 실제 GPT 응답이 아니라, 테스트용 더미 데이터입니다.\n\n1. 후킹 문구\n2. 문제 제시\n3. 기능 설명\n4. 감성 연출\n5. CTA (상품 보기 클릭 유도)";
        }
    }
}
