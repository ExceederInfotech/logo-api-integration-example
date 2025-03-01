using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace LogoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogoDetailsController : ControllerBase
    {
        [HttpGet(Name = "GetLogoPricing")]
        public async Task<ActionResult<CompanyLogoDTO>> Get(string companyName)
        {
            List<CompanyLogoDTO> companyLogoDetails = [];
            try
            {
                var client = new HttpClient();
                string requestURL = "https://api.logo.dev/search?q=" + companyName;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "sk_UrDS6e_0TNqw_nE33Ut5vQ");
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
