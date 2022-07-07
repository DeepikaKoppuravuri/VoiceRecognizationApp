using Microsoft.AspNetCore.Mvc;
using MotivityTravels.BusinessLogic;
using MotivityTravels.Models;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace MotivityTravels.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private SpeechServices _speechServices;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VoiceRecord()
        {
            TravelDetails entities = new TravelDetails();
            Common commonLogic = new Common();
            try
            {
                entities = await commonLogic.GetEntities();

                return View("Index", entities);
            }
            catch(Exception ee)
            {
                throw new Exception(ee.Message);
            }
            finally
            {
                entities = null;
                commonLogic = null;
            }
        }

        //save travel information into db
        [HttpPost]
        public async Task<IActionResult> PostTravelDetails(TravelDetails details)
        {

            try
            {
                string responseMessage = string.Empty;
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                using (var client = new HttpClient(clientHandler))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.PostAsJsonAsync("https://localhost:7236/traveldetails", details);

                    if (response.IsSuccessStatusCode)
                    {
                        ViewData["message"] = "Data saved successfully! ";
                        ModelState.Clear();
                        return View("Index");
                    }
                    else
                    {
                        ViewData["message"] = "Data not saved ";
                        return View("Index");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                details = null;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}