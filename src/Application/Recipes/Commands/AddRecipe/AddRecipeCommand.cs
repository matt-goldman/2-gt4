﻿using CloudyMobile.Application.Common.Interfaces;
using CloudyMobile.Application.Recipes.Common;
using CloudyMobile.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CloudyMobile.Application.Recipes.Commands.AddRecipe
{
    public class AddRecipeCommand : IRequest<int>
    {
        public RecipeDto viewModel { get; set; }  
    }

    public class AddRecipeCommandHandler : IRequestHandler<AddRecipeCommand, int>
    {
        public IApplicationDbContext _context;

        public AddRecipeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AddRecipeCommand request, CancellationToken cancellationToken)
        {
            var entity = new Recipe
            {
                Name = request.viewModel.Name,
                MassUnits = request.viewModel.MassUnits,
                TemperatureUnit = request.viewModel.TemperatureUnit,
                LiquidUnits = request.viewModel.LiquidUnits,
                Notes = request.viewModel.Notes
            };

            var style = await _context.Styles
                .Where(s => s.Id == request.viewModel.Id)
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken);

            if(style is null)
            {
                style = new Style
                {
                    ImageUrl = request.viewModel.Style.ImageUrl,
                    Name = request.viewModel.Style.Name,
                    Notes = request.viewModel.Notes
                };

                _context.Styles.Add(style);
            }

            entity.Style = style;

            _context.Recipes.Add(entity);

            if(request.viewModel.Ingredients != null && request.viewModel.Ingredients.Count > 0)
            {
                request.viewModel.Ingredients.ForEach(async i =>
                {
                    await _context.RecipeIngredients.AddAsync(new RecipeIngredients
                    {
                        Recipe = entity,
                        IngredientId = i.IngredientId,
                        Quantity = i.Quantity
                    }, cancellationToken);
                });
            }

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
