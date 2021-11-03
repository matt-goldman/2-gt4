﻿using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Extensions.DependencyInjection;
using IdentityModel.OidcClient.Browser;
using DevHops.Maui.Helpers;
using DevHops.Maui.Services.Abstractions;
using DevHops.Maui.Services.Concretions;
using DevHops.Maui.ViewModels;
using DevHops.Maui.Pages.Mobile;
using DevHops.Maui.Pages.Shared;
using Maui.Plugins.PageResolver;

namespace DevHops.Maui
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});

			// register services
			builder.Services.AddSingleton<IBrowser, AuthBrowser>();
			builder.Services.AddSingleton<RetryHandler>();
			builder.Services.AddSingleton<IAuthService, AuthService>();
			builder.Services.AddSingleton<IBatchService, BatchService>();
			builder.Services.AddSingleton<IRecipeService, RecipesService>();
			builder.Services.AddSingleton<BaseService>();

			// register viewmodels
			builder.Services.AddSingleton<AddBatchViewModel>();
			builder.Services.AddSingleton<BatchesViewModel>();
			builder.Services.AddSingleton<MainViewModel>();
			builder.Services.AddSingleton<RecipeViewModel>();
			builder.Services.AddSingleton<SearchRecipeViewModel>();
			builder.Services.AddSingleton<AddSampleViewModel>();
			builder.Services.AddSingleton<MainViewModel>();

			// register pages
			builder.Services.AddSingleton<AddBatchPage>();
			builder.Services.AddSingleton<RecipeDetailPage>();
			builder.Services.AddSingleton<SearchRecipePage>();
			builder.Services.AddSingleton<AddSamplePage>();
			builder.Services.AddSingleton<MainPage>();

			// register PageResolver
			builder.Services.UsePageResolver();

			return builder.Build();
		}
	}
}