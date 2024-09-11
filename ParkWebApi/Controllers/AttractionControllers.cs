using Microsoft.AspNetCore.Mvc;
using DataModels;

using System.Linq;
using LinqToDB;

namespace ParkWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttractionsController : ControllerBase
    {
        private readonly P684612TemaDB _context;

        public AttractionsController(P684612TemaDB context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAttractions()
        {
            var attractions = _context.Attractions
                .Select(a => new AttractionDto
                {
                    AttractionId = a.AttractionId,
                    NameAttraction = a.NameAttraction,
                    Capacity = a.Capacity,
                    Price = a.Price,
                    Description = a.Description
                })
                .ToList();

            return Ok(attractions);
        }

        [HttpGet("{id}")]
        public IActionResult GetAttraction(int id)
        {
            var attraction = _context.Attractions
                .Where(a => a.AttractionId == id)
                .Select(a => new AttractionDto
                {
                    AttractionId = a.AttractionId,
                    NameAttraction = a.NameAttraction,
                    Capacity = a.Capacity,
                    Price = a.Price,
                    Description = a.Description
                })
                .FirstOrDefault();

            if (attraction == null)
            {
                return NotFound();
            }

            return Ok(attraction);
        }

        [HttpPost]
        public IActionResult PostAttraction([FromBody] AttractionDto attractionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var attraction = new Attraction
            {
                NameAttraction = attractionDto.NameAttraction,
                Capacity = attractionDto.Capacity,
                Price = attractionDto.Price,
                Description = attractionDto.Description
            };

            _context.Insert(attraction);
            attractionDto.AttractionId = attraction.AttractionId;

            return CreatedAtAction(nameof(GetAttraction), new { id = attraction.AttractionId }, attractionDto);
        }

        [HttpPut("{id}")]
        public IActionResult PutAttraction(int id, [FromBody] AttractionDto attractionDto)
        {
            if (id != attractionDto.AttractionId || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var attraction = _context.Attractions.Find(id);
            if (attraction == null)
            {
                return NotFound();
            }

            attraction.NameAttraction = attractionDto.NameAttraction;
            attraction.Capacity = attractionDto.Capacity;
            attraction.Price = attractionDto.Price;
            attraction.Description = attractionDto.Description;

            _context.Update(attraction);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAttraction(int id)
        {
            var attraction = _context.Attractions.Find(id);
            if (attraction == null)
            {
                return NotFound();
            }

            _context.Delete(attraction);
            return NoContent();
        }
    }
}
