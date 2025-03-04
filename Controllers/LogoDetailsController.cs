using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace LogoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogoDetailsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public LogoDetailsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet(Name = "GetLogoPricing")]
        public async Task<ActionResult<CompanyLogoDTO>> Get(string companyName)
        {
            List<CompanyLogoDTO> companyLogoDetails = [];
            try
            {
                var client = new HttpClient();
                string requestURL = "https://api.logo.dev/search?q=" + companyName;
                string secretKey = _configuration.GetValue<string>("secretkey");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secretKey);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestURL);
                var response = await client.SendAsync(httpRequest);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    companyLogoDetails = JsonConvert.DeserializeObject<List<CompanyLogoDTO>>(content);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Ok(companyLogoDetails);
        }

        public class CompanyLogoDTO
        {
            //public int? id { get; set; }
            public string? name { get; set; }
            public string? logo_url { get; set; }
        }
    }
}
