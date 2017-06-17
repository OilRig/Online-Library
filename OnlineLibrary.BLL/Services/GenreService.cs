using OnlineLibrary.BLL.Interfaces;
using System.Collections.Generic;
using OnlineLibrary.BLL.DTO;
using OnlineLibrary.DAL.Interfaces;
using OnlineLibrary.DAL.Entities;
using AutoMapper;

namespace OnlineLibrary.BLL.Services
{
    public class GenreService : IGenreService
    {
        IUnitOfWork Database { get; set; }
        public GenreService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public void Create(GenreDTO genreDto)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<GenreDTO, Genre>());
            Genre genre = Mapper.Map<GenreDTO, Genre>(genreDto);
            Database.GenreManager.Create(genre);
            Database.SaveAsync();
        }
        public void Delete(int id)
        {
            Database.GenreManager.Delete(id);
            Database.SaveAsync();    
        }
        public GenreDTO GetGenre(int id)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Genre, GenreDTO>());
            GenreDTO genreDto = Mapper.Map<Genre, GenreDTO>(Database.GenreManager.Get(id));
            return genreDto;
        }
        public List<GenreDTO> GetAllGenres()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Genre, GenreDTO>());
            List<GenreDTO> genresDto = Mapper.Map<IEnumerable<Genre>, List<GenreDTO>>(Database.GenreManager.GetAll());
            return genresDto;
        }
        public void Update(GenreDTO genreDto)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<GenreDTO, Genre>());
            Genre genre = Mapper.Map<GenreDTO, Genre>(genreDto);
            Database.GenreManager.Update(genre);
            Database.SaveAsync();
        }
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
