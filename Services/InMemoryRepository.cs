using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoviesAPI.Entities;

namespace MoviesAPI.Services
{
    public class InMemoryRepository: IRepository
    {
        private List<Genre> _genres;
        
        public InMemoryRepository()
        {
            _genres = new List<Genre>()
            {
                new Genre() {Id = 1, Name = "Comedy"},
                new Genre() {Id = 2, Name = "Action"}
            };
        }

        public async Task<List<Genre>> GetAllGenres()
        {
            await Task.Delay(3000);
            return _genres;
        }

        public Genre GetGenreById(int id)
        {
            return _genres.FirstOrDefault(x => x.Id == id);
        }

        public void AddGenre(Genre genre)
        {
            genre.Id = _genres.Max(x => x.Id) + 1;
            _genres.Add(genre);
        }
    }
}