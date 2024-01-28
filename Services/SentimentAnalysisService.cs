
using Inverse_CC_bot.Interfaces;
using VaderSharp2;

namespace Inverse_CC_bot.Services
{
    public class SentimentAnalysisService: ISentimentAnalysisService

       
    {
        public SentimentIntensityAnalyzer _analyzer;

        public SentimentAnalysisService()
        {
            _analyzer = new SentimentIntensityAnalyzer();
        }
        public SentimentAnalysisResults AnalyzeSentiment(string text)
        {
            return _analyzer.PolarityScores(text);

        }
    }
}
