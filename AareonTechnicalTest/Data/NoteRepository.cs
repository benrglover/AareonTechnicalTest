using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AareonTechnicalTest.Models;

namespace AareonTechnicalTest.Data
{
    public class NoteRepository: INoteRepository
    {

        private readonly ApplicationContext _context;
        private readonly ILogger<NoteRepository> _logger;

        public NoteRepository(ApplicationContext context, ILogger<NoteRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Add<T>(T entity) where T : class
        {
            _logger.LogInformation($"Adding an object of type {entity.GetType()} to the context.");
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _logger.LogInformation($"Removing an object of type {entity.GetType()} to the context.");
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            _logger.LogInformation($"Attempting to save the changes in the context");

            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Note[]> GetAllNotesAsync()
        {
            _logger.LogInformation($"Getting all Notes");

            IQueryable<Note> query = _context.Notes;

            query = query.OrderByDescending(c => c.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Note> GetNoteAsync(int id)
        {
            _logger.LogInformation($"Getting a Note for {id}");

            IQueryable<Note> query = _context.Notes;

            query = query.Where(c => c.Id == id);

            return await query.FirstOrDefaultAsync();
        }

    }
}
