using EncurtadorDeUrl.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static EncurtadorDeUrl.Services.EncurtadorUrlService;

namespace EncurtadorDeUrl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncurtadorUrlController : ControllerBase
    {
        private readonly IEncurtadorUrlService _encurtadorUrlService;

        public EncurtadorUrlController (IEncurtadorUrlService encurtadorUrlService)
        {
            _encurtadorUrlService = encurtadorUrlService;
        }

        [HttpPost]
        public ActionResult ShortenURL([FromBody] string originalURL)
        {
            var (id, shortURL, expiration) = _encurtadorUrlService.EncurtaURL(originalURL);
            return Ok(new
            {
                ID = id,
                ShortURL = shortURL,
                Expiration = expiration
            });
        }

        [HttpGet]
        public ActionResult GetOriginalURL(string shortURL)
        {
            try
            {
                string originalURL = _encurtadorUrlService.GetOriginalURL(shortURL);
                return Ok(originalURL);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
