using Microsoft.AspNetCore.Mvc;
using NewAPI.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace NewAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleDataController : ControllerBase
    {
        static HttpClient client = new HttpClient();
        // GET: api/<SampleDataController>
        [HttpGet]
        public async Task<List<Persons>> GetAsync()
        {
            var url = "https://run.mocky.io/v3/d8b7f392-cb74-42ef-b4fd-2fb1177906b0";
            HttpResponseMessage response = await client.GetAsync(url);
            var result = new List<Persons>();

            if(response.IsSuccessStatusCode)
            {
                result = await JsonSerializer.DeserializeAsync<List<Persons>>(await response.Content.ReadAsStreamAsync()); //response.Content.ReadAsAsync<object>();
            }

            return result;
        }

        // GET api/<SampleDataController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
 
    }
}
