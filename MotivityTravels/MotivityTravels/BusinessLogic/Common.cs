using MotivityTravels.Models;
using MotivityTravels.BusinessLogic;
using Microsoft.CognitiveServices.Speech;

namespace MotivityTravels.BusinessLogic
{
    public class Common
    {
        private SpeechServices _speechServices;
        private LocationBusiness _locationEntities;
        private DatesBusiness _dateEntities;
        private PersonBusiness _peopleEntities;
        public Common()
        {
            _speechServices = new SpeechServices();
            _locationEntities = new LocationBusiness();
            _dateEntities = new DatesBusiness();
            _peopleEntities = new PersonBusiness();
        }
        public async Task<TravelDetails> GetEntities()
        {
            TravelDetails entities = new TravelDetails();
            string text = _speechServices.GetTextFromMicAsync().Result;

            Root intents = await _speechServices.GetIntents("9fd4c3541f074790870cbca4739be190", "https://eastus.api.cognitive.microsoft.com/", "4cf080b3-821a-4f38-8816-d356313b5bd2", text);
          
            entities = await CheckUserEntities(intents);
            entities = await CheckUserAllEntities(entities);

            return entities;
        }

        public async Task<TravelDetails> CheckUserEntities(Root root)
        {
            TravelDetails details = new TravelDetails();
            string intent = root.prediction.topIntent;
            switch(intent.ToLower())
            {
                case "location":
                    {
                        details = await _locationEntities.GetUserLocationInformation(root);
                        break;
                    }
                case "dates":
                    {
                        details = await _dateEntities.GetUserTravelDateInformation(root);
                        break ;
                    }
                case "persons":
                    {
                        details = await _peopleEntities.CheckPersonEntities(root);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return details;
        }
        public async Task<TravelDetails> CheckUserAllEntities(TravelDetails entities)
        {
            if (entities.FromSource == "" || entities.ToDestination == "")
            {
                TravelDetails locationEntities = await _locationEntities.GetSpecificEntities();
                entities.FromSource = locationEntities.FromSource;
                entities.ToDestination = locationEntities.ToDestination;
            }
            if (entities.FromDate == null || entities.ToDate == null)
            {
                TravelDetails dateEntities = await _dateEntities.GetSpecificTravelDateEntities();
                entities.FromDate = dateEntities.FromDate;
                entities.ToDate = dateEntities.ToDate;
            }
            if (entities.ParentsCount == 0 || entities.ChildernCount == 0)
            {
                TravelDetails personsEntities = await _peopleEntities.GetSpecificPersonEntities();
                entities.ParentsCount = personsEntities.ParentsCount;
                entities.ChildernCount = personsEntities.ChildernCount;
            }
            if (entities.FromSource == "" || entities.ToDestination == "" || entities.FromDate == null || entities.ToDate == null ||
                entities.ParentsCount == 0 || entities.ChildernCount == 0)
            {
                string speechText = "we are unable to identify few details, please enter manually..";
                await TextToSpeech(speechText);
            }
            return entities;
        }
        public async Task<Root> GetSpecificUserIntent(string speechText)
        {
            await TextToSpeech(speechText);
            string userSpeech = string.Empty;
            Root root = new Root();
            userSpeech = await _speechServices.GetTextFromMicAsync();
            root = await _speechServices.GetIntents("9fd4c3541f074790870cbca4739be190", "https://eastus.api.cognitive.microsoft.com/", "4cf080b3-821a-4f38-8816-d356313b5bd2", userSpeech);
            
            return root;
        }
        public async Task TextToSpeech(string speechText)
        {
            var config = SpeechConfig.FromSubscription("c7c252ec4cdc426cadd3d96bff0918e0", "eastus");

            // for speak slag
            // config.SpeechSynthesisVoiceName = "en-GB-RyanNeural";

            using var synthesizer = new SpeechSynthesizer(config);
           
            await synthesizer.SpeakTextAsync(speechText);
            
        }
    }
}
