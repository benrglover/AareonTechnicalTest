using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AareonTechnicalTest.Data;
using AareonTechnicalTest.Models;
using AutoMapper;
using Microsoft.AspNetCore.Routing;

namespace AareonTechnicalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {

        private readonly ITicketRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public TicketsController(ITicketRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {

            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            
        }

        [HttpGet]
        public async Task<ActionResult<Ticket[]>> Get()
        {

            try
            {

                var results = await _repository.GetAllTicketsAsync();
                
                return results;

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> Get(int id)
        {

            try
            {

                var result = await _repository.GetTicketAsync(id);

                if (result == null) return NotFound();

                return result;

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

        }

        [HttpPost]
        public async Task<ActionResult<Ticket>> Post(Ticket model)
        {

            try
            {
                // extra validation to check if Id has been used before
                var existing = await _repository.GetTicketAsync(model.Id);
                if (existing != null)
                {
                    return BadRequest("Ticket Id in Use");
                }

                //Create a new Ticket
                var ticket = _mapper.Map<Ticket>(model);
                _repository.Add(ticket);
                if (await _repository.SaveChangesAsync())
                {

                    return await _repository.GetTicketAsync(ticket.Id);

                }

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Ticket>> Put(int id, Ticket model)
        {

            try
            {

                var oldTicket = await _repository.GetTicketAsync(id);

                if (oldTicket == null) return NotFound($"Could not find Ticket with Id of {id}");

                _mapper.Map(model, oldTicket);

                // save
                if (await _repository.SaveChangesAsync())
                {
                    return await _repository.GetTicketAsync(id);
                }

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {

                var oldTicket = await _repository.GetTicketAsync(id);

                if (oldTicket == null) return NotFound($"Could not find Ticket with Id of {id}");

                _repository.Delete(oldTicket);

                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest("Failed to delete the Ticket");

        }

    }
}
