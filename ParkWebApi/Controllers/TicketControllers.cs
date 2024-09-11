using Microsoft.AspNetCore.Mvc;
using DataModels;
using System.Linq;
using LinqToDB;

namespace ParkWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly P684612TemaDB _context;

        public TicketsController(P684612TemaDB context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetTickets()
        {
            var tickets = _context.Tickets
                .Select(t => new TicketDto
                {
                    TicketId = t.TicketId,
                    ClientId = t.ClientId,
                    Status = t.Status,
                    DatePayment = t.DatePayment,
                    // You may add more properties here based on your requirements
                })
                .ToList();

            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public IActionResult GetTicket(int id)
        {
            var ticket = _context.Tickets
                .Where(t => t.TicketId == id)
                .Select(t => new TicketDto
                {
                    TicketId = t.TicketId,
                    ClientId = t.ClientId,
                    Status = t.Status,
                    DatePayment = t.DatePayment,
                    // You may add more properties here based on your requirements
                })
                .FirstOrDefault();

            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }

        [HttpGet("client/{clientId}")]
        public IActionResult GetClientTickets(int clientId)
        {
            var tickets = _context.Tickets
                .Where(t => t.ClientId == clientId)
                .Select(t => new TicketWithItemsDto
                {
                    TicketId = t.TicketId,
                    ClientId = t.ClientId,
                    Status = t.Status,
                    DatePayment = t.DatePayment,
                    TicketItems = t.TicketItemibfks.Select(ti => new TicketitemDto
                    {
                        TicketItemId = ti.TicketItemId,
                        TicketId = ti.TicketId,
                        AttractionId = ti.AttractionId,
                        Quantity = ti.Quantity,
                        AttractionName = ti.Attraction.NameAttraction, 
                        Price = ti.Attraction.Price
                    }).ToList()
                })
                .ToList();

            if (tickets == null || tickets.Count == 0)
            {
                return NotFound();
            }

            return Ok(tickets);
        }

       

        [HttpPost]
        public IActionResult PostTicket([FromBody] TicketDto ticketDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ticket = new Ticket
            {
                ClientId = ticketDto.ClientId,
                Status = ticketDto.Status,
                DatePayment = ticketDto.DatePayment,
                // You may add more properties here based on your requirements
            };

            _context.Insert(ticket);
            ticketDto.TicketId = ticket.TicketId;

            return CreatedAtAction(nameof(GetTicket), new { id = ticket.TicketId }, ticketDto);
        }

        [HttpPost("client/{clientId}/tickets")]
        public IActionResult PostClientTicket(int clientId, [FromBody] CreateTicketDto createTicketDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ticket = new Ticket
            {
                ClientId = clientId,
                Status = createTicketDto.Status,
                DatePayment = createTicketDto.DatePayment,
            };

            try
            {
                // Вставка записи Ticket и получение сгенерированного TicketId
                var insertedTicketId = _context.InsertWithInt32Identity(ticket);
                ticket.TicketId = insertedTicketId;

                // Убедиться, что TicketId присвоен корректно
                if (ticket.TicketId <= 0)
                {
                    return StatusCode(500, "Ошибка генерации TicketId");
                }

                // Вставка записей TicketItem
                foreach (var item in createTicketDto.TicketItems)
                {
                    var ticketItem = new Ticketitem
                    {
                        TicketId = ticket.TicketId,
                        AttractionId = item.AttractionId,
                        Quantity = item.Quantity
                    };

                    _context.Insert(ticketItem);
                }

                return Ok(); // Возврат кода состояния 200
            }
            catch (Exception ex)
            {
                // Логирование исключения (используйте логгер в реальном приложении)
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult PutTicket(int id, [FromBody] TicketDto ticketDto)
        {
            if (id != ticketDto.TicketId || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ticket = _context.Tickets.Find(id);
            if (ticket == null)
            {
                return NotFound();
            }

            ticket.ClientId = ticketDto.ClientId;
            ticket.Status = ticketDto.Status;
            ticket.DatePayment = ticketDto.DatePayment;
            // Update other properties based on your requirements

            _context.Update(ticket);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTicket(int id)
        {
            var ticket = _context.Tickets.Find(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Delete(ticket);
            return NoContent();
        }
    }
}
