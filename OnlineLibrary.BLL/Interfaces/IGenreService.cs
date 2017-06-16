using OnlineLibrary.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.BLL.Interfaces
{
    public interface IGenreService
    {
        void Create(GenreDTO genreDto);
        void Delete(int id);
        void Update(GenreDTO bookDto);
        GenreDTO GetGenre(int? id);
        List<GenreDTO> GetGenres();
        void Dispose();
    }
}
