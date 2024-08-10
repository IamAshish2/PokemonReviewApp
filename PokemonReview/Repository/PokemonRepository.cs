using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PokemonReview.Data;
//using PokemonReview.Dtos;
using PokemonReview.Interfaces;
using PokemonReview.Models;

namespace PokemonReview.Repository
{
    public class PokemonRepository:IPokemonRepository
    {
        private readonly DataContext _context;

        public PokemonRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokemonOwner = _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
            var pokemonCategory = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

            var ownerOfPokemon = new PokemonOwner
            {
                Owner = pokemonOwner,
                Pokemon = pokemon
            };
            _context.Add(ownerOfPokemon);

            var categoryOfPokemon = new PokemonCategory
            {
                Category = pokemonCategory,
                Pokemon = pokemon
            };
            _context.Add(categoryOfPokemon);
            _context.Add(pokemon);
            return Save();
        }

        public Pokemon GetPokemon(int id)
        {
            return _context.Pokemons.Where(p => p.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _context.Pokemons.Where(p => p.Name == name).FirstOrDefault();
        }

       //Check this later 
       public decimal GetPokemonRating(int pokeId)
        {
            var review = _context.Reviews.Where(p => p.Pokemon.Id == pokeId);

            if (review.Count() <= 0)
                return 0;

            return ((decimal)review.Sum(r => r.Rating) / review.Count());
        }

        
        public ICollection<Pokemon> GetPokemons() {

            return _context.Pokemons.OrderBy(p => p.Id).ToList();
        }

        public bool PokemonExists(int pokeId)
        {
            return _context.Pokemons.Any( p => p.Id == pokeId); 
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
