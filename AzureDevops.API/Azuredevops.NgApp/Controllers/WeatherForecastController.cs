using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
namespace Azuredevops.NgApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private string personalAccessToken = "idk4b2wt4yag4aggmod7wilqscs5d5nhwm25lylcsvwmtiycmpeq";
        private string org = "askha";
        private string project = "WVD%20Management";
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        //[HttpGet]
        //public IEnumerable<WeatherForecast> Get()
        //{           
        //    var rng = new Random();
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = rng.Next(-20, 55),
        //        Summary = Summaries[rng.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}
        [HttpGet]
        public string GetBuild()
        {
            string _respons = AzureDevopAPI();
            return _respons;

        }
        public string AzureDevopAPI()
        {
            //encode your personal access token                   
            string credentials = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalAccessToken)));

            string viewModel = null;

            //use the httpclient
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"https://dev.azure.com/{org}/");  //url of your organization
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                //connect to the REST endpoint            
                HttpResponseMessage response = client.GetAsync(
                    //"_apis/projects?stateFilter=All&api-version=1.0"
                    "WVD%20Management/_apis/build/builds?api-version=6.0"
                    ).Result;

                //check to see if we have a successful response
                if (response.IsSuccessStatusCode)
                {
                    //set the viewmodel from the content in the response
                    viewModel = response.Content.ReadAsStringAsync().Result;

                    //var value = response.Content.ReadAsStringAsync().Result;
                }
            }
            return viewModel;
        }
    } 
}
