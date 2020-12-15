using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NewAPI.model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NewAPI.Controllers
{
    public class HomeController : Controller
    {
        static HttpClient client = new HttpClient();

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Secret()
        {
            var url = "https://run.mocky.io/v3/d8b7f392-cb74-42ef-b4fd-2fb1177906b0";
            HttpResponseMessage response = await client.GetAsync(url);
            var result = new List<Persons>();

            if (response.IsSuccessStatusCode)
            {
                result = await JsonSerializer.DeserializeAsync<List<Persons>>(await response.Content.ReadAsStreamAsync()); //response.Content.ReadAsAsync<object>();
            }

            //return result;

            return View(result);
        }

        public IActionResult Authenticate()
        {

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "some_id"),
                new Claim("grandpa","cookie")
            };

            var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);

            var key = new SymmetricSecurityKey(secretBytes);

            var algorithm = SecurityAlgorithms.HmacSha256;

            var signingCredentials = new SigningCredentials(key,algorithm );

            var token = new JwtSecurityToken(
                Constants.Issuer,
                Constants.Audience,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(2),
                signingCredentials);

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { access_token = tokenJson });
        }
    }
}
