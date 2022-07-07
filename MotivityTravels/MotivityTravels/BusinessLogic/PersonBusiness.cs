using Microsoft.CognitiveServices.Speech;
using MotivityTravels.Models;

namespace MotivityTravels.BusinessLogic
{
    public class PersonBusiness
    {
        public async Task<TravelDetails> CheckPersonEntities(Root root)
        {
            TravelDetails personEntities = new TravelDetails();
            try
            {
                if (root.prediction.entities.TotalPersons[0].Adults != null)
                {
                    if (root.prediction.entities.number[0] != null)
                    {
                        personEntities.ParentsCount = root.prediction.entities.number[0];
                    }
                    if (root.prediction.entities.number[1] != null)
                    {
                        personEntities.ChildernCount = root.prediction.entities.number[1];
                    }

                    // string adults = root.prediction.entities.TotalPersons[0].Adults[0];
                }
                return personEntities;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                personEntities = null;
                root = null;
            }
        }
        //this method will return, adults and child count
        public async Task<TravelDetails> GetSpecificPersonEntities()
        {
            Common _commonLogic = new Common();
            try
            {

                string speechText = "please tell us number of adult and children";
                Root numberOfPersons = await _commonLogic.GetSpecificUserIntent(speechText);
                TravelDetails personEntities = await _commonLogic.CheckUserEntities(numberOfPersons);

                return personEntities;
            }
            catch (Exception ee)
            {
                throw;
            }
            finally
            {
                _commonLogic = null;
            }
        }
    }
}
