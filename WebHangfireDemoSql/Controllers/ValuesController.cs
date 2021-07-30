using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace WebHangfireDemoSql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        [Route("[action]")]
        public IActionResult DiscountMail()
        {
            var jobId = BackgroundJob.Schedule(() => SendDiscountMail("Welcome To Ootty Nice to Meet You...!!"), TimeSpan.FromSeconds(45));
            return Ok($"Job Id : {jobId}, Send discount mail to customer");
        }

        public void SendDiscountMail(string message)
        {
            Console.WriteLine(message);
        }

    }
}
