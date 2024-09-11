using Microsoft.AspNetCore.Mvc;
using DataModels;
using System.Linq;
using LinqToDB;

namespace ParkWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly P684612TemaDB _context;

        public ClientsController(P684612TemaDB context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetClients()
        {
            var clients = _context.Clients
                .Select(c => new ClientDto
                {
                    ClientId = c.ClientId,
                    Surname = c.Surname,
                    Name = c.Name,
                    Lastname = c.Lastname,
                    Birthday = c.Birthday,
                    Gender = c.Gender,
                    Phone = c.Phone,
                    Email = c.Email
                }).ToList();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public IActionResult GetClient(int id)
        {
            var client = _context.Clients
                .Where(c => c.ClientId == id)
                .Select(c => new ClientDto
                {
                    ClientId = c.ClientId,
                    Surname = c.Surname,
                    Name = c.Name,
                    Lastname = c.Lastname,
                    Birthday = c.Birthday,
                    Gender = c.Gender,
                    Phone = c.Phone,
                    Email = c.Email
                })
                .FirstOrDefault();

            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpGet("by-email/{email}")]
        public IActionResult GetClientByEmail(string email)
        {
            var client = _context.Clients
                .Where(c => c.Email == email)
                .Select(c => new ClientDto
                {
                    ClientId = c.ClientId,
                    Surname = c.Surname,
                    Name = c.Name,
                    Lastname = c.Lastname,
                    Birthday = c.Birthday,
                    Gender = c.Gender,
                    Phone = c.Phone,
                    Email = c.Email
                })
                .FirstOrDefault();

            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpPost]
        public IActionResult PostClient([FromBody] ClientDto clientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = new Client
            {
                Surname = clientDto.Surname,
                Name = clientDto.Name,
                Lastname = clientDto.Lastname,
                Birthday = clientDto.Birthday,
                Gender = clientDto.Gender,
                Phone = clientDto.Phone,
                Email = clientDto.Email
            };

            _context.Insert(client);
            clientDto.ClientId = client.ClientId;

            return CreatedAtAction(nameof(GetClient), new { id = client.ClientId }, clientDto);
        }

        [HttpPut("{id}")]
        public IActionResult PutClient(int id, [FromBody] ClientDto clientDto)
        {
            if (!ModelState.IsValid || id != clientDto.ClientId)
            {
                return BadRequest(ModelState);
            }

            var client = _context.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            client.Surname = clientDto.Surname;
            client.Name = clientDto.Name;
            client.Lastname = clientDto.Lastname;
            client.Birthday = clientDto.Birthday;
            client.Gender = clientDto.Gender;
            client.Phone = clientDto.Phone;
            client.Email = clientDto.Email;

            _context.Update(client);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteClient(int id)
        {
            var client = _context.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Delete(client);
            return NoContent();
        }
    }
}
