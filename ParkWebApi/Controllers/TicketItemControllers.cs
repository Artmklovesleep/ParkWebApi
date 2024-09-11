using Microsoft.AspNetCore.Mvc;
using DataModels;
using System.Linq;
using LinqToDB;

namespace ParkWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketItemsController : ControllerBase
    {
        private readonly P684612TemaDB _context;

        public TicketItemsController(P684612TemaDB context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetTicketItems()
        {
            var ticketItems = _context.Ticketitems.ToList();
            return Ok(ticketItems);
        }

        [HttpGet("{id}")]
        public IActionResult GetTicketItem(int id)
        {
            var ticketItem = _context.Ticketitems.Find(id);
            if (ticketItem == null)
            {
                return NotFound();
            }
            return Ok(ticketItem);
        }

        [HttpPost]
        public IActionResult PostTicketItem([FromBody] TicketitemDto ticketItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ticketItem = new Ticketitem
            {
                TicketId = ticketItemDto.TicketId,
                AttractionId = ticketItemDto.AttractionId,
                Quantity = ticketItemDto.Quantity
            };

            _context.Insert(ticketItem);
            ticketItemDto.TicketItemId = ticketItem.TicketItemId;

            return CreatedAtAction(nameof(GetTicketItem), new { id = ticketItem.TicketItemId }, ticketItemDto);
        }

        [HttpPut("{id}")]
        public IActionResult PutTicketItem(int id, [FromBody] TicketitemDto ticketItemDto)
        {
            if (id != ticketItemDto.TicketItemId || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ticketItem = _context.Ticketitems.Find(id);
            if (ticketItem == null)
            {
                return NotFound();
            }

            ticketItem.TicketId = ticketItemDto.TicketId;
            ticketItem.AttractionId = ticketItemDto.AttractionId;
            ticketItem.Quantity = ticketItemDto.Quantity;

            _context.Update(ticketItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTicketItem(int id)
        {
            var ticketItem = _context.Ticketitems.Find(id);
            if (ticketItem == null)
            {
                return NotFound();
            }

            _context.Delete(ticketItem);
            return NoContent();
        }
    }
}
