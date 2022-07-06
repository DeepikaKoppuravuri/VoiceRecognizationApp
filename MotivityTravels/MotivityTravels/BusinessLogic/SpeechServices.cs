using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Intent;
using MotivityTravels.Models;
using System.Web;

namespace MotivityTravels.BusinessLogic
{
    public class SpeechServices
    {
   
        public async Task<string> GetTextFromMicAsync()
        {
            try
            {
                var config = SpeechConfig.FromSubscription("c7c252ec4cdc426cadd3d96bff0918e0", "eastus");
                config.RequestWordLevelTimestamps();
                using (var recognizer = new IntentRecognizer(config))
                {
                    var result = await recognizer.RecognizeOnceAsync();
                    if (result.Reason == ResultReason.RecognizedSpeech)
                    {
                        return result.Text;
                    }
                    else
                    {
                        return "Speech could not be recognizes";
                    }
                }
            }
            catch (Exception ee)
            {
                return ee.Message;
            }
        }
        public async Task<Root> GetIntents(string predictionKey, string predionEndPoint, string appId, string text)
        {
            try
            {
                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                client.DefaultRequestHeaders.Add("ocp-Apim-Subscription-key", predictionKey);
                queryString["query"] = text;
                var preEndPointUri = string.Format("{0}luis/prediction/v3.0/apps/{1}/slots/production/predict?{2}", predionEndPoint, appId, queryString);
                var response = await client.GetAsync(preEndPointUri);
                var result = await response.Content.ReadAsStringAsync();

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                Root root = JsonConvert.DeserializeObject<Root>(result, settings);

                return root;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
