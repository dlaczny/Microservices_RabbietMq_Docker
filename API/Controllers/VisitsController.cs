using AutoMapper;
using API;
using API.Models;
using API.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class VisitsController : Controller
    {
        private readonly AppDbContext _context;

        private readonly IMessageBusRepository _queue;

        public VisitsController(AppDbContext context, IMessageBusRepository queue)
        {
            _context = context;
            _context.Database.EnsureCreated();
            _queue = queue;
        }

        [HttpGet]
        public ActionResult<int> CountOfProcessedFiles()
        {
            var count = _context.Visits.Count();
            return Ok(count);
        }

        [HttpGet("country")]
        public ActionResult<int> CountOfVisitsByCountry(string country)
        {
            var countryVisits = _context.Visits.Where(x => x.Country == country).Select(x => x.Visitors).Sum();
            return Ok(countryVisits);
        }

        [HttpPost]
        public async Task<IActionResult> GetFileAsync(VisitCreateDto visitDto)
        {
            if (visitDto.Visitors == 0)
            {
                return BadRequest("Visitors count invalid");
            }
            if (!System.IO.Directory.Exists("../var/Data"))
                System.IO.Directory.CreateDirectory("../var/Data");

            var guid = Guid.NewGuid();
            using (StreamWriter file = System.IO.File.CreateText(@"../var/Data/" + guid + ".json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, visitDto);
            }

            await _queue.AddMessage(guid.ToString());
            return Ok();
        }
    }
}