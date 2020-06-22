using System.Collections.Generic;
using System.Threading.Tasks;
using MoviesAPI.Entities;

namespace MoviesAPI.Services
{
    public interface IRepository
    {
        Task<List<Genre>> GetAllGenres();
        Genre GetGenreById(int id);

        void AddGenre(Genre genre);
    }
}