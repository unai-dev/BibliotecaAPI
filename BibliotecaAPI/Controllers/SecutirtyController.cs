using BibliotecaAPI.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/security")]
    public class SecutirtyController: ControllerBase
    {
        private readonly IDataProtector protector;
        private readonly ITimeLimitedDataProtector protectorTimeLimit;
        private readonly IHashService hashService;

        public SecutirtyController(IDataProtectionProvider protectionProvider, IHashService hashService)
        {
            protector = protectionProvider.CreateProtector("SecurityController");
            protectorTimeLimit = protector.ToTimeLimitedDataProtector();
            this.hashService = hashService;
        }

        [HttpGet("hash")]
        public ActionResult Hash(string plainText)
        {
            var hash = hashService.Hash(plainText);
            var result = new { plainText, hash };
            return Ok(result);
                
        }

        [HttpGet("encript-time")]
        public ActionResult EncriptTime(string text)
        {
            string cifred = protectorTimeLimit.Protect(text, lifetime: TimeSpan.FromSeconds(30));

            return Ok(new { cifred });
        }

        [HttpGet("desencriptar-time")]
        public ActionResult DesencriptarTime(string cifred)
        {
            string text = protectorTimeLimit.Unprotect(cifred);

            return Ok(new { text });
        }

        [HttpGet("encript")]
        public ActionResult Encript(string text)
        {
            string cifred = protector.Protect(text);

            return Ok(new { cifred });
        }

        [HttpGet("desencriptar")]
        public ActionResult Desencriptar(string cifred)
        {
            string text = protector.Unprotect(cifred);

            return Ok(new { text });
        }
    }
}
