using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDTO> response = new List<RegionDTO>();
            try
            {

                // Get All Regions from Web api
                var client = httpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync("https://localhost:7193/api/regions");
                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>());

            }
            catch (Exception)
            {
                throw;
            }

            return View(response);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7193/api/regions"),
                    Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
                };
                var httpResponseMessage = await client.SendAsync(httpRequestMessage);

                httpResponseMessage.EnsureSuccessStatusCode();

                var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();

                if (response != null)
                {
                    return RedirectToAction("Index", "Regions");

                }
                return View();

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<RegionDTO>($"https://localhost:7193/api/regions/{id.ToString()}");
            if (response != null)
            {
                return View(response);
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Edit(RegionDTO regionDTO)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"https://localhost:7193/api/regions/{regionDTO.Id}"),
                    Content = new StringContent(JsonSerializer.Serialize(regionDTO), Encoding.UTF8, "application/json")
                };
                var httpResponseMessage = await client.SendAsync(httpRequestMessage);

                httpResponseMessage.EnsureSuccessStatusCode();

                var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();

                if (response != null)
                {
                    return RedirectToAction("Edit", "Regions");

                }
                return View();

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RegionDTO regionDTO)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpReponseMessage = await client.DeleteAsync($"https://localhost:7193/api/regions/{regionDTO.Id}");
                httpReponseMessage.EnsureSuccessStatusCode();


                return RedirectToAction("Index", "Regions");
            }
            catch (Exception ex)
            {

            }
            return View("Edit");
        }
    }

}
