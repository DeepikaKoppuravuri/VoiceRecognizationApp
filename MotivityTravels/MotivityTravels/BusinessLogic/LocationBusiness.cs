using Microsoft.CognitiveServices.Speech;
using MotivityTravels.Models;

namespace MotivityTravels.BusinessLogic
{
    public class LocationBusiness
    {
        private SpeechServices _speechServices;
        public LocationBusiness()
        {
            _speechServices = new SpeechServices();
        }
        //this method will return user source and destination 
        public async Task<TravelDetails> GetUserLocationInformation(Root root)
        {
            try
            {
                TravelDetails checkLocationDetails = new TravelDetails();

                if (root.prediction.entities.Destination != null)
                {

                    if (root.prediction.entities.Destination[0].FromSource != null)
                    {
                        checkLocationDetails.FromSource = (root.prediction.entities.Destination[0].FromSource[0]).Substring(4);
                    }
                    if (root.prediction.entities.Destination[1].ToDestination != null)
                    {
                        checkLocationDetails.ToDestination = (root.prediction.entities.Destination[1].ToDestination[0]).Substring(3);
                    }

                }
                return checkLocationDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //this method will execute, in case of source or destination are missing
        public async Task<TravelDetails> GetSpecificEntities()
        {
            try
            {
                Common _commonServices = new Common();
                TravelDetails entities = new TravelDetails();
                string textFromSource = "please tell us from source and to destination";
                Root locationEntities = await _commonServices.GetSpecificUserIntent(textFromSource);
                TravelDetails locationEntity = await _commonServices.CheckUserEntities(locationEntities);
                entities.FromSource = locationEntity.FromSource;
                entities.ToDestination = locationEntity.ToDestination;

                return entities;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
