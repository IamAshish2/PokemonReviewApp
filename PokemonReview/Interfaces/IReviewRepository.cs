using PokemonReview.Models;

namespace PokemonReview.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int id);
        ICollection<Review> GetReviewOfPokemon(int pokeId);
        bool HasReview(int id);
    }
}
