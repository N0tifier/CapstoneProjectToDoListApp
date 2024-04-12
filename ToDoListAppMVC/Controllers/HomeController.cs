using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDoListAppMVC.Models;

namespace ToDoListAppMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public HomeController(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        public async Task<IActionResult> ToDoList()
        {
            var inputModel = new GetToDoInput
            {
                ImportantOnly = true, // Set according to your needs or user input
                TodayOnly = true      // Set according to your needs or user input
            };

            var apiUrl = _configuration["ToDoApiUrl"] + "/get-list";
            var client = _clientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(apiUrl, inputModel);

            if (response.IsSuccessStatusCode)
            {
                var toDoList = await response.Content.ReadFromJsonAsync<GenericResponse>();
                return View(toDoList.Output); // Assumes you have a corresponding view
            }
            else
            {
                // Handle error
                return View("Error");
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string requestId)
        {
            return View(new ErrorViewModel { RequestId = requestId });
        }
    }
}