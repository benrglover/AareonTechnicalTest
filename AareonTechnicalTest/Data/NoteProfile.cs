using AareonTechnicalTest.Models;
using AutoMapper;

namespace AareonTechnicalTest.Data
{
    public class NoteProfile: Profile
    {

        public NoteProfile()
        {
            this.CreateMap<Note, Note>()
                .ReverseMap();

        }

    }
}
