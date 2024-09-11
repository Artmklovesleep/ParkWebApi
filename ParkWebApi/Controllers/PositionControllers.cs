using Microsoft.AspNetCore.Mvc;
using DataModels;
using System.Linq;
using LinqToDB;

namespace ParkWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly P684612TemaDB _context;

        public PositionsController(P684612TemaDB context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetPositions()
        {
            var positions = _context.Positions
                .Select(p => new PositionDto
                {
                    PositionId = p.PositionId,
                    NamePosition = p.NamePosition
                })
                .ToList();

            return Ok(positions);
        }

        [HttpGet("{id}")]
        public IActionResult GetPosition(int id)
        {
            var position = _context.Positions
                .Where(p => p.PositionId == id)
                .Select(p => new PositionDto
                {
                    PositionId = p.PositionId,
                    NamePosition = p.NamePosition
                })
                .FirstOrDefault();

            if (position == null)
            {
                return NotFound();
            }

            return Ok(position);
        }

        [HttpPost]
        public IActionResult PostPosition([FromBody] PositionDto positionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var position = new Position
            {
                NamePosition = positionDto.NamePosition
            };

            _context.Insert(position);
            positionDto.PositionId = position.PositionId;

            return CreatedAtAction(nameof(GetPosition), new { id = position.PositionId }, positionDto);
        }

        [HttpPut("{id}")]
        public IActionResult PutPosition(int id, [FromBody] PositionDto positionDto)
        {
            if (id != positionDto.PositionId || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var position = _context.Positions.Find(id);
            if (position == null)
            {
                return NotFound();
            }

            position.NamePosition = positionDto.NamePosition;

            _context.Update(position);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePosition(int id)
        {
            var position = _context.Positions.Find(id);
            if (position == null)
            {
                return NotFound();
            }

            _context.Delete(position);
            return NoContent();
        }
    }
}
