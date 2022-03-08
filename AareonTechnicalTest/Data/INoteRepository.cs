using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AareonTechnicalTest.Models;

namespace AareonTechnicalTest.Data
{
    public interface INoteRepository
    {

        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
        Task<Note[]> GetAllNotesAsync();
        Task<Note> GetNoteAsync(int id);

    }
}
