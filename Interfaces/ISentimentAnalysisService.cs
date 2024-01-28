using VaderSharp2;

namespace Inverse_CC_bot.Interfaces
{
    public interface ISentimentAnalysisService
    {
        SentimentAnalysisResults AnalyzeSentiment(string text);
    }
}
