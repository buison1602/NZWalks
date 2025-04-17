using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
            List<RegionDto> response = new List<RegionDto>();

            try
            {
                // Get All Regions from Web API
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7027/api/regions");

                httpResponseMessage.EnsureSuccessStatusCode();

                // chuyển đổi (deserialize) nội dung phản hồi từ một yêu cầu HTTP thành một danh sách
                // các đối tượng kiểu RegionsDto
                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
            }
            catch (Exception ex)
            {
                // Log the exception
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
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7027/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

            if (response is not null)
            {
                return RedirectToAction("index", "Regions");
            }

            return View();
        }


        /* LÝ DO DÙNG HttpPost cho Edit và Delete
        - Controller của ứng dụng MVC (hoặc Razor Pages), không phải là API. Trong khi đó, các yêu cầu được 
        gửi từ controller đến API backend (tại https://localhost:7027/api/regions/...) sử dụng các phương 
        thức HTTP đúng ngữ nghĩa:
            + Edit sử dụng HttpMethod.Put để gọi API (phù hợp với cập nhật dữ liệu).
            + Delete sử dụng client.DeleteAsync (tương ứng với HttpMethod.Delete).
        
        - Tuy nhiên, trong controller, các phương thức Edit và Delete được gọi từ client-side (trình duyệt) 
        thông qua form HTML, nên chúng sử dụng [HttpPost]. Controller ở đây đóng vai trò trung gian:
            + Nhận yêu cầu POST từ form HTML.
            + Chuyển đổi thành yêu cầu PUT hoặc DELETE để gọi API.
            + Xử lý phản hồi từ API và trả về kết quả cho người dùng (ví dụ: redirect hoặc view).
         */

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = httpClientFactory.CreateClient();

            var response = await client.GetFromJsonAsync<RegionDto>($"https://localhost:7027/api/regions/{id.ToString()}");

            if (response is not null)
            {
                return View(response);
            }
            return View(null);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(RegionDto request)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7027/api/regions/{request.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

            if (response is not null)
            {
                return RedirectToAction("index", "Regions");
            }

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Delete(RegionDto request)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7027/api/regions/{request.Id}");

                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("index", "Regions");
            }
            catch (Exception ex)
            {
                // Log the exception
                throw;
            }

            return View("Edit");
        }
    }
}
