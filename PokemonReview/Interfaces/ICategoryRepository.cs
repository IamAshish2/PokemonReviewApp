﻿using PokemonReview.Models;

namespace PokemonReview.Interfaces
{
    public interface ICategoryRepository 
    {
        ICollection<Category> GetCategories();

        Category GetCategory(int id);
        ICollection<Pokemon> GetPokemonByCategory(int CategoryId);
        bool CategoryExists(int id);

        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
    }
}
