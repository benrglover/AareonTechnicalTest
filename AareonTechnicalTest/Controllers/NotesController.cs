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
    public class NotesController : ControllerBase
    {

        private readonly INoteRepository _repository;
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public NotesController(INoteRepository repository, IPersonRepository personRepository, IMapper mapper, LinkGenerator linkGenerator)
        {

            _repository = repository;
            _personRepository = personRepository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;

        }

        [HttpGet]
        public async Task<ActionResult<Note[]>> Get()
        {

            try
            {

                var results = await _repository.GetAllNotesAsync();

                return results;

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> Get(int id)
        {

            try
            {

                var result = await _repository.GetNoteAsync(id);

                if (result == null) return NotFound();

                return result;

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

        }

        [HttpPost]
        public async Task<ActionResult<Note>> Post(Note model)
        {

            try
            {
                // extra validation to check if Id has been used before
                var existing = await _repository.GetNoteAsync(model.Id);
                if (existing != null)
                {
                    return BadRequest("Note Id in Use");
                }

                //Create a new Note
                var note = _mapper.Map<Note>(model);
                _repository.Add(note);
                if (await _repository.SaveChangesAsync())
                {

                    return await _repository.GetNoteAsync(note.Id);

                }

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Note>> Put(int id, Note model)
        {

            try
            {

                var oldNote = await _repository.GetNoteAsync(id);

                if (oldNote == null) return NotFound($"Could not find Note with Id of {id}");

                _mapper.Map(model, oldNote);

                // save
                if (await _repository.SaveChangesAsync())
                {
                    return await _repository.GetNoteAsync(id);
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

                var oldNote = await _repository.GetNoteAsync(id);

                if (oldNote == null) return NotFound($"Could not find Note with Id of {id}");

                if (IsPersonAdmin(oldNote.PersonId).Result)
                {

                    _repository.Delete(oldNote);

                    if (await _repository.SaveChangesAsync())
                    {
                        return Ok();
                    }

                }
                else
                {

                    return Conflict($"This Person could not delete this note with PersonID of " + oldNote.PersonId.ToString());

                }

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest("Failed to delete the Note");

        }

        private async Task<bool> IsPersonAdmin(int id)
        {

            bool isAdmin = false;

            var person = await _personRepository.GetPersonAsync(id);

            isAdmin = person.IsAdmin;

            return isAdmin;

        }

    }
}
