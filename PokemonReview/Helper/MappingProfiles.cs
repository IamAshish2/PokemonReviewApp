
using AutoMapper;
using PokemonReview.Dtos;
using PokemonReview.Models;

namespace PokemonReview.Helper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Pokemon, PokemonDto>();
<<<<<<< HEAD
            CreateMap<Category, CategoryDto>();
=======
>>>>>>> master
        }
    }
}
