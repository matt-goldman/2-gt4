﻿using CloudyMobile.Client;
using CloudyMobile.Maui.Services.Concretions;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CloudyMobile.Maui.ViewModels
{
    public class SearchRecipeViewModel : BaseViewModel
    {
        private readonly RecipeService recipeService;

        public string NameSearchTerm { get; set; }
        public string StyleSearchTerm { get; set; }

        public List<RecipeDto> Results { get; set; } = new List<RecipeDto>();

        public RecipeDto RecipeDetails { get; set; }

        public bool ShowRecipeDetails { get; set; } = false;

        public bool RecipeDetailsVisible { get; set; } = false;


        public ICommand SearchButtonCommand { get; set; }

        public ICommand ViewRecipeDetailsCommand { get; set; }

        public ICommand RecipeSelectedCommand { get; set; }

        public ICommand HideRecipeDetailsCommand { get; set; }

        public SearchRecipeViewModel(RecipeService recipeService)
        {
            this.recipeService = recipeService;
            SearchButtonCommand = new Command(async () => await UpdateSearchResults());
            ViewRecipeDetailsCommand = new Command<RecipeDto>((recipe) => ViewRecipeDetails(recipe));
            HideRecipeDetailsCommand = new Command(() => { ShowRecipeDetails = false; RaisePropertyChanged(nameof(ShowRecipeDetails)); });
            Results.Add(new RecipeDto
            {
                Name = "Test Recipe",
                Style = new BeerStyleDto
                {
                    Name = "Test Style"
                }
            });
        }

        public async Task UpdateSearchResults()
        {
            Console.WriteLine("Getting recipe search results");

            IsBusy = true;
            RaisePropertyChanged(nameof(IsBusy));

            try
            {
                Results.Clear();
                RaisePropertyChanged(nameof(Results));
                var results = await recipeService.SearchRecipes(NameSearchTerm, StyleSearchTerm);

                results.Recipes.ForEach(recipe => Results.Add(recipe));
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Failed to get recipes");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine($"Got {Results.Count} recipes");

            IsBusy = false;
            RaisePropertyChanged(nameof(IsBusy));

            RaisePropertyChanged(nameof(Results));
        }

        public void ViewRecipeDetails(RecipeDto recipe)
        {
            Console.WriteLine($"Recipe {recipe.Name} tapped");
            RecipeDetails = recipe;
            ShowRecipeDetails = true;
            RaisePropertyChanged(nameof(RecipeDetails), nameof(ShowRecipeDetails));
        }

        public async Task RecipeSelected()
        {
            MessagingCenter.Send<object, int>(this, "RecipeSelected", RecipeDetails.Id);
            await Navigation.PopAsync();
        }
    }
}