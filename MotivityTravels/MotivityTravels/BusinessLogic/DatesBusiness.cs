using Microsoft.CognitiveServices.Speech;
using MotivityTravels.Models;

namespace MotivityTravels.BusinessLogic
{
    public class DatesBusiness
    {
        //this method will return start and end dates
        public async Task<TravelDetails> GetUserTravelDateInformation(Root root)
        {
            try
            {
                TravelDetails checkDate = new TravelDetails();
                if (root.prediction.entities.datetimeV2 != null)
                {
                    if (root.prediction.entities.datetimeV2[0].values[0].resolution[0].start != null)
                         checkDate.FromDate = Convert.ToDateTime(root.prediction.entities.datetimeV2[0].values[0].resolution[0].start);

                    if (root.prediction.entities.datetimeV2[0].values[0].resolution[0].end != null)
                         checkDate.ToDate = Convert.ToDateTime(root.prediction.entities.datetimeV2[0].values[0].resolution[0].end);
                }
                return checkDate;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //this method will execute, in case of start ot end dates missing
        public async Task<TravelDetails> GetSpecificTravelDateEntities()
        {
            try
            {
                Common _commonServices = new Common();
                TravelDetails entities = new TravelDetails();
                if (entities.FromDate == null || entities.ToDate == null)
                {
                    string textFromSource = "please tell us from date and to date";
                    Root locationEntities = await _commonServices.GetSpecificUserIntent(textFromSource);
                    TravelDetails fromDateEntity = await _commonServices.CheckUserEntities(locationEntities);
                    entities.FromDate = fromDateEntity.FromDate;
                    entities.ToDate = fromDateEntity.ToDate;
                }

                return entities;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
