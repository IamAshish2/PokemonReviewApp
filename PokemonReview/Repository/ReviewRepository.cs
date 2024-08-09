using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokemonReview.Data;
using PokemonReview.Interfaces;
using PokemonReview.Models;

namespace PokemonReview.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context)
        {
            _context = context;
        }
        public Review GetReview(int id) => _context.Reviews.Where(r => r.Id == id).FirstOrDefault();

        public ICollection<Review> GetReviewOfPokemon(int pokeId) => _context.Reviews.Where(r => r.Pokemon.Id == pokeId).ToList();

        public ICollection<Review> GetReviews() => _context.Reviews.OrderBy(r => r.Id).ToList();

        public bool HasReview(int id) => _context.Reviews.Any(r => r.Id == id);
    }
}
