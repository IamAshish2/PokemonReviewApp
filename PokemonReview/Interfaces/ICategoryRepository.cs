using PokemonReview.Models;

namespace PokemonReview.Interfaces
{
    public interface ICategoryRepository 
    {
        ICollection<Category> GetCategories();

        Category GetCategory(int id);
        ICollection<Pokemon> GetPokemonByCategory(int CategoryId);
        bool CategoryExists(int id);
    }
}
