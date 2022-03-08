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
    public class PersonsController : ControllerBase
    {

        private readonly IPersonRepository _repository;
        
        public PersonsController(IPersonRepository repository)
        {

            _repository = repository;
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> Get(int id)
        {

            try
            {

                var result = await _repository.GetPersonAsync(id);

                if (result == null) return NotFound();

                return result;

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

        }

    }
}
